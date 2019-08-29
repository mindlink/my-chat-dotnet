namespace MindLink.Recruitment.MyChat
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents the model of a conversation.
    /// </summary>
    public sealed class Conversation
    {
        public string name;
        public IEnumerable<Message> messages;

        public Conversation(string name, IEnumerable<Message> messages)
        {
            this.name = name;
            this.messages = messages;
        }
    }
}
