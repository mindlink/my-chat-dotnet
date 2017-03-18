using System.Collections.Generic;
using Newtonsoft.Json.Serialization;

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
            this.content = content;
            this.timestamp = timestamp;
            this.senderId = senderId;
        }
    }
}
