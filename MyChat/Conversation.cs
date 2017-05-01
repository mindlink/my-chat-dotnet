namespace MindLink.Recruitment.MyChat
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents the model of a conversation.
    /// </summary>
    public sealed class Conversation
    {
        /// <summary>
        /// The name of the conversation.
        /// </summary>
        public string name { get; }

        /// <summary>
        /// The messages in the conversation.
        /// </summary>
        private List<Message> _messages;
        public List<Message> messages
        {
            get
            {
                return _messages;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Conversation"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the conversation.
        /// </param>
        /// <param name="messages">
        /// The messages in the conversation.
        /// </param>
        public Conversation(string name, List<Message> messages)
        {
            this.name = name;
            _messages = messages;
        }

        /// <summary>
        /// Discards any messages not sent by <paramref name="user"/>.
        /// </summary>
        /// <param name="user">
        /// The user to filter messages by.
        /// </param>
        public void FilterByUser(string user)
        {
            if (user == "")
            {
                return;
            }
            var filteredMessages = new List<Message>();
            foreach (Message m in messages)
            {
                if (m.senderId == user)
                {
                    filteredMessages.Add(m);
                }
            }
            _messages = filteredMessages;
        }
    }
}
