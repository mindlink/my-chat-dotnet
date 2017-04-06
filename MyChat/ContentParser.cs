namespace MindLink.Recruitment.MyChat
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Represents a parser that can read the conversation and return the content section.
    /// </summary>
    public static class ContentParser
    {
        /// <summary>
        /// Helper method to read the content section of conversation from <paramref name="splitedConversation"/>.
        /// </summary>
        /// <param name="splitedConversation">
        /// The conversation after is has been splitted in string array with whitespace separator.
        /// </param>
        /// <returns>
        /// A <see cref="string"/> representing the content section of conversation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the <see cref="splitedConversation"/> is null.
        /// </exception>
        public static string ParseConversationContent(string[] splitedConversation, string blacklist = null)
        {
            if (splitedConversation == null)
            {
                throw new ArgumentNullException("splitedConversation", "Error while parsing message string. Message string is null.");
            }

            // StringBuilder for concatenation in tight loops.
            StringBuilder sb = new StringBuilder();

            // loop through all splits except first 2.
            for (int i = 2; i < splitedConversation.Length; i++)
            {
                // Check if blacklist is to be applied.
                if (blacklist != null)
                {
                    string contentSplit = splitedConversation[i];

                    // to handle cases of multiple blacklisted words separated by comma.
                    string[] blackListArr = blacklist.Replace(" ", "").ToUpperInvariant().Split(',');

                    // use this regex to remove any non alphanumeric characters from the string during comparison against blacklisted words.
                    string onlyAlphaNum = new Regex("[^a-zA-Z0-9 -]").Replace(contentSplit, "");

                    // Check if word is equal to blacklisted. UpperInvariant is used to avoid case-sensitive cases.
                    if (blackListArr.Contains(onlyAlphaNum.ToUpperInvariant()))
                    {
                        contentSplit = contentSplit.Replace(onlyAlphaNum, new string('*', onlyAlphaNum.Length));
                    }

                    // append the content of conversation.
                    sb.Append(contentSplit);
                }
                else
                {
                    sb.Append(splitedConversation[i]);
                }

                // Add whitespace after each split.
                sb.Append(' ');
            }

            // return string and trim whitespace at end.
            return sb.ToString().TrimEnd();
        }

        /// <summary>
        /// Iterates through Conversation messages to hide numbers.
        /// </summary>
        /// <param name="convesation">
        /// String representing the message's content.
        /// </param>
        /// <returns>
        /// String representing the message content after sensitive numbers have been hidden.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when conversation is null or the conversation messages is null.
        /// </exception>
        public static void HideNumbersFromConvesation(Conversation conversation)
        {
            if (conversation == null)
            {
                throw new ArgumentNullException("conversation", "Error while parsing message string. Message string is null.");
            }

            if (conversation.Messages == null)
            {
                throw new ArgumentNullException("conversation", "Conversation did not contain any messages.");
            }

            foreach (Message message in conversation.Messages)
            {
                HideNumbersFromContent(message);
            }
        }

        /// <summary>
        /// Hides sensitive numbers from message's content.
        /// </summary>
        /// <param name="message">
        /// The message to hide numbers from.
        /// </param>
        /// <returns>
        /// String representing the message content after sensitive numbers have been hidden.
        /// </returns>
        private static void HideNumbersFromContent(Message message)
        {
            // Instantiate the regular expression object.
            Regex creditCardRegex = new Regex(ConfigurationManager.AppSettings["creditCardRegex"], RegexOptions.IgnoreCase);

            // Match the regular expression pattern against a text string.
            Match matchingString = creditCardRegex.Match(message.Content);

            // Check if regex matching was successful.
            if (matchingString.Success)
            {
                // Replace matched string with stars.
                message.Content = Regex.Replace(message.Content, matchingString.Value, new string('*', matchingString.Value.Length));
            }

            // Instantiate the regular expression object.
            Regex phoneNumberRegex = new Regex(ConfigurationManager.AppSettings["phoneNumberRegex"], RegexOptions.IgnoreCase);

            // Match the regular expression pattern against a text string.
            matchingString = phoneNumberRegex.Match(message.Content);

            // Check if regex matching was successful.
            if (matchingString.Success)
            {
                // Replace matched string with stars.
                message.Content = Regex.Replace(message.Content, matchingString.Value, new string('*', matchingString.Value.Length));
            }
        }
    }
}
