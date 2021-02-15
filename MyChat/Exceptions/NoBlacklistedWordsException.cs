using System;

namespace MindLink.Recruitment.MyChat.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the 
    /// </summary>
    public class NoBlacklistedWordsException : Exception
    {
        public NoBlacklistedWordsException(string message) : base(message)
        {

        }
    }
}
