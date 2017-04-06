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
        private string name;

        /// <summary>
        /// List of user activity.
        /// </summary>
        private IEnumerable<UserActivity> userActivity;

        /// <summary>
        /// The messages in the conversation.
        /// </summary>
        private IEnumerable<Message> messages;

        public string Name { get => name; set => name = value; }
        public IEnumerable<UserActivity> UserActivity { get => userActivity; set => userActivity = value; }
        public IEnumerable<Message> Messages { get => messages; set => messages = value; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Conversation"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the conversation.
        /// </param>
        /// <param name="messages">
        /// The messages in the conversation.
        /// </param>
        public Conversation(string name, IEnumerable<Message> messages)
        {
            this.Name = name;
            this.Messages = messages;
        }
    }
}
