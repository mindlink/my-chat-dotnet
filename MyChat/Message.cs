namespace MindLink.Recruitment.MyChat
{
    using System;

    public sealed class Message
    {
        public string content;

        public DateTimeOffset timestamp;

        public string senderId;

        public Message(DateTimeOffset timestamp, string senderId, string content)
        {
            this.content = content;
            this.timestamp = timestamp;
            this.senderId = senderId;
        }
    }
}
