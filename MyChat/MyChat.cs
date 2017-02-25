using MindLink.Recruitment.MyChat.Sates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat
{
    public sealed class MyChat
    {
        /// <summary>
        /// The application entry point.
        /// </summary>
        /// <param name="args">
        /// The command line arguments.
        /// </param>
        static void Main(string[] args)
        { 
            State state = null;
            ConversationsManager cm = ConversationsManager.GetInstance;

            if (args.Length == 0)
            {
                state = new DispalyHelp();
            }
            else {
                state = (new Initializing(args, cm)).Run();
            }

            while (state != null) state =state.Run();


        }
    }
}
