using System;

namespace MindLink.Recruitment.MyChat.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the 
    /// </summary>
    public class NoMessagesException : Exception
    {
        public NoMessagesException(string message) : base(message)
        {

        }
    }
}
