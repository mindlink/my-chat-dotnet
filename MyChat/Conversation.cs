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
        public readonly string name;

        /// <summary>
        /// The messages in the conversation.
        /// </summary>
        public readonly IEnumerable<Message> messages;

        public readonly IEnumerable<Activity> activity;

        /// <summary>
        /// Initializes a new instance of the <see cref="Conversation"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the conversation.
        /// </param>
        /// <param name="messages">
        /// The messages in the conversation.
        /// </param>
        public Conversation(string name, IEnumerable<Message> messages, IEnumerable<Activity> activity)
        {
            this.name = name;
            this.messages = messages;
            this.activity = activity;
        }
    }
}
