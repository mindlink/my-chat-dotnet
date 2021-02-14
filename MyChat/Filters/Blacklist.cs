using System.Text.RegularExpressions;
using MindLink.Recruitment.MyChat.Data;

namespace MindLink.Recruitment.MyChat.Filters
{
    public class Blacklist : IFilter
    {
        private string[] _words;
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
            foreach (var message in conversation.messages)
            {
                foreach(var word in _words)
                {
                    var pattern = @"\b" + word + @"\b"; // The \b marks the boundary of the word.
                    message.content = Regex.Replace(message.content, pattern, _replacementWord, RegexOptions.IgnoreCase);
                }
            }
            return conversation;
        }
    }
}
