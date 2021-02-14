using System.Collections.Generic;
using System.Linq;

namespace MindLink.Recruitment.MyChat.Filters
{
    public class KeywordFilter : IFilter
    {
        private string[] _keywords;

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
