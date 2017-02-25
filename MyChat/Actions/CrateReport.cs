using MindLink.Recruitment.MyChat.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MindLink.Recruitment.MyChat.Elements.Conversation;

namespace MindLink.Recruitment.MyChat.Actions
{
    class CrateReport : ConversationAction
    {
        /// <summary>
        /// Constractor
        /// </summary>
        public CrateReport() {
            this.actionID = "/r";
            this.priority = 10;
        }

        /// <summary>
        /// Creates the senders messages report
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

            // convert Senders Info dictionary to a list of SenderInfo
            List<SenderInfo> sInfo = conversation.SendersInfos.Select(x => x.Value).ToList();
            //sort the list
            conversation.SendersInfoSorted = sInfo.OrderByDescending(o => o.count).ToList();
            // Find the most active user
            conversation.MostActiveSender = conversation.SendersInfoSorted.First().sender;
            return conversation;
        }
    }
}
