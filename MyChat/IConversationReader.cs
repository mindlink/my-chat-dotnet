using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat
{
    public interface IConversationReader
    {
        Conversation ReadConversation(string inputFilePath);
    }
}
