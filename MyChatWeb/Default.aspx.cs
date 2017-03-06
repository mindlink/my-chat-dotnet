using MyChatWeb.ServiceReference1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MyChatLibrary;
using Newtonsoft.Json;

namespace MyChatWeb
{
    public partial class _Default : Page
    {
        /// <summary>
        /// The message content.
        /// </summary>
        public string content;

        /// <summary>
        /// Full Filename with Path
        /// </summary>
        private string Filename;

        /// <summary>
        /// Google Charts Conform formatted User Stats
        /// </summary>
        public string _chartData;


        /// <summary>
        /// Occurs when the server control is loaded into the Page object
        /// </summary>
        /// <param name="sender">
        /// The ID of the sender.
        /// </param>
        /// <param name="e">
        /// Event Arguments 
        /// </param>
        protected void Page_Load(object sender, EventArgs e)
        {
        }


        /// <summary>
        /// Occurs when firing the Upload Button
        /// </summary>
        /// <param name="sender">
        /// The ID of the sender.
        /// </param>
        /// <param name="e">
        /// Event Arguments 
        /// </param>
        protected void uploadFile(object sender, EventArgs e)
        {
            // create a full path & filename string 
            string path = HttpContext.Current.ApplicationInstance.Server.MapPath("~/user_uploads");
            Filename = System.IO.Path.Combine(path, FileUpload1.FileName);

            // open a Service Instance
            IService1 clientUpload = new Service1Client();
            RemoteFileInfo uploadRequestInfo = new RemoteFileInfo();

            // Upload, Convert and Show the results
            Label1.Text = this.readUploadFile();
            this.uploadFile(clientUpload);
            Label2.Text = this.convertToJson(clientUpload);

            // getting datas for the Pie Chart and refresh Page
            _chartData = getChartData();                        
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "google.charts.load('current', { 'packages': ['corechart'] })", true);

            // Show the Chat Table
            this.showChatTable();
        }


        /// <summary>
        /// Helper - getting the Chat Datas from Output File and show in Table
        /// </summary>
        private void showChatTable()
        {
            var serializedConversation = new StreamReader(new FileStream(Filename.Replace(".txt", ".json"), FileMode.Open, FileAccess.Read)).ReadToEnd();
            Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);
            var messages = savedConversation.messages.ToList();

            foreach (var rec in messages)
            {
                TableRow tRow = new TableRow();

                TableCell tCell = new TableCell();
                tCell.Text = string.Format("{0}\t\t {1}\t {2}\n",
                    rec.timestamp.ToString().Substring(0, rec.timestamp.ToString().Length - 7),
                    rec.senderId,
                    rec.content);

                tRow.Cells.Add(tCell);

                Table1.Rows.Add(tRow);
            }
        }


        /// <summary>
        /// Helper - getting the Chart Datas from Web Service
        /// </summary>
        /// <returns>
        /// Google Conform Formatted DataString
        /// </returns>
        private string getChartData()
        {
            try
            {
                IService1 chartData = new Service1Client();
                return chartData.GetChartData(Filename.Replace(".txt", ".json")); ;
            }
            // no File choosen
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Helper - Read File which should be uploaded and return his content
        /// </summary>
        /// <returns>
        /// Content String
        /// </returns>
        private string readUploadFile()
        {
            string res = null;          // file content

            try
            {
                FileUpload1.PostedFile.SaveAs(Filename);
            }
            catch (DirectoryNotFoundException)      // the Directory is not existing, so create on the Server
            {
                string path = HttpContext.Current.ApplicationInstance.Server.MapPath("~/user_uploads");
                System.IO.Directory.CreateDirectory(path);
                FileUpload1.PostedFile.SaveAs(Filename);
            }

            // Read File Content and write out 
            var stream1 = FileUpload1.FileContent;
           
                using (StreamReader reader = new StreamReader(stream1, Encoding.UTF8))
                {
                    res = reader.ReadToEnd().Replace("\n", "<br />");   // replace new lines beacause label don't understand 
                }
            stream1.Close();


            return res;
        }


        /// <summary>
        /// Helper - Convert a myChat file on the server into json 
        /// </summary>
        /// <returns>
        /// Content String in JSON Format
        /// </returns>
        private string convertToJson(IService1 clientUpload)
        {
            var res = clientUpload.TriggerConversation(this.createArgumentList());
            string str = null;

            // Download from Server
            IService1 clientDownload = new Service1Client();
            DownloadRequest downloadrequest = new DownloadRequest();
            downloadrequest.FileName = Filename.Replace(".txt", ".json");

            var res1 = clientDownload.DownloadFile(downloadrequest);

            using (var reader = new StreamReader(res1.FileByteStream, Encoding.UTF8))
            {
                str = reader.ReadToEnd().Replace("\n", "<br />");
            }

            return str;
        }

        /// <summary>
        /// Helper - Upload a File to the Server
        /// </summary>
        private void uploadFile(IService1 clientUpload)
        {
            RemoteFileInfo uploadRequestInfo = new RemoteFileInfo();

            // Prepare and Upload
            uploadRequestInfo.FileName = Filename;
            uploadRequestInfo.Length = Filename.Length;
            uploadRequestInfo.FileByteStream = FileUpload1.FileContent;
            clientUpload.UploadFile(uploadRequestInfo);
        }



        /// <summary>
        /// Helper - create a Argument list based on the selected filters 
        /// </summary>
        /// <returns>
        /// Content String in JSON Format
        /// </returns>
        private string[] createArgumentList()
        {
            List<string> str1 = new List<string>();
            str1.Add(Filename);
            str1.Add(Filename.Replace(".txt", ".json"));

            if (userFilter.Text.CompareTo("") != 0)
            {
                str1.Add("user=" + userFilter.Text);
            }

            if (includingWords.Text.CompareTo("") != 0)
            {
                str1.Add("keyword_include=" + includingWords.Text);
            }

            if (excludingWords.Text.CompareTo("") != 0)
            {
                str1.Add("keyword_exclude=" + excludingWords.Text);
            }

            if (blackList.Text.CompareTo("") != 0)
            {
                str1.Add("blacklist=" + blackList.Text);
            }

            if (creditCard.Checked)
            {
                str1.Add("hideCreditCard");
            }

            if (phoneNumber.Checked)
            {
                str1.Add("hidePhone");
            }
            if (obfuscateUser.Checked)
            {
                str1.Add("obfuscateUser");
            }

            return str1.ToArray(); ;
        }

    }
}
