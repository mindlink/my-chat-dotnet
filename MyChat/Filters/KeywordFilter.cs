using System;
using System.Collections.Generic;
using System.Linq;
using MindLink.Recruitment.MyChat.Data;
using MindLink.Recruitment.MyChat.Exceptions;

namespace MindLink.Recruitment.MyChat.Filters
{
    public class KeywordFilter : IFilter
    {
        private readonly string[] _keywords;

        public KeywordFilter(string[] keywords)
        {
            _keywords = keywords;
        }

        /// <summary>
        /// Returns all messages that contains a specified keyword.
        /// </summary>
        /// <inheritdoc />
        public Conversation Filter(Conversation conversation)
        {
            if (conversation == null)
            {
                throw new ArgumentNullException("There must be a conversation to filter.");
            }

            if (conversation.messages.Count() == 0)
            {
                throw new NoMessagesException("There must be at least one message to filter.");
            }

            if (_keywords.Length == 0)
            {
                throw new NoKeywordsException("You must specify at least one keyword to filter.");
            }

            var newMessages = new List<Message>();

            foreach (var message in conversation.messages)
            {
                foreach (var keyword in _keywords)
                {
                    if (message.content.Contains(keyword))
                    {
                        newMessages.Add(message);
                    }
                }
            }

            return new Conversation(conversation.name, newMessages.Distinct());
        }
    }
}
