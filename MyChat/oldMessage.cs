namespace MindLink.Recruitment.MyChat
{
    using System;

    /// <summary>
    /// Represents a chat message.
    /// </summary>
    public sealed class OldMessage
    {
        /// <summary>
        /// The message content.
        /// </summary>
        public string content;

        /// <summary>
        /// The message timestamp.
        /// </summary>
        public DateTimeOffset timestamp;

        /// <summary>
        /// The message sender.
        /// </summary>
        public string senderId;

        /// <summary>
        /// Initializes a new instance of the <see cref="OldMessage"/> class.
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
        public OldMessage(DateTimeOffset timestamp, string senderId, string content)
        {
            this.content = content;
            this.timestamp = timestamp;
            this.senderId = senderId;
        }
    }
}
