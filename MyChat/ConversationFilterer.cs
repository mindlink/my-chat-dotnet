using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat
{
    public class ConversationFilterer : IConversationFilterer
    {

        private const string CCAndPhoneNumberReplaceString = "*redacted*";
        private const string BlacklistedWordReplaceString = "*redacted*";

        public Conversation FilterConversation(Conversation conversation, ProgramOptions options)
        {
            Dictionary<ulong, string> obfuscateLookup = new Dictionary<ulong, string>();
            List<Message> filteredMessages = conversation.Messages.ToList();

            if (options.HideCCAndPhoneNumbers)
            {
                filteredMessages = filteredMessages.Select(m =>
                                        new Message(m.Timestamp, m.Sender, Regex.Replace(m.Content, @"\b(\d){16}\b", CCAndPhoneNumberReplaceString))).ToList();
                filteredMessages = filteredMessages.Select(m =>
                                        new Message(m.Timestamp, m.Sender, Regex.Replace(m.Content, @"\b(\d){10}\b", CCAndPhoneNumberReplaceString))).ToList();
            }

            if (options.UserFilter != null)
            {
                filteredMessages = conversation.Messages.Where(m => m.Sender == options.UserFilter).ToList();
            }

            if (options.KeywordFilter != null)
            {
                filteredMessages = filteredMessages.Where(m => m.Content.Contains(options.KeywordFilter)).ToList();
            }

            if (options.BlacklistedWords != null)
            {
                foreach(var blacklisted in options.BlacklistedWords)
                {
                    filteredMessages = filteredMessages.Select(m => m = new Message(m.Timestamp, m.Sender, m.Content.Replace(blacklisted, BlacklistedWordReplaceString))).ToList();
                }
            }

            if (options.ObfuscateUserIDs)
            {
                var obfuscatedMessages = new List<Message>();
                foreach (var message in filteredMessages)
                {
                    ulong hash = FNV1(message.Sender);
                    obfuscateLookup[hash] = message.Sender;
                    obfuscatedMessages.Add(new Message(message.Timestamp, hash.ToString(), message.Content));
                }
                return new Conversation(conversation.Name, obfuscatedMessages, "", null);
            }

            return new Conversation(conversation.Name, filteredMessages, "", null);
        }

        private ulong FNV1(string s)
        {
            ulong offsetPrime = 1099511628211;
            ulong offsetBasis = 14695981039346656037;

            byte[] bytes = Encoding.UTF8.GetBytes(s);

            ulong hash = offsetBasis;

            foreach (var b in bytes)
            {
                hash *= offsetPrime;
                hash ^= b;
            }

            return hash;
        }
    }
}
