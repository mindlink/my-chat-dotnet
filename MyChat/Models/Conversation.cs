using System.Collections.Generic;

namespace MindLink.Recruitment.MyChat
{

    public sealed class Conversation : IConversation
    {
        public string Name { get; private set; }
        public List<Message> Messages { get; private set; }

        public Conversation(string name, List<Message> messages)
        {
            Name = name;
            Messages = messages;
        }
    }
}
