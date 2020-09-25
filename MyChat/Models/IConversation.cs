using System.Collections.Generic;

namespace MindLink.Recruitment.MyChat
{
    public interface IConversation
    {
        string Name { get; }
        List<Message> Messages { get; }
    }
}