using System;

namespace MindLink.Recruitment.MyChat
{
    public sealed class Message
    {
        public string Content { get; set; }
        public DateTimeOffset Timestamp { get; private set; }
        public string SenderId { get; private set; }

        public Message(DateTimeOffset timestamp, string senderId, string content)
        {
            Content = content;
            Timestamp = timestamp;
            SenderId = senderId;
        }
    }
}
