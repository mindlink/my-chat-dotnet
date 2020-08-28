namespace MindLink.Recruitment.MyChat
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Represents methods that censor conversations.
    /// </summary>
    public sealed class ConversationCensor
    {
        /// <summary>
        /// Imports the conversation at <paramref name="inputFilePath"/>.
        /// </summary>
        /// <param name="inputFilePath">
        /// The input file path.
        /// </param>
        /// <param name="blacklistedWord">
        /// The word to be blacklisted from conversation.
        /// </param>
        /// <param name="censorCard">
        /// A flag to hide credit card numbers.
        /// </param>
        /// <param name="censorPhone">
        /// A flag to hide phone numbers.
        /// </param>
        /// <param name="obfuscateUser">
        /// A flag to obfuscate user ids.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when a path is invalid.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown when something bad happens.
        /// </exception>
        public List<Message> BlacklistWord(string inputFilePath, string blacklistedWord, string censorCard, string censorPhone, string obfuscateUser)
        {
            var reader = new StreamReader(new FileStream(inputFilePath, FileMode.Open, FileAccess.Read), Encoding.ASCII);
            var messages = new List<Message>();
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                if (censorCard.Equals("yes", StringComparison.OrdinalIgnoreCase) || censorCard.Equals("y", StringComparison.OrdinalIgnoreCase)) 
                    line = CensorCardNumber(line);
                if (censorPhone.Equals("yes", StringComparison.OrdinalIgnoreCase) || censorPhone.Equals("y", StringComparison.OrdinalIgnoreCase)) 
                    line = CensorPhoneNumber(line);
                
                var split = line.Split(' ');
                string content = "";

                for (int i = 2; i < split.Length; i++)
                {
                    string wordRegex = Regex.Replace(split[i], @"[^0-9a-zA-Z]+", "");

                    if (obfuscateUser.Equals("yes", StringComparison.OrdinalIgnoreCase) || obfuscateUser.Equals("y", StringComparison.OrdinalIgnoreCase))
                        split[1] = Obfuscate.ObfuscateString(split[1]);

                    content += wordRegex.Equals(blacklistedWord, StringComparison.OrdinalIgnoreCase) ? "*redacted*" + " " : split[i] + " ";
                }

                if (long.TryParse(split[0], out long timestamp)) messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(timestamp), split[1], content.TrimEnd()));
            }

            return messages;
        }

        /// <summary>
        /// Searches message to censor card number. <paramref name="s"/>.
        /// </summary>
        /// <param name="s">
        /// The message string.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when a path is invalid.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown when something bad happens.
        /// </exception>
        public static string CensorCardNumber(string s) => Regex.Replace(s, @"(([0-9]{4}(-| )){3}[0-9]{4})|(\b[0-9]{16}(?=[0-9]{0}\b))", "*redacted*");

        /// <summary>
        /// Searches message to censor phone number. <paramref name="s"/>.
        /// </summary>
        /// <param name="s">
        /// The message string.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when a path is invalid.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown when something bad happens.
        /// </exception>
        public static string CensorPhoneNumber(string s) => Regex.Replace(s, @"((\+44(\s\(0\)\s|\s0\s|\s)?)|0)7\d{3}(\s)?\d{6}", "*redacted*");
    }
}
