namespace MindLink.Recruitment.MyChat
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Responsible for filtering <see cref="Conversation"/> objects.
    /// </summary>
    public class ConversationFilter : IConversationFilter
    {
        /// <summary>
        /// Filters a <see cref="Conversation"/> object according to a <see cref="ConversationConfig"/> object.
        /// </summary>
        /// <param name="configuration">
        /// The configuration object.
        /// </param>
        /// <param name="conversation">
        /// The conversation object.
        /// </param>
        public Conversation FilterConversation(ConversationConfig configuration, Conversation conversation)
        {
            IList<Message> messages = new List<Message>();
            try
            {
                messages = conversation.Messages.ToList();
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentException("Conversation contains zero messages.");
            }
            
            IList<Message> filteredMessages = new List<Message>();
            IDictionary<string, int> obfuscatedUsers = new Dictionary<string, int>();
            int obfuscatedIDCount = 1;
            string redacted = "**redacted**";

            foreach (Message message in messages.ToList())
            {
                if (configuration.UserFilter != null && !message.SenderId.Equals(configuration.UserFilter))
                {
                    messages.Remove(message);
                }
                if (configuration.KeywordFilter != null && message.Content.IndexOf(configuration.KeywordFilter, StringComparison.OrdinalIgnoreCase) == -1)
                {
                    messages.Remove(message);
                }
                if (configuration.KeywordBlacklist != null)
                {
                    string[] split = message.Content.Split(' ');

                    for (int i = 0; i < split.Length; i++)
                    {
                        string word = split[i];
                        string strippedWord = new string(word.Where(c => !char.IsPunctuation(c)).ToArray());

                        foreach (string blockedWord in configuration.KeywordBlacklist)
                        {
                            if (strippedWord.Equals(blockedWord, StringComparison.OrdinalIgnoreCase))
                            {
                                split[i] = redacted;
                            }
                        }

                    }
                    message.Content = string.Join(' ', split);
                }

                if (configuration.HidePhoneNumbers)
                {
                    Regex rx = new Regex(@"[\d\s-\(\)]{10,}");
                    message.Content = rx.Replace(message.Content, redacted);
                }

                if (configuration.HideCreditCards)
                {
                    Regex rx = new Regex(@"\b(?:\d[ -]*?){13,16}\b");
                    message.Content = rx.Replace(message.Content, redacted);
                }
                if (configuration.ObfuscateUserID)
                {
                    if (obfuscatedUsers.ContainsKey(message.SenderId))
                    {
                        message.SenderId = obfuscatedUsers[message.SenderId].ToString();
                    }
                    else
                    {
                        obfuscatedUsers.Add(message.SenderId, obfuscatedIDCount);
                        message.SenderId = obfuscatedUsers[message.SenderId].ToString();
                        obfuscatedIDCount += 1;
                    }
                }
            }
            Conversation filteredConversation = new Conversation { Name = conversation.Name, Messages = messages };
            return filteredConversation;
        }
    }
}