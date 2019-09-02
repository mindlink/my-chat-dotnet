namespace MindLink.Recruitment.MyChat
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Linq;
    using MindLink.Recruitment.MyChat;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a conversation writer that can write a conversation to JSON.
    /// </summary>
    public static class ConversationWriter
    {
        public static void ConversationToJson(Conversation conversation, string outputFilePath)
        {
            var writer = new StreamWriter(new FileStream(@"..\..\" + outputFilePath, FileMode.Create, FileAccess.ReadWrite));

            var serialized = JsonConvert.SerializeObject(conversation, Formatting.Indented);

            writer.Write(serialized);

            writer.Flush();

            writer.Close();
        }
    }
}
