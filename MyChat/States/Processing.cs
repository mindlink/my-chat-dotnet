using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat.States
{
    class Processing : State
    {
        ConversationsManager cm;

        public Processing(ConversationsManager CM)
        {
            this.cm = CM;

        }
        public override State Run()
        {
            cm.loadConversation();
            cm.PerformActions();
            cm.exportConversation();
            return null;
        }
    }
}
