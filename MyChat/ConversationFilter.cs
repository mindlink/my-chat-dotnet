using System.Collections.Generic;
using System.Linq;
using System;
using System.Text.RegularExpressions;
namespace MindLink.Recruitment.MyChat
{
    /// </summary>
    /// Provide functionality to filter by word/name or blacklist.
    /// </summary>
    public sealed class ConversationFilter
    {
        private List<Message> messages;

        public ConversationFilter()
        {
        }

        /// <summary>
        /// Parses the given <paramref name="arguments"/> into the exporter configuration.
        /// </summary>
        /// <returns>
        /// A <see cref="filtered"/> conversation.
        /// </returns>
        public Conversation FilterParser(ConversationExporterConfiguration configuration, Conversation conversation)
        {
            messages = new List<Message>();
            messages = conversation.Messages.ToList();

            if (messages.Count != 0)
            {
                foreach (Message message in messages.ToList())
                {
                    // Keyword filter
                    if (configuration.Filter != null && configuration.FilterID == false)
                    {
                        if (message.Content.IndexOf(configuration.Filter, StringComparison.OrdinalIgnoreCase) == -1)
                        {
                            messages.Remove(message);
                        }
                    }
                    // Name filter
                    else if (configuration.FilterID == true && configuration.Blacklist == false)
                    {
                        if (message.SenderId.IndexOf(configuration.Filter, StringComparison.OrdinalIgnoreCase) == -1)
                        {
                            messages.Remove(message);
                        }
                    }
                    // Redact words from blacklist
                    else if (configuration.Blacklist == true)
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
                    else if (configuration.PersonalNumbers == true)
                    {
                        // Redact Personal information
                        var messageToList = message.Content.Split(" ");
                        for (int i = 0; i < messageToList.Length; i++)
                        {
                            if (ValidateNumber(messageToList[i]))
                            {
                                messageToList[i] = "*redacted*";
                            }
                        }
                        message.Content = string.Join(' ', messageToList);
                    }
                }
            }
            Conversation filtered = new Conversation { Name = conversation.Name, Messages = messages };
            return filtered;
        }

        /// <summary>
        /// Helper function to detect a phone or credit card number in a message.
        /// </summary>
        public bool ValidateNumber(string number)
        {
            number = Regex.Replace(number, @"[^\d]", "");
            // Each line represents a different kind of credit card
            Regex creditExpression = new Regex(@"^(?:
                                                    4[0-9]{12}(?:[0-9]{3})?|
                                                    5[1-5][0-9]{14}|6(?:011| 
                                                    5[0-9][0-9])[0-9]{12}|
                                                    3[47][0-9]{13}|
                                                    3(?:0[0-5]|[68][0-9])[0-9]{11}|
                                                    (?:2131|1800|35\d{3})\d{11})$");

            Regex phoneExpression = new Regex("\\(?\\d{3}\\)?[-\\.]? *\\d{3}[-\\.]? *[-\\.]?\\d{4}");
            if (creditExpression.IsMatch(number) || phoneExpression.IsMatch(number))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}