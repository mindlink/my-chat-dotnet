using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat
{
    interface IConversationFilterer
    {
        Conversation FilterConversation(Conversation conversation, ProgramOptions options);
    }
}
