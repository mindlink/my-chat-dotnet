using System;
namespace MindLink.Recruitment.MyChat
{
    public interface INumberCover
    {
        Conversation Conversation { get; set; }
        string[] RegexString { get; }
        void Hide();
    }
}
