namespace MindLink.Recruitment.MyChat.ConversationFilters
{
    using System.Text.RegularExpressions;
    using MindLink.Recruitment.MyChat.ConversationData;

    /// <summary>
    /// Redacts phone numbers from <see cref="Message"/> objects.
    /// </summary>
    public sealed class PhoneNumberFilter : IMessageFilter
    {
        private Regex phoneNumberRegex;
        private string redactedPN;
        private Message filteredMessage;

        /// <summary>
        /// Initialises a new instance of the <see cref="PhoneNumberFilter"/> class.
        /// </summary>
        public PhoneNumberFilter()
        {
            phoneNumberRegex = new Regex(@"[\d\s-\(\)]{10,}");
            redactedPN = "*redacted*";
        }

        /// <summary>
        /// Replaces all phone numbers in a message with "*redacted*"
        /// </summary>
        /// <param name="message">
        /// Message object to be filtered.
        /// </param>
        public Message FilterMessage(Message message)
        {
            filteredMessage = message;
            filteredMessage.Content = phoneNumberRegex.Replace(message.Content, redactedPN);

            return filteredMessage;
        }
    }
}