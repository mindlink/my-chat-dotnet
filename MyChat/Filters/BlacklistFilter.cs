using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MindLink.Recruitment.MyChat.Filters
{
    public sealed class BlacklistFilter
    {
        /// <summary>
        /// Collection of messages.
        /// </summary>
        private List<Message> messages;

        public BlacklistFilter()
        {
        }
        /// <summary>
        /// Returns a conversation with words redacted from the blacklist.
        /// </summary>
        public Conversation filter(ConversationExporterConfiguration configuration, Conversation conversation)
        {
            messages = new List<Message>();
            messages = conversation.Messages.ToList();
            foreach (Message message in messages.ToList())
            {
                var messageToList = message.Content.Split(" ");
                var resultList = configuration.BlacklistWords.Select(x => messageToList);

                for (int i = 0; i < messageToList.Length; i++)
                {
                    string word = messageToList[i];
                    // Remove punctuation from any word provided in the blacklist
                    word = Regex.Replace(word, "(\\p{P})", "");

                    if (resultList != null)
                    {
                        foreach (string replace in configuration.BlacklistWords)
                        {
                            if (word.Equals(replace, StringComparison.OrdinalIgnoreCase))
                            {
                                messageToList[i] = "*redacted*";
                            }
                        }
                    }
                }
                message.Content = string.Join(' ', messageToList);
            }
            Conversation filtered = new Conversation { Name = conversation.Name, Messages = messages };
            return filtered;
        }

    }

}