namespace MindLink.Recruitment.MyChat
{
    using System;
    using System.Globalization;

    public static class MessageGenerator
    {
        /// <summary>
        /// Creates a Message object given <paramref name="line"/>.
        /// </summary>
        /// <param name="line">
        /// The string representing all the content of message.
        /// </param>
        /// <param name="blacklist">
        /// The strings to be blacklisted from message content.
        /// </param>
        /// <returns>
        /// A <see cref="Message"/> representing the full message including timestamp, username and content.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when string representing the message is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when conversation is null or the conversation messages is null.
        /// </exception>
        public static Message CreateNewMessage(string line, string blacklist)
        {
            if (line == null)
            {
                throw new ArgumentNullException("line", "String containing Message cannot be null.");
            }

            var split = line.Split(' ');

            if (split.Length < 3)
            {
                throw new ArgumentOutOfRangeException("line", "String representing Message must contain at least 3 sections separated with whitespace.");
            }

            // sender ID is always the second argument in each line after timestamp and whitespace.
            string senderId = split[1];

            // the timestamp of message is at first position followed by whitespace.
            DateTimeOffset timestamp = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(split[0], CultureInfo.InvariantCulture));

            // the message content must be all the remaining strings in array with whitespace in between them.
            string content = ContentParser.ParseConversationContent(split, blacklist);

            // instantiate new Message object.
            return new Message(timestamp, senderId, content);
        }
    }
}
