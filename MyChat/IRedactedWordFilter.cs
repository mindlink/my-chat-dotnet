using System;
using System.Collections.Generic;
using System.Text;

namespace MindLink.Recruitment.MyChat
{
    interface IRedactedWordFilter
    {
        List<Message> RedactedWordFilter(List<Message> message, string TargetWord);
    }
}


