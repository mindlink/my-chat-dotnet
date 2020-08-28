namespace MindLink.Recruitment.MyChat
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Represents filter methods to apply to conversation.
    /// </summary>
    public sealed class ConversationFilter
    {
        /// <summary>
        /// Imports the conversation at <paramref name="inputFilePath"/>.
        /// </summary>
        /// <param name="inputFilePath">
        /// The input file path.
        /// </param>
        /// <param name="searchName">
        /// The user id.
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
        public List<Message> FilterUser(string inputFilePath, string searchName, string censorCard, string censorPhone, string obfuscateUser)
        {
            var reader = new StreamReader(new FileStream(inputFilePath, FileMode.Open, FileAccess.Read), Encoding.ASCII);
            var messages = new List<Message>();
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                if (censorCard.Equals("yes", StringComparison.OrdinalIgnoreCase) || censorCard.Equals("y", StringComparison.OrdinalIgnoreCase))
                    line = ConversationCensor.CensorCardNumber(line);
                if (censorPhone.Equals("yes", StringComparison.OrdinalIgnoreCase) || censorPhone.Equals("y", StringComparison.OrdinalIgnoreCase))
                    line = ConversationCensor.CensorPhoneNumber(line);

                var split = line.Split(' ');
                string content = "";

                if (split[1].Equals(searchName, StringComparison.OrdinalIgnoreCase))
                {
                    if (obfuscateUser.Equals("yes", StringComparison.OrdinalIgnoreCase) || obfuscateUser.Equals("y", StringComparison.OrdinalIgnoreCase))
                        split[1] = Obfuscate.ObfuscateString(split[1]);

                    for (int i = 2; i < split.Length; i++) content += split[i] + " ";

                    messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(split[0])), split[1], content.TrimEnd()));
                }
            }

            return messages;
        }

        /// <summary>
        /// Imports the conversation at <paramref name="inputFilePath"/>.
        /// </summary>
        /// <param name="inputFilePath">
        /// The input file path.
        /// </param>
        /// <param name="searchKeyword">
        /// The keyword to filter.
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
        public List<Message> FilterKeyword(string inputFilePath, string searchKeyword, string censorCard, string censorPhone, string obfuscateUser)
        {
            var reader = new StreamReader(new FileStream(inputFilePath, FileMode.Open, FileAccess.Read), Encoding.ASCII);
            var messages = new List<Message>();
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                if (censorCard.Equals("yes", StringComparison.OrdinalIgnoreCase) || censorCard.Equals("y", StringComparison.OrdinalIgnoreCase))
                    line = ConversationCensor.CensorCardNumber(line);
                if (censorPhone.Equals("yes", StringComparison.OrdinalIgnoreCase) || censorPhone.Equals("y", StringComparison.OrdinalIgnoreCase))
                    line = ConversationCensor.CensorPhoneNumber(line);

                var split = line.Split(' ');
                string content = "";
                string keyword = split.FirstOrDefault(s => Regex.Replace(s, @"[^0-9a-zA-Z]+", "").Equals(searchKeyword, StringComparison.OrdinalIgnoreCase));

                if (keyword != null) // Test empty aswell
                {
                    if (obfuscateUser.Equals("yes", StringComparison.OrdinalIgnoreCase) || obfuscateUser.Equals("y", StringComparison.OrdinalIgnoreCase))
                        split[1] = Obfuscate.ObfuscateString(split[1]);

                    for (int i = 2; i < split.Length; i++) content += split[i] + " ";

                    messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(split[0])), split[1], content.TrimEnd()));
                }
            }

            return messages;
        }
    }
}
