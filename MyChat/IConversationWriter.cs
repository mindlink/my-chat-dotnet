using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat
{
    public interface IConversationWriter
    {
        void WriteConversation(Conversation conversation, string outputFilePath);
    }
}
