namespace MindLink.Recruitment.MyChat.Features.Additional
{
    using MindLink.Recruitment.MyChat.Interfaces.FeatureInterfaces;
    using MyChatModel.ModelData;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public sealed class FilterCardDetails : IStrategyFilter
    {
        /// <summary>
        /// List to store regex for card details
        /// </summary>
        private IList<Regex> cardDetailsRegex;

        /// <summary>
        /// Constructor for FilterCardDetails, sets up the regex to be used on the conversation
        /// </summary>
        public FilterCardDetails() 
        {
            // INITIALISE list of Regex's
            cardDetailsRegex = new List<Regex>();
            // ADD different regexes to find card details, source
            // https://www.regular-expressions.info/creditcard.html
            // visa cards
            cardDetailsRegex.Add(new Regex(@"\b4[0-9]{12}(?:[0-9]{3})?\b"));
            // master card
            cardDetailsRegex.Add(new Regex(@"\b(?:5[1-5][0-9]{2}|222[1-9]|22[3-9][0-9]|2[3-6][0-9]{2}|27[01][0-9]|2720)[0-9]{12}\b"));
            // regex for American Express
            cardDetailsRegex.Add(new Regex(@"\b[47][0-9]{13}\b"));
            // regex for JCB
            cardDetailsRegex.Add(new Regex(@"\b(?:2131|1800|35\d{3})\d{11}\b"));
        }

        public Conversation ApplyFilter(Conversation conversation)
        {
            foreach (Message msg in conversation.Messages) 
            {
                foreach (Regex rgx in cardDetailsRegex)
                {
                    if (rgx.IsMatch(msg.Content))
                        msg.Content = rgx.Replace(msg.Content, "\\*redacted\\*");
                }
            }

            return conversation;
        }
    }
}
