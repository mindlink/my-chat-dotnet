using System;
using System.Collections.Generic;
using System.Text;

namespace MindLink.Recruitment.MyChat
{
    interface IFilterMessages
    {
         Conversation FilterMessages(Conversation conversation);
    }
}
