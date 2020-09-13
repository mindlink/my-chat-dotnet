using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MindLink.Recruitment.MyChat.Filters
{
    public sealed class InformationFilter
    {
        private List<Message> messages;

        public InformationFilter()
        {
        }

        /// <summary>
        /// Returns a conversation object with personal information redacted.
        /// </summary>
        public Conversation filter(Conversation conversation)
        {
            messages = new List<Message>();
            messages = conversation.Messages.ToList();
            foreach (Message message in messages.ToList())
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
