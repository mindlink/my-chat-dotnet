using System.Text.RegularExpressions;

namespace MindLink.MyChat.Domain.Transformers
{
    public class CreditCardTransformer : IMessageTransformer
    {
        private static readonly Regex regex = new Regex(
                "((4\\d{3})|(5[1-5]\\d{2})|(6011)|(34\\d{1})|(37\\d{1}))[- ]?\\d{4}[- ]?\\d{4}[- ]?\\d{4}|3[4,7][\\d\\s-]{15}");

        public Message TransformMessage(Message message)
        {
            var result = regex.Replace(message.Content, "*redacted*");

            return new Message(message.Timestamp, message.SenderId, result);
        }
    }
}