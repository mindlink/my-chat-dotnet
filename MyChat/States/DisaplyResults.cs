using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat.States
{
    
    class DisaplyResults : State
    {
        private String resultMsg = "";

        public override State Run()
        {
            Console.WriteLine(resultMsg);
            return null;
        }

        public void setResultMsg(string msg)
        {
            resultMsg = msg;

        }
    }
}
