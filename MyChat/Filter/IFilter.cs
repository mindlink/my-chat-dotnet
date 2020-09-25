using System;

namespace MindLink.Recruitment.MyChat
{
    public interface IFilter
    {
        string Word { get; set; }

        Conversation Filter(Conversation conversation);
        
    }
}
