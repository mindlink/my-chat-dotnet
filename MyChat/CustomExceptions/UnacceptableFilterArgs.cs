using System;
using System.Collections.Generic;
using System.Text;

namespace MindLink.Recruitment.MyChat.CustomExceptions
{
    /// <summary>
    /// Custom exception, if the user tries to supply/forgets to supply an argument
    /// and instead supplies a filter as an argument
    /// </summary>
    [Serializable]
    public class UnacceptableFilterArgs : Exception
    {

        public UnacceptableFilterArgs() : base() 
        {

        }

        public UnacceptableFilterArgs(string message) : base(message) 
        {

        }

        public UnacceptableFilterArgs(string message, Exception innerException) : base(message, innerException) 
        {

        }
    }
}
