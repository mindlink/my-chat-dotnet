using System;
using System.Linq;
using System.Text.RegularExpressions;
using MindLink.Recruitment.MyChat.Data;
using MindLink.Recruitment.MyChat.Exceptions;

namespace MindLink.Recruitment.MyChat.Filters
{
    public class Blacklist : IFilter
    {
        private readonly string[] _words;
        private const string _replacementWord = "*redacted*";

        public Blacklist(string[] words)
        {
            _words = words;
        }

        /// <summary>
        /// Returns the conversation with specific keywords redacted.
        /// </summary>
        /// <inheritdoc />
        public Conversation Filter(Conversation conversation)
        {
            if (conversation == null)
            {
                throw new ArgumentNullException("There must be a conversation to blacklist.");
            }

            if (conversation.messages.Count() == 0)
            {
                throw new NoMessagesException("There must be at least one message to apply blacklist filter to.");
            }

            if (_words.Length == 0)
            {
                throw new NoBlacklistedWordsException("You must specify at least one word to blacklist.");
            }

            foreach (var message in conversation.messages)
            {
                foreach (var word in _words)
                {
                    var pattern = @"\b" + word + @"\b"; // The \b marks the boundary of the word.
                    message.content = Regex.Replace(message.content, pattern, _replacementWord, RegexOptions.IgnoreCase);
                }
            }

            return new Conversation(conversation.name, conversation.messages);
        }
    }
}
