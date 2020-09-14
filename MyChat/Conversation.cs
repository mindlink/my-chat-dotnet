namespace MindLink.Recruitment.MyChat
{
    using System.Collections.Generic;
    
    public sealed class Conversation
    {
        private string Name { get; set; }
        private IEnumerable<Message> Messages;

        public Conversation(string name, IEnumerable<Message> messages)
        {
            Name = name;
            Messages = messages;
        }
    }
}
