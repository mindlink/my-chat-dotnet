using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MindLink.Recruitment.MyChat
{
    class FilterRedacted:Filter,IRedactedWordFilter
    {
        private string redactedWord = "*Redacted*";

        public List<Message> RedactedWordFilter(List<Message> message, string TargetWord)
        {
            try
            {
                foreach (var line in message)
                {
                    if (line.content.Contains(TargetWord))
                    {
                        FilteredMessages.Add(new Message(line.timestamp, line.senderId, line.content.Replace(TargetWord, redactedWord)));
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
