using System;
using System.Collections.Generic;
using System.Linq;

namespace MindLink.Recruitment.MyChat.Filters
{
    public sealed class NameFilter
    {
        private List<Message> messages;
        public string Name { get; }

        public NameFilter(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Returns a conversation object filtered by Sender.
        /// </summary>
        public Conversation filter(string filter, Conversation conversation)
        {
            messages = new List<Message>();
            messages = conversation.Messages.ToList();
            foreach (Message message in messages.ToList())
            {
                if (message.SenderId.IndexOf(filter, StringComparison.OrdinalIgnoreCase) == -1)
                {
                    messages.Remove(message);
                }
            }
            Conversation filtered = new Conversation { Name = conversation.Name, Messages = messages };
            return filtered;
        }
    }
}
