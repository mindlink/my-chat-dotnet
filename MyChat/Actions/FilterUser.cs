using MindLink.Recruitment.MyChat.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat.Actions
{
    public class FilterUser : ConversationAction
    {
        string userId;

        /// <summary>
        /// Constrator
        /// </summary>
        /// <param name="userId"></param>
        public FilterUser(string userId) {
            //if userid is empty we throw an exception
            if (String.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException("userId", String.Format("Exception in {0}, Error message : {1}",
                        this.GetType().Name + "." +System.Reflection.MethodBase.GetCurrentMethod().Name, "User id can not be empty when filering by user"));
            }
            this.userId = userId;
            this.actionID = "/u";
            this.actionPriority = Priority.Normal;
        }

        /// <summary>
        /// Filter message using the curent userid
        /// </summary>
        /// <param name="conversation"></param>
        /// <returns></returns>
        public override Conversation PerformOn(Conversation conversation) {

            //if conversation is null we throw an exception
            if (conversation == null)
            {
                throw new ArgumentNullException("conversation", String.Format("Exception in {0}, Error message : {1}",
                       this.GetType().Name+"."+System.Reflection.MethodBase.GetCurrentMethod().Name, "PerfromOn received a null argument!"));
            }

            conversation.Messages = conversation.Messages.Where(item => item.msgSender.senderID == userId).ToList<Message>();

            return conversation;
        }
    }
}
