using System;
using System.Collections.Generic;
using System.Linq;

namespace MindLink.Recruitment.MyChat.Filters
{
    public sealed class KeywordFilter
    {
        private List<Message> messages;
        public string Word { get; }

        public KeywordFilter(string word)
        {
            this.Word = word;
        }

        /// <summary>
        /// Return a conversation filterd by name.
        /// </summary>
        public Conversation filter(string filter, Conversation conversation)
        {
            messages = new List<Message>();
            messages = conversation.Messages.ToList();
            foreach (Message message in messages.ToList())
            {
                if (message.Content.IndexOf(filter, StringComparison.OrdinalIgnoreCase) == -1)
                {
                    messages.Remove(message);
                }
            }
            Conversation filtered = new Conversation { Name = conversation.Name, Messages = messages };
            return filtered;
        }

    }

}