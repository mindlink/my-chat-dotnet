namespace MindLink.Recruitment.MyChat.Features.Additional
{
    using MindLink.Recruitment.MyChat.Interfaces.FeatureInterfaces;
    using MyChatModel.ModelData;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Class to filter phone numbers out of conversations
    /// </summary>
    public sealed class FilterPhoneNumbers : IStrategyFilter
    {
        //
        private IList<Regex> phoneNumRegex; 

        /// <summary>
        /// FilterPhoneNummbers
        /// </summary>
        public FilterPhoneNumbers() 
        {
            // INITIALISE list for phone number regex's
            phoneNumRegex = new List<Regex>();
            // 
            phoneNumRegex.Add(new Regex(@"\b[0-9]{11}$\b"));

        }

        public Conversation ApplyFilter(Conversation conversation)
        {
            foreach (Message msg in conversation.Messages) 
            {
                foreach (Regex rgx in phoneNumRegex)
                    msg.Content = rgx.Replace(msg.Content, "\\*redacted\\*");
            }


            return conversation;
        }
    }
}
