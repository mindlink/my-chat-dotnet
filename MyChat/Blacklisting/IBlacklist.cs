using System;
using System.Collections.Generic;
using System.Text;

namespace MindLink.Recruitment.MyChat.Blacklisting
{
    //used to allow other blacklist functions to be used without changing the other classes
    public interface IBlacklist
    {
        //checks the inputs against blacklist information
        public string[] CheckInput(string unixTime, string userId, string messageContent);
    }
}
