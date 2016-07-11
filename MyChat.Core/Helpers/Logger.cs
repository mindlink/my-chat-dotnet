using MyChat.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyChat.Core.Helpers
{
    public static class Logger
    {

        public static void Log(String log)
        {
            DataManager.Instance.AppendTextToFile("Logged at " + DateTime.Now.ToString("hh:mm:ss ") + " : " + log);

            Console.WriteLine( log);

            #if DEBUG
               // Console.ReadKey();
            #endif
        }

        public static void Log(Exception log)
        {
            Log(log.Message + ": \t\t" + log.StackTrace);
        }
    }
}
