namespace MindLink.Recruitment.MyChat
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// Represents the model of a conversation.
    /// </summary>
    public sealed class Conversation
    {
        /// <summary>
        /// The name of the conversation.
        /// </summary>
        public string name;

        /// <summary>
        /// The messages in the conversation.
        /// </summary>
        public IEnumerable<Message> messages;

        /// <summary>
        /// Additional info of conversation
        /// </summary>
        public IEnumerable<Report> activity;

        /// <summary>
        /// Initializes a new instance of the <see cref="Conversation"/> class. Overloaded to include an additional list of <see cref="Report">
        /// </summary>
        /// <param name="name">
        /// The name of the conversation.
        /// </param>
        /// <param name="messages">
        /// The messages in the conversation.
        /// </param>
        /// <param name="activity">
        /// Activity report of the conversation
        /// </param>
        public Conversation(string name, IEnumerable<Message> messages, IEnumerable<Report> activity)
        {
            this.name = name;
            this.messages = messages;
            this.activity = activity;
        }
    }
}
