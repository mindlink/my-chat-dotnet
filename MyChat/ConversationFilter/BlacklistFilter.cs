namespace MindLink.Recruitment.MyChat
{
    using System;
    using System.Linq;

    /// <summary>
    /// Filters <see cref="Message"/> objects by keyword blacklist.
    /// </summary>
    public sealed class BlacklistFilter : IMessageFilter
    {
        /// <summary>
        /// Stores the blacklist to be filtered out
        /// </summary>
        public string[] KeywordBlacklist { get; }
        private Message filteredMessage;

        /// <summary>
        /// Initialises a new instance of the <see cref="BlacklistFilter"/> class.
        /// </summary>
        public BlacklistFilter(string[] keywordBlacklist)
        {
            this.KeywordBlacklist = keywordBlacklist;
        }

        /// <summary>
        /// Replaces all instances of blacklisted words in message content with "*redacted*"
        /// </summary>
        /// <param name="message">
        /// Message object to be filtered.
        /// </param>
        public Message FilterMessage(Message message)
        {
            filteredMessage = message;

            string[] split = filteredMessage.Content.Split(' ');

            for (int i = 0; i < split.Length; i++)
            {
                string word = split[i];
                string strippedWord = new string(word.Where(c => !char.IsPunctuation(c)).ToArray());

                foreach (string blockedWord in KeywordBlacklist)
                {
                    if (strippedWord.Equals(blockedWord, StringComparison.OrdinalIgnoreCase))
                    {
                        split[i] = "*redacted*";
                    }
                }

            }
            filteredMessage.Content = string.Join(' ', split);

            return filteredMessage;
        }
    }
}
