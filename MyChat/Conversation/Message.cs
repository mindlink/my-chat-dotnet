namespace MindLink.Recruitment.MyChat
{
    using System;

    /// <summary>
    /// Stores messsage data.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// The message content.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// The message timestamp.
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }

        /// <summary>
        /// The message sender.
        /// </summary>
        public string SenderId { get; set; }
    }
}
