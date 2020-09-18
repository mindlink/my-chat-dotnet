namespace MyChatModel.ModelData
{
    using System;
    using System.Collections.Generic;
    using System.Text;

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
