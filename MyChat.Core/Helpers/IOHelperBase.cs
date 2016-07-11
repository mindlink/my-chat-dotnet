using MindLink.Recruitment.MyChat.Core;
using MyChat.Core.Abstract;
using MyChat.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyChat.Core.Helpers
{
    public class IOHelperBase
    {

        public string LogsFolder { get; set; }
        public string CurrentLogFile { get; set; }

        public virtual void Log(Exception e)
        {
            Logger.Log(e);
        }

        public virtual void Log(String e)
        {
            Logger.Log(e);
        }
 
    }
}
