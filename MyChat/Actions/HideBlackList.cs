using MindLink.Recruitment.MyChat.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat.Actions
{
   public class HideBlackList : ConversationAction
    {
        List<String> blackList = new List<String>();

        /// <summary>
        /// Constrator
        /// </summary>
        public HideBlackList()
        {
            this.actionID = "/b";
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
        /// Replace the blacklisted words in the message with *redacted*
        /// </summary>
        /// <param name="msg"></param>
        private void removeWords(Message msg) {
            foreach (String curenWord in blackList) {
                msg.Content = Regex.Replace(msg.Content, curenWord, "*redacted*", RegexOptions.IgnoreCase);
            }
        }
        /// <summary>
        /// Add a word to the list of the blacklisted words
        /// </summary>
        /// <param name="word"></param>
        public void addBlackListWord(String word) {

            //if word is null we throw an exception
            if (String.IsNullOrWhiteSpace(word))
            {
                throw new ArgumentNullException("word", String.Format("Exception in {0}, Error message : {1}",
                       this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, "addBlackListWord received a null argument!"));
            }
            blackList.Add(word);
        }
    }
}
