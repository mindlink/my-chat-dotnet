using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using MindLink.Recruitment.MyChat.Tests.ServiceReference1;
using System;
using System.IO;
using System.Text;

namespace MindLink.Recruitment.MyChat.Tests
{
    [TestClass]
    public class WebServiceTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            ServiceReference1.Service1Client client = new ServiceReference1.Service1Client();
            //Console.WriteLine("What Number?: {0}", client.GetData(365));
        }

        [TestMethod]
        public void FileUpload()
        {
            string res;

            IService1 clientUpload = new Service1Client();
            RemoteFileInfo uploadRequestInfo = new RemoteFileInfo();

            using (System.IO.FileStream stream =
                   new System.IO.FileStream(@"E:\upload\chatoriginal.txt", System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                // Upload File
                uploadRequestInfo.FileName = @"E:\upload\chat.txt";
                uploadRequestInfo.Length = @"E:\upload\chat.txt".Length;
                uploadRequestInfo.FileByteStream = stream;
                clientUpload.UploadFile(uploadRequestInfo);

                // Convert into JSON
                string[] str1 = new string[] { "blacklist=pie", "obfuscateUser" };
                res = clientUpload.TriggerConversation(new string[] { @"E:\upload\chat.txt", @"E:\upload\chat.json", "blacklist=pie", "obfuscateUser=true" });
            }

            Console.WriteLine("Arguments: {0}", res.ToString());
        }

        [TestMethod]
        public void FileDownload()
        {
            string value;

            IService1 clientDownload = new Service1Client();
            DownloadRequest downloadrequest = new DownloadRequest();
            downloadrequest.FileName = @"E:\upload\chat.json";

            var res = clientDownload.DownloadFile(downloadrequest);

            using (var reader = new StreamReader(res.FileByteStream, Encoding.UTF8))
            {
                value = reader.ReadToEnd();
            }

            Console.WriteLine("Arguments: {0}\t{1}\t{2}", res.FileName, res.Length, value);
        }
    }
}
