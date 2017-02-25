using MindLink.Recruitment.MyChat.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat.Actions
{
    public abstract class ConversationAction
    {
        protected string actionID;
        protected int priority = 0;

        public abstract Conversation PerformOn(Conversation conversation);

        /// <summary>
        /// Return the actionID
        /// </summary>
        public string ActionID
        {
            get
            {
                return actionID;
            }
        }
    }
}
