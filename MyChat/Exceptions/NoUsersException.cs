using System;

namespace MindLink.Recruitment.MyChat.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the 
    /// </summary>
    public class NoUsersException : Exception
    {
        public NoUsersException(string message) : base(message)
        {

        }
    }
}
