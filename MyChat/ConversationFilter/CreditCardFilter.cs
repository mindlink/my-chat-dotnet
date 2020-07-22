namespace MindLink.Recruitment.MyChat
{
    using System.Text.RegularExpressions;

    /// <summary>
    /// Redacts credit card numbers from <see cref="Message"/> objects.
    /// </summary>
    public sealed class CreditCardFilter : IMessageFilter
    {
        private Regex creditCardRegex;
        private string redactedCC;
        private Message filteredMessage;

        /// <summary>
        /// Initialises a new instance of the <see cref="CreditCardFilter"/> class.
        /// </summary>
        public CreditCardFilter()
        {
            creditCardRegex = new Regex(@"\b(?:\d[ -]*?){13,16}\b");
            redactedCC = "*redacted*";
        }

        /// <summary>
        /// Replaces all credit card numbers in a message with "*redacted*"
        /// </summary>
        /// <param name="message">
        /// Message object to be filtered.
        /// </param>
        public Message FilterMessage(Message message)
        {
            filteredMessage = message;
            filteredMessage.Content = creditCardRegex.Replace(message.Content, redactedCC);

            return filteredMessage;
        }
    }
}