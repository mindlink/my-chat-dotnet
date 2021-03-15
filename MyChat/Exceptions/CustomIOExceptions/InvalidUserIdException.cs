using System;
using System.Collections.Generic;
using System.Text;

namespace MindLink.Recruitment.MyChat.Exceptions.CustomIOExceptions
{
    [Serializable]
    //used when there is no/incorrect UserId information
    class InvalidUserIdException: Exception
    {
        public InvalidUserIdException()
        {

        }

        public InvalidUserIdException(string message) : base(message)
        {

        }

        public InvalidUserIdException(string message, Exception inner) : base(message, inner)
        {

        }

    }
}
