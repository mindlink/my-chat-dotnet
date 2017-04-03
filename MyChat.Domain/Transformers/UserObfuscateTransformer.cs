using System.Text;
using Murmur;

namespace MindLink.MyChat.Domain.Transformers
{
    public class UserObfuscateTransformer : IMessageTransformer
    {
        private readonly Murmur32 hashFunc = MurmurHash.Create32();

        public Message TransformMessage(Message message)
        {
            this.hashFunc.Initialize();

            var hashed = this.hashFunc.ComputeHash(Encoding.UTF8.GetBytes(message.SenderId));

            return new Message(message.Timestamp, FormatHash(hashed), message.Content);
        }

        public static string FormatHash(byte[] hash)
        {
            var hex = new StringBuilder(hash.Length * 2);
            foreach (var b in hash)
            {
                hex.AppendFormat("{0:x2}", b);
            }

            return hex.ToString();
        }
    }
}