namespace MindLink.Recruitment.MyChat
{
    using System;

    /// <summary>
    /// Filters <see cref="Message"/> objects by keyword.
    /// </summary>
    public sealed class KeywordFilter : IMessageFilter
    {
        /// <summary>
        /// Stores the keyword to be filtered for
        /// </summary>
        public string Keyword { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="KeywordFilter"/> class.
        /// </summary>
        public KeywordFilter(string keyword)
        {
            this.Keyword = keyword;
        }

        /// <summary>
        /// Returns message if it contains keyword.
        /// </summary>
        /// <param name="message">
        /// Message object to be filtered.
        /// </param>
        public Message FilterMessage(Message message)
        {
            if (message.Content.IndexOf(Keyword, StringComparison.OrdinalIgnoreCase) != -1)
                return message;
            else return null;
        }
    }
}