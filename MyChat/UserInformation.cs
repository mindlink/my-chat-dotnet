using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat
{
    public class UserInformation
    {
        public string userID;
        public int messageCount;

        public UserInformation(string userID)
        {
            this.userID = userID;
            messageCount = 0;
        }


    }
}
