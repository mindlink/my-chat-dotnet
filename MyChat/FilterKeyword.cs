using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MindLink.Recruitment.MyChat
{
    class FilterKeyword:Filter,IKeywordFilter
    {
        public List<Message> KeywordFilter(List<Message> message, string Keyword)
        {
            try
            {
                foreach (var line in message)
                {
                    if (line.content.ToLower().Contains(Keyword.ToLower()))
                    {
                       FilteredMessages.Add(new Message(line.timestamp, line.senderId, line.content));
                    }
                }
                return FilteredMessages;
            }
            catch (FormatException)
            {
                throw new Exception("Input string is in incorrect format");
            }
        }

    }
}
