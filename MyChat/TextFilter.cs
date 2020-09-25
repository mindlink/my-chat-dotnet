using System;
using System.Collections.Generic;
using System.Text;

namespace MindLink.Recruitment.MyChat
{
    // Class that handles filtering messages depending on user input
    public class TextFilter : INameFilter, IKeywordFilter, IRedactedWordFilter
    {
        private string redactedWord = "*Redacted*";

        // method that filters messages depending on what name is entered by the user
        public List<Message> NameFilter(List<Message> message, string NameFilter)
        {
            try
            {
                var FilteredMessages = new List<Message>();

                foreach (var line in message)
                {
                    if (line.senderId.ToLower().Equals(NameFilter.ToLower()))
                    {
                        FilteredMessages.Add(new Message(line.timestamp, line.senderId, line.content));
                    }
                }
                return FilteredMessages;
            }
            catch (MessageListErrorException)
            {
                throw new Exception("Something went wrong with the input message list");
            }
        }

        // method that returns messages depending on the keyword the user enters
        public List<Message> KeywordFilter(List<Message> message, string Keyword)
        {
            try
            {
                var FilteredMessages = new List<Message>();

                foreach (var line in message)
                {
                    if (line.content.ToLower().Contains(Keyword.ToLower()))
                    {
                        FilteredMessages.Add(new Message(line.timestamp, line.senderId, line.content));
                    }
                }

                return FilteredMessages;
            }
            catch (MessageListErrorException)
            {
                throw new Exception("Something went wrong with the input message list");
            }
        }

        // method that redacts any word that the user enters from the messages
        public List<Message> RedactedWordFilter(List<Message> message, string TargetWord)
        {
            try
            {
                var FilteredMessages = new List<Message>();

                foreach (var line in message)
                {
                    if (line.content.Contains(TargetWord))
                    {
                        FilteredMessages.Add(new Message(line.timestamp, line.senderId, line.content.Replace(TargetWord, redactedWord)));
                    }
                }
                return FilteredMessages;
            }
            catch (MessageListErrorException)
            {
                throw new Exception("Something went wrong with the input message list");
            }
        }


    }
}