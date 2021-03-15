using MindLink.Recruitment.MyChat.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MindLink.Recruitment.MyChat.Blacklisting
{
    public class KeywordBlacklist : IBlacklist
    {
        //a range of values to be checked for
        public string[] blacklistedValues { get; set; }

        //message used to replace blacklist items
        public string replacementMessage { get; set; }

        //constructors
        public KeywordBlacklist()
        { 
        }

        public KeywordBlacklist(string[] blacklistedValues, string replacementMessage)
        {
            this.blacklistedValues = blacklistedValues;
            this.replacementMessage = replacementMessage;
        }

        //checks inputs and replaces items that exist in the list
        public string[] CheckInput(string unixTime, string userId, string messageContent)
        {
            var checkedMessageContent = messageContent;
            
            foreach (var blackItem in blacklistedValues)
            {
                if (checkedMessageContent.Contains(blackItem))
                    checkedMessageContent = TextFunctions.ReplaceWords(checkedMessageContent, blackItem, replacementMessage);
            }

            return new string[] { unixTime, userId, checkedMessageContent};
        }
    }
}
