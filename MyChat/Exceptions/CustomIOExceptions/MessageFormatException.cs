using System;
using System.Collections.Generic;
using System.Text;

namespace MindLink.Recruitment.MyChat.Exceptions.CustomIOExceptions
{
    [Serializable]
    //exception used when the message content doesn't exist or is incorrectly formatted
    class MessageFormatException : Exception
    {

        public MessageFormatException()
        {

        }

        public MessageFormatException(string message) : base(message)
        {

        }

        public MessageFormatException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
