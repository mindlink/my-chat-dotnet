namespace MindLink.Recruitment.MyChat
{
    using System;

    /// <summary>
    /// Represents a chat message.
    /// </summary>
    public sealed class Message
    {
        /// <summary>
        /// The message content.
        /// </summary>
        private string content;

        /// <summary>
        /// The message timestamp.
        /// </summary>
        private DateTimeOffset timestamp;

        /// <summary>
        /// The message sender.
        /// </summary>
        private string senderId;

        public string Content { get => content; set => content = value; }
        public DateTimeOffset Timestamp { get => timestamp; set => timestamp = value; }
        public string SenderId { get => senderId; set => senderId = value; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="timestamp">
        /// The message timestamp.
        /// </param>
        /// <param name="senderId">
        /// The ID of the sender.
        /// </param>
        /// <param name="content">
        /// The message content.
        /// </param>
        public Message(DateTimeOffset timestamp, string senderId, string content)
        {
            this.Content = content;
            this.Timestamp = timestamp;
            this.SenderId = senderId;
        }
    }
}
