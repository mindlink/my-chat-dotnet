using System.Linq;

namespace MindLink.MyChat.Transformers
{
    public class PhoneNumberTransformer : IMessageTransformer
    {
        private readonly PhoneNumbers.PhoneNumberUtil util = PhoneNumbers.PhoneNumberUtil.GetInstance();

        public Message TransformMessage(Message message)
        {
            var matches = this.util.FindNumbers(message.Content, "US");

            var result = matches.Aggregate(message.Content, (current, match) => current.Replace(match.RawString, "*redacted*"));

            return new Message(message.Timestamp, message.SenderId, result);
        }
    }
}
