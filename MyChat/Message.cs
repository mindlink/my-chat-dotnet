using System;

namespace MindLink.Recruitment.MyChat
{
    /// <summary>
    /// Represents a chat message.
    /// </summary>
    public sealed class Message
    {
        /// <summary>
        /// The message content.
        /// </summary>
        string content;

        /// <summary>
        /// The message timestamp.
        /// </summary>
        DateTimeOffset timestamp;

        /// <summary>
        /// The message sender.
        /// </summary>
        string senderId;

        /// <summary>
        /// Sets or returns the Content of the message
        /// </summary>
        public string Content 
        {
            get { return this.content; }
            set { this.content = value; }
        }          

        /// <summary>
        /// Sets or returns the time stamp of a message
        /// </summary>
        public DateTimeOffset Timestamp 
        {
            get { return this.timestamp; }
            set { this.timestamp = value; }
        }

        /// <summary>
        /// Sets or returns the id of a Sender 
        /// </summary>
        public string SenderId 
        {
            get { return this.senderId; }
            set { this.senderId = value; }
        }        

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
            Content = content;
            Timestamp = timestamp;
            SenderId = senderId;
        }
    }
}
