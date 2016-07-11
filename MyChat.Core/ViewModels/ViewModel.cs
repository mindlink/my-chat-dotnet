using MyChat.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat.Core.ViewModels
{
    public class ViewModel
    {

        public virtual void Log(Exception e)
        {
            Logger.Log(e);
        }

        public virtual void Log(String e)
        {
            Logger.Log(e);
        }

        public virtual Conversation ExportConversation(String[] args)
        {
            return null;
        }
    }
}
