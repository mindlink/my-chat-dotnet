using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using MindLink.Recruitment.MyChat;
using Newtonsoft.Json;

namespace MindLink.Recruitment.MyChat
{
    public static class ConversationWriter
    {
        public static void ConversationToJson(Conversation conversation, string outputFilePath)
        {
            var writer = new StreamWriter(new FileStream(outputFilePath, FileMode.Create, FileAccess.ReadWrite));

            var serialized = JsonConvert.SerializeObject(conversation, Formatting.Indented);

            writer.Write(serialized);

            writer.Flush();

            writer.Close();
        }
    }
}
