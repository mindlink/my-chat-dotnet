using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MindLink.Recruitment.MyChat
{
    class FilterName:Filter,INameFilter
    {
        public List<Message> NameFilter(List<Message> message, string NameFilter)
        {
            try
            {
                foreach (var line in message)
                {
                    if (line.senderId.ToLower().Equals(NameFilter.ToLower()))
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
