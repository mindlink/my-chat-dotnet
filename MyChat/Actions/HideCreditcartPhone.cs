using MindLink.Recruitment.MyChat.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat.Actions
{
   public class HideCreditcartPhone : ConversationAction
    {
        /// <summary>
        /// Constrator
        /// </summary>
        public HideCreditcartPhone()
        {
            this.actionID = "/ct";
            this.priority = 0;
        }
        /// <summary>
        ///  Replace the blacklisted words in the conversation with redacted
        /// </summary>
        /// <param name="conversation"></param>
        /// <returns></returns>
        public override Conversation PerformOn(Conversation conversation)
        {
            //if conversation is null we throw an exception
            if (conversation == null)
            {
                throw new ArgumentNullException("conversation", String.Format("Exception in {0}, Error message : {1}",
                       this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, "PerfromOn received a null argument!"));
            }

            // we use parallelism to speed up the search process
            Parallel.ForEach(conversation.Messages, item => removeWords(item));
            return conversation;
        }
        /// <summary>
        /// Replace the Credit cart and Phone number in the message with *redacted*
        /// </summary>
        /// <param name="msg"></param>
        private void removeWords(Message msg)
        {
            Regex rgx;
            // hide phone number
            rgx = new Regex(@"(^|\s)([0|\+[0-9]{1,5})?(\s?[0-9]{6,10})(\s|$)");
            msg.Content = rgx.Replace(msg.Content, " *redacted* ").Trim();
            // hide credit cart
            rgx = new Regex(@"\b(?:3[47]\d{2}([\ \-]?)\d{6}\1\d|(?:(?:4\d|5[1-5]|65)\d{2}|6011)([\ \-]?)\d{4}\2\d{4}\2)\d{4}\b");
            msg.Content = rgx.Replace(msg.Content, "*redacted*").Trim();

        }
    }
}
