using MindLink.Recruitment.MyChat.Elements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat.FilesProcessing
{
    public class FileImporter
    {


        /// <summary>
        /// Helper method to read the conversation from <paramref name="inputFilePath"/>.
        /// </summary>
        /// <param name="inputFilePath">
        /// The input file path.
        /// </param>
        /// <returns>
        /// A <see cref="Conversation"/> model representing the conversation.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the input file could not be found.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown when something else went wrong.
        /// </exception>
        public Conversation ReadConversationFromTextFile(string inputFilePath)
        {

            //if inputFilePath is empty we throw an exception
            if (String.IsNullOrWhiteSpace(inputFilePath))
            {
                throw new ArgumentNullException("inputFilePath", String.Format("Exception in {0}, Error message : {1}",
                        this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, "inputFilePath can not be empty when inporting a conversation from a file"));
            }

            try
            {
                var reader = new StreamReader(new FileStream(inputFilePath, FileMode.Open, FileAccess.Read),
                    Encoding.ASCII);

                string conversationName = reader.ReadLine();

                //if conversationName is empty we throw an exception
                if (String.IsNullOrWhiteSpace(conversationName))
                {
                    throw new ArgumentException("conversationName", String.Format("Exception in {0}, Error message : {1}",
                           this.GetType().Name +"."+ System.Reflection.MethodBase.GetCurrentMethod().Name, "The input file : "+ inputFilePath+ @" is not in the corect format :<conversation_name><new_line>(<unix_timestamp><space><username><space><message><new_line>)*"));
                }
                var conversation = new Conversation(conversationName);


                string line;
                //validates this format <unix_timestamp><space><username><space><message>
                Regex rgx = new Regex(@"^([0-9]{10})\s(\w+)\s(.+)$");
                
                while ((line = reader.ReadLine()) != null)
                {
                    if (rgx.IsMatch(line))
                    {
                        var split = line.Split(' ');
                        string cont = split[2];
                        for (int i = 3; i < split.Length; i++) cont = cont + " " + split[i];
                        conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(split[0])), split[1], cont);
                    }
                    else {
                        throw new ArgumentException("inputFilePath", String.Format("Exception in {0}, Error message : {1}",
                            this.GetType().Name + "." +System.Reflection.MethodBase.GetCurrentMethod().Name, "The input file : " + inputFilePath + " is not in the corect format :<conversation_name><new_line>(<unix_timestamp><space><username><space><message><new_line>)*"));
                    }

                }
                reader.Dispose();
                return conversation;
            }
            catch (FileNotFoundException)
            {
                throw new ArgumentException("inputFilePath", String.Format("Exception in {0}, Error message : {1}",
                           this.GetType().Name + "." +System.Reflection.MethodBase.GetCurrentMethod().Name, "The input file : " + inputFilePath + " was not found."));
            }
            catch (IOException ex)
            {
                throw new IOException(String.Format("Exception in {0}, Error message : {1}",
                       this.GetType().Name + "." +System.Reflection.MethodBase.GetCurrentMethod().Name, "Something went wrong while trying  to access the file " + inputFilePath + " Exception message : " + ex.Message));
            }
        }

    }
}
