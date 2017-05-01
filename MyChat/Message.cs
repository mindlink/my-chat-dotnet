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
        private string _content;
        public string content
        {
            get
            {
                return _content;
            }
        }

        /// <summary>
        /// The message timestamp.
        /// </summary>
        public DateTimeOffset timestamp { get; }

        /// <summary>
        /// The message sender.
        /// </summary>
        public string senderId { get; }

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
            _content = content;
            this.timestamp = timestamp;
            this.senderId = senderId;
        }

        /// <summary>
        /// Removes <paramref name="count"/> characters from the message content starting at <paramref name="index"/>.
        /// Then inserts <paramref name="replaceWith"/> in place of the removed characters.
        /// </summary>
        /// <param name="index">
        /// Index where word is replaced.
        /// </param>
        /// <param name="count">
        /// Number of charactrers to remove.
        /// </param>
        /// <param name="replaceWith">
        /// String to be inserted at <paramref name="index"/>.
        /// </param>
        public void ReplaceWord(int index, int count, string replaceWith)
        {
            _content = _content.Remove(index, count);
            _content = _content.Insert(index, replaceWith);
        }
    }
}
