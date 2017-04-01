using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindLink.MyChat.Transformers
{
    public class BlacklistTransformer : IMessageTransformer
    {
        private readonly string[] blacklist;

        public BlacklistTransformer(IEnumerable<string> blacklist)
        {
            this.blacklist = blacklist.Select(word => word.ToLower()).Distinct().ToArray();
        }

        public Message TransformMessage(Message message)
        {
            var result = this.blacklist.Aggregate(message.Content, Replace);

            return new Message(message.Timestamp, message.SenderId, result);
        }

        /// <summary>
        /// Custom implementation of replace for ignoring case
        /// </summary>
        /// <param name="current"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        private static string Replace(string current, string word)
        {
            var sb = new StringBuilder();

            var previousIndex = 0;
            var index = current.IndexOf(word, StringComparison.InvariantCultureIgnoreCase);

            while (index != -1)
            {
                sb.Append(current.Substring(previousIndex, index - previousIndex));
                sb.Append("*redacted*");
                index += word.Length;

                previousIndex = index;
                index = current.IndexOf(word, index, StringComparison.InvariantCultureIgnoreCase);
            }

            sb.Append(current.Substring(previousIndex));

            return sb.ToString();
        }
    }
}