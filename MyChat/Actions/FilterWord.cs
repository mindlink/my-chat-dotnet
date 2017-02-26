using MindLink.Recruitment.MyChat.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat.Actions
{
   public class FilterWord : ConversationAction
    {
        protected string word;
        protected int priority = 5;

        /// <summary>
        /// Constrator
        /// </summary>
        /// <param name="word"></param>
        public FilterWord(string word)
        {
            //if word is empty we throw an exception
            if (String.IsNullOrWhiteSpace(word))
            {
                throw new ArgumentNullException("word", String.Format("Exception in {0}, Error message : {1}",
                        this.GetType().Name + "." +System.Reflection.MethodBase.GetCurrentMethod().Name, "Word can not be empty when filering using a word"));
            }
            this.word = word;
            this.actionID = "/w";
            this.actionPriority = Priority.Normal;
        }

        /// <summary>
        /// Filter message using the curent word
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

            conversation.Messages = conversation.Messages.Where(item => item.Content.Contains(word)).ToList<Message>();

            return conversation;
        }
    }
}
