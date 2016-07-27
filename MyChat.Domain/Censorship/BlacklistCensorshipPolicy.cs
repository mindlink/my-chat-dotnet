namespace MindLink.Recruitment.MyChat.Domain.Censorship
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public sealed class BlacklistCensorshipPolicy : ICensorshipPolicy
    {
        private readonly List<string> _blacklist;
        private readonly string _replacement;

        /// <summary>
        /// Initializes a <see cref="BlacklistCensorshipPolicy"/>
        /// </summary>
        /// <param name="blacklist">The list of words to censor.</param>
        /// <param name="replacement">The phrase to replace each blacklisted word.</param>
        public BlacklistCensorshipPolicy(IEnumerable<string> blacklist, string replacement)
        {
            if (blacklist == null)
                throw new ArgumentNullException($"The value of '{nameof(blacklist)}' cannot be null.");

            if (replacement == null)
                throw new ArgumentNullException($"The value of {nameof(replacement)} cannot be null.");

            _blacklist = new List<string>(blacklist);
            _replacement = replacement;
        }

        /// <summary>
        /// Censors the specified value.
        /// </summary>
        /// <param name="value">The value to censor.</param>
        /// <returns></returns>
        public string Censor(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            var pattern = $"\\b({string.Join("|", _blacklist.Select(Regex.Escape))})\\b";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.Replace(value, _replacement);
        }
    }
}
