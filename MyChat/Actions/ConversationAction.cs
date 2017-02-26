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
        // actions priority which determines which action is executed first, we execute first the report action them the filtering actions and lastly the hide actions
        public enum Priority
        {
            Higth = 10,
            Normal = 5,
            Low = 0,
        }

        protected string actionID;
        protected Priority actionPriority = Priority.Low;

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

        /// <summary>
        /// Return the priority
        /// </summary>
        public Priority ActionPriority
        {
            get
            {
                return actionPriority;
            }
        }
    }
}
