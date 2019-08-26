namespace MindLink.Recruitment.MyChat
{
    using System;

    /// <summary>
    /// Represents a chat message.
    /// </summary>
    public sealed class Message
    {
        public string content;
        public string senderId;
        public DateTimeOffset timestamp;

        public Message(DateTimeOffset timestamp, string senderId, string content)
        {
            this.content = content;
            this.timestamp = timestamp;
            this.senderId = senderId;
        }
    }
}
