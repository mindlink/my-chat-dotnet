using System;


namespace MindLink.Recruitment.MyChat
{
    public class MessageListErrorException : Exception
    {
        public MessageListErrorException(string message)
        : base(message)
        {
        }

        public MessageListErrorException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}