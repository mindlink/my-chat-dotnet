using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MindLink.Recruitment.MyChat
{
    public abstract class FilterOptions : ConversationOptions
    {
        public abstract List<Message> Filter(List<Message> messages, string filterTarget);

    }

    public class FilterByName : FilterOptions
    {
        /// <summary>
        /// Filter and return messages from user that matches chosen name
        /// </summary>
        /// <param name="messages">
        /// Messages from the conversation
        /// </param>
        /// <param name="nameToFilter">
        /// Name of a user
        /// </param>
        /// <returns>
        /// Filtered list of <see cref="Message">
        /// </returns>
        public override List<Message> Filter(List<Message> messages, string nameToFilter)
        {
            var filteredMessages = new List<Message>();

            foreach (var message in messages)
            {
                if (message.senderId.Equals(nameToFilter))
                {
                    filteredMessages.Add(message);
                }
            }

            return filteredMessages;
        }
    }

    public class FilterByWord : FilterOptions
    {
        /// <summary>
        /// Filter and return messages containing the keyword
        /// </summary>
        /// <param name="messages">
        /// Messages from the conversation
        /// </param>
        /// <param name="wordToFilter">
        /// The keyword to filter the message
        /// </param>
        /// <returns>
        /// Filtered list of <see cref="Message">
        /// </returns>
        public override List<Message> Filter(List<Message> messages, string wordToFilter)
        {
            try
            {
                var filteredMessages = new List<Message>();

                foreach (var message in messages)
                {
                    var messageContent = message.content;
                    var messageContentArray = messageContent.Split(' ');

                    Regex wordToFilterRegex = new Regex(@"\b" + wordToFilter + @"[!?.,']?\b", RegexOptions.IgnoreCase);
                    
                    foreach(var word in messageContentArray)
                    {
                        var matches = wordToFilterRegex.Matches(word);
                        if (matches.Count > 0)
                        {
                            filteredMessages.Add(message);
                            break;
                        }
                    }
                }

                return filteredMessages;
            }
            catch (ArgumentException) 
            {
                throw new ArgumentException("Something went wrong with the regular expression!");
            }
        }
    }
}