using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace MindLink.Recruitment.MyChat
{
    public class JsonConversationWriter : IConversationWriter
    {
        public void WriteConversation(Conversation conversation, string outputFilePath)
        {
            try
            {
                using (var writer = new StreamWriter(new FileStream(outputFilePath, FileMode.Create, FileAccess.Write)))
                {
                    writer.WriteLine(JsonConvert.SerializeObject(conversation, Formatting.Indented));
                }
            }
            catch (Exception ex) when (ex is IOException || ex is FileNotFoundException || 
                                       ex is DirectoryNotFoundException || ex is ArgumentException || 
                                       ex is System.Security.SecurityException)
            {
                throw;
            }
        }
    }
}
