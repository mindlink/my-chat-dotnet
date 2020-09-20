namespace MindLink.Recruitment.MyChat
{
    using System;

    public sealed class Message
    {
        public string content { get;}

        public DateTimeOffset timestamp { get; }

        public string senderId { get; }

        public Message(DateTimeOffset Timestamp, string SenderId, string Content)
        {
            content = Content;
            timestamp = Timestamp;
            senderId = SenderId;

            
        }
    }
}
