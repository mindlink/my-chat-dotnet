using System;
using System.IO;
using System.Security;
using Newtonsoft.Json;

namespace MindLink.Recruitment.MyChat
{
    public class ConversationWriter
    {
        public bool Successful { get; private set; } = false;

        public ConversationWriter(Conversation conversation, string path)
        {
            WriteConversation(conversation, path);
        }

        public void WriteConversation(Conversation conversation, string outputFilePath)
        {
            try
            {
                using (var writer = new StreamWriter(new FileStream(outputFilePath, FileMode.Create, FileAccess.ReadWrite)))
                {
                    var serialized = JsonConvert.SerializeObject(conversation, Formatting.Indented);
                    writer.Write(serialized);

                }
                Successful = true;

            }
            catch (SecurityException ex)
            {
                Console.WriteLine(ex.Message, ex.StackTrace);
                throw ex;
                
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine(ex.Message, ex.StackTrace);
                throw ex;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex.StackTrace);
                throw ex;
            }

        }
    }
}
