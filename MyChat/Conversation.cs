namespace MindLink.Recruitment.MyChat
{
    using System.Collections.Generic;
    
    public sealed class Conversation
    {
        public string Name { get;}
        public IEnumerable<Message> Messages { get; }

        public Conversation(string name, IEnumerable<Message> messages)
        {
            Name = name;
            Messages = messages;
        }
    }
}
