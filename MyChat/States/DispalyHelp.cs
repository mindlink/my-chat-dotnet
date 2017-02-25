using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat.Sates
{
    class DispalyHelp : State
    {
        private string errorMsg;

        public override State Run()
        {
            System.Console.WriteLine("Expected a directory to be specified with the dir parameter.");
            return null;
        }

        public void setErrorMsg(string msg) {
            errorMsg = msg;

        }
    }

}
