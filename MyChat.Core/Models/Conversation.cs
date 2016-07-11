using MyChat.Core.Helpers;

namespace MindLink.Recruitment.MyChat.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    /// <summary>
    /// Represents the model of a conversation.
    /// </summary>
    public sealed class Conversation
    {
        /// <summary>
        /// The name of the conversation.
        /// </summary>
        public string name;

        /// <summary>
        /// The messages in the conversation.
        /// </summary>
        public IEnumerable<Message> messages;

        /// <summary>
        /// Initializes a new instance of the <see cref="Conversation"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the conversation.
        /// </param>
        /// <param name="messages">
        /// The messages in the conversation.
        /// </param>
        public Conversation(string name, IEnumerable<Message> messages)
        {
            this.name = name;
            this.messages = messages;
        }

        public IEnumerable<Message> FilterMessagesForUser(List<String> user)
        {

            if (messages == null || !messages.Any() || !user.Any())
            {
                Logger.Log("Message or user is null or empty");
                return new List<Message>();
            }

            //Assuming space is valid for username constuct the aggragate string 
            var UserId = user.Aggregate((current, next) => current + " " + next);

            if (user.Count > 1)
            {
                Logger.Log("Warning username filtering keyword containts spaces");
                //return messages;       
            }

            return messages.Where(i => i.senderId.Contains(UserId)); 
        }

        public IEnumerable<Message> FilterMessagesForKeyword(List<String> keyword)
        {

            if (messages == null || !messages.Any() || !keyword.Any())
            {
                Logger.Log("Message or keyword is null or empty");
                return new List<Message>();
            }

            //Case Insensitive keyword search
            var Keyword = keyword.Aggregate((current, next) => current + " " + next).ToLower();

            return messages.Where(i => i.content.ToLower().Contains(Keyword)); 
        }

        public IEnumerable<Message> HideWordsFromMessages(List<String> keyword)
        {

            if (messages == null || !messages.Any() || !keyword.Any())
            {
                Logger.Log("Message or keyword is null or empty");
                return new List<Message>();
            }

             //Case Insensitive keyword search and replace
             var Keyword = keyword.Aggregate((current, next) => current + " " + next).ToLower();
             messages.ToList().ForEach(i => i.content = Regex.Replace(i.content, Keyword, @"\*redacted\*", RegexOptions.IgnoreCase));

             return messages;
        }


    }
}
