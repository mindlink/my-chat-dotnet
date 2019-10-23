namespace MindLink.Recruitment.MyChat
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

        public void FilterByUser(string user)
        {
            this.messages = from m in this.messages
                            where m.senderId == user
                            select m;
        }

        public void FilterByKeyword (string keyword)
        {
            this.messages = from m in this.messages
                            where Regex.IsMatch(m.content, @"\b(" + Regex.Escape(keyword) + @")\b", RegexOptions.IgnoreCase)
                            select m;
        }

        public void FilterBlacklist (string[] blacklistedWords)
        {
            List<string> blacklist = new List<string>();

            for (int i = 0; i < blacklistedWords.Length; i++)
            {
                if (blacklistedWords[i] != "")
                {
                    blacklist.Add(blacklistedWords[i]);
                }
            }

            string joined_blacklist = String.Join("|", blacklist);

            // Consider revising in future might be better to put a method in Message class for modifying content. 
            foreach (var m in this.messages)
            {
                m.content = Regex.Replace(m.content, @"\b(" + joined_blacklist + @")\b", "*redacted*", RegexOptions.IgnoreCase);
            }
        }

        public void HideCreditCardAndPhoneNumbers ()
        {
            foreach (var m in this.messages)
            {
                m.content = Regex.Replace(m.content, @"(((\d\s*){15}\d)|((\d\s*){10})\d)", "*redacted*");
            }
        }
    }
}
