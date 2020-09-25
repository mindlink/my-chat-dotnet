using System;
using System.Collections.Generic;
using System.Text;

namespace MindLink.Recruitment.MyChat
{
    interface IKeywordFilter
    {
        List<Message> KeywordFilter(List<Message> message, string KeyWordFilter);
    }
}
