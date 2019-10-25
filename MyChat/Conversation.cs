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
        /// List of the most activated users. Generated in response 
        /// to -mau flag.
        /// </summary>
        public List<User> mostActiveUsers;

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

        public void ObfuscateUserID ()
        {
            Dictionary<string, string> userIDs = new Dictionary<string, string>();

            int user_number = 1;

            foreach (var m in this.messages)
            {
                if (!userIDs.ContainsKey(m.senderId))
                {
                    userIDs.Add(m.senderId, "Hidden User " + user_number.ToString());
                    user_number++;
                }

                m.senderId = userIDs[m.senderId];
            }

            foreach (var user in userIDs)
            {
                foreach (var m in this.messages)
                {
                    m.content = Regex.Replace(m.content, @"\b(" + user.Key + @")\b", user.Value, RegexOptions.IgnoreCase);
                }
            }
        }
          
        public void generateMostActiveUsersReport ()
        {
            UserList users = new UserList();

            foreach (var m in this.messages)
            {
                if (!users.Contains(m.senderId))
                {
                    users.Add(new User(m.senderId, 1));
                } else
                {
                    users[m.senderId].IncrementMessageCount();
                }
            }

            users.SortByActivity();

            this.mostActiveUsers = users.ToList();
        }
    }
}
