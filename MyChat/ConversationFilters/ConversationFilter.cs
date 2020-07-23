namespace MindLink.Recruitment.MyChat.ConversationFilters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MindLink.Recruitment.MyChat.ConversationData;
    using MindLink.Recruitment.MyChat.CommandLineParsing;

    /// <summary>
    /// Responsible for filtering <see cref="Conversation"/> objects.
    /// </summary>
    public sealed class ConversationFilter : IConversationFilter
    {
        private IList<Message> messages;
        private IDictionary<string, int> obfuscatedUsers;
        private int obfuscatedIDCount;

        /// <summary>
        /// Initialises a new instance of the <see cref="ConversationFilter"/>
        /// </summary>
        public ConversationFilter()
        {
            obfuscatedIDCount = 1;
        }

        /// <summary>
        /// Filters a <see cref="Conversation"/> object according to a <see cref="ConversationConfig"/> object.
        /// </summary>
        /// <param name="configuration">
        /// The configuration object.
        /// </param>
        /// <param name="conversation">
        /// The conversation object.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the conversation argument has zero messages.
        /// </exception>
        public Conversation FilterConversation(ConversationConfig configuration, Conversation conversation)
        {
            messages = new List<Message>();
            obfuscatedUsers = new Dictionary<string, int>();

            try
            {
                messages = conversation.Messages.ToList();
            }
            catch (ArgumentNullException e)
            {
                throw new ArgumentException("Conversation contains zero messages.", e);
            }                    

            foreach (Message message in messages.ToList())
            {
                Message filteredMessage;

                if (configuration.Filters != null)
                {
                    foreach (IMessageFilter messageFilter in configuration.Filters)
                    {
                        filteredMessage = messageFilter.FilterMessage(message);
                        if (filteredMessage == null)
                            messages.Remove(message);
                    }
                }

                if (configuration.ObfuscateUserID)
                    ObfuscateUsers(message);
            }
            Conversation filteredConversation = new Conversation { Name = conversation.Name, Messages = messages };
            return filteredConversation;
        }

        /// <summary>
        /// Obfuscates user id of <see cref="Message"/> objects, retaining relationship between message and sender throughout a <see cref="Conversation"/>
        /// </summary>
        /// <param name="message">
        /// Message obect requiring user id obfuscation.
        /// </param>
        private void ObfuscateUsers(Message message)
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
}