using MindLink.Recruitment.MyChat.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat.Actions
{
    class HideSenderID : ConversationAction
    {
        /// <summary>
        /// Constractor
        /// </summary>
        public HideSenderID()
        {
            this.actionID = "/hs";
            this.actionPriority = Priority.Low;
        }

        /// <summary>
        ///  Activating the flag to hide senderid
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

            conversation.HideSenders = true;

            return conversation;
        }
    }
}
