using MindLink.Recruitment.MyChat.Elements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat.FilesProcessing
{
    class FileImporter
    {


        /// <summary>
        /// Helper method to read the conversation from <paramref name="inputFilePath"/>.
        /// </summary>
        /// <param name="inputFilePath">
        /// The input file path.
        /// </param>
        /// <returns>
        /// A <see cref="oldConversation"/> model representing the conversation.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the input file could not be found.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown when something else went wrong.
        /// </exception>
        public Conversation ReadConversationFromTextFile(string inputFilePath)
        {
            try
            {
                var reader = new StreamReader(new FileStream(inputFilePath, FileMode.Open, FileAccess.Read),
                    Encoding.ASCII);

                string conversationName = reader.ReadLine();
                var conversation = new Conversation(conversationName);

                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    
                    var split = line.Split(' ');
                    if (split.Length >= 3)
                    {
                        string cont = split[2];
                        for (int i = 3; i < split.Length; i++) cont = cont + " " + split[i];
                        conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(split[0])), split[1], cont);
                    }
                    else throw new ArgumentException("dfsfs");

                }
                reader.Dispose();
                return conversation;
            }
            catch (FileNotFoundException)
            {
                throw new ArgumentException(String.Format("The text has {0} '{' characters and {1}  {2} '}' characters.",
                       this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "The file was not found."));
            }
            catch (IOException)
            {
                throw new Exception("Something went wrong in the IO.");
            }
        }

    }
}
