namespace MindLink.Recruitment.MyChat
{
    using System;
    using System.Configuration;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Represents a helper class to assist with filtering the messages.
    /// </summary>
    public static class MessageFilter
    {
        /// <summary>
        /// Decides whether message should be included in export or not.
        /// </summary>
        /// <param name="message">
        /// The Message object to be filtered.
        /// </param>
        /// <param name="userName">
        /// The username to filter messages with.
        /// </param>
        /// <param name="keyword">
        /// The keywords to filter messages with.
        /// </param>
        /// <returns>
        /// Boolean value indicating whether the message should be added to export or not.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when conversation is null or the conversation messages is null.
        /// </exception>
        public static bool Filter(Message message, string userName, string keyword)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message", "Message cannot be null.");
            }

            // Check if both filters are enabled.
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(keyword))
            {
                return message.SenderId == userName && message.Content.Contains(keyword);
            }
            else
            {
                // Only if username filter is enabled.
                if (!string.IsNullOrEmpty(userName))
                {
                    return message.SenderId == userName;
                }

                // Only if keyword filter is enabled.
                if (!string.IsNullOrEmpty(keyword))
                {
                    return message.Content.Contains(keyword);
                }

                return true;
            }
        }
    }
}
