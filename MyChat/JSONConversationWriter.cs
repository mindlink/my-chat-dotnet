using System.IO;
using Newtonsoft.Json;

namespace MindLink.Recruitment.MyChat
{
    public class JSONConversationWriter : IConversationWriter
    {
        public void WriteConversation(TextWriter writer, Conversation conversation)
        {
            var serialized = JsonConvert.SerializeObject(conversation, Formatting.Indented);

            writer.Write(serialized);
            writer.Flush();
            writer.Close();
        }
    }

    public interface IConversationWriter
    {
        public void WriteConversation(TextWriter writer, Conversation conversation);
    }
}