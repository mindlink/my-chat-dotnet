using System;

namespace MindLink.Recruitment.MyChat
{
    public interface IMessage
    {
        public string content { get;}

        public DateTimeOffset timestamp { get; }

        public string senderId { get; }
    }
}