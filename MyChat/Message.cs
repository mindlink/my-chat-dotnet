namespace MindLink.Recruitment.MyChat
{
    using System;

    /// <summary>
    /// Represents a chat message.
    /// </summary>
    public sealed class Message
    {
        public string content;
        public string userId;
        public DateTimeOffset timestamp;

        public Message(DateTimeOffset timestamp, string userId, string content)
        {
            this.content = content;
            this.timestamp = timestamp;
            this.userId = userId;
        }
    }
}
