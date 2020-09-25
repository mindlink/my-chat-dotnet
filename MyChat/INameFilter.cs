using System;
using System.Collections.Generic;
using System.Text;

namespace MindLink.Recruitment.MyChat
{
    interface INameFilter
    {
        List<Message> NameFilter(List<Message> message, string NameFilter);
    }
}