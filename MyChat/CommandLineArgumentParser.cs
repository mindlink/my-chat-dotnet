namespace MindLink.Recruitment.MyChat
{
    using System;

    /// <summary>
    /// Represents a helper to parse command line arguments.
    /// </summary>
    public sealed class CommandLineArgumentParser
    {
        private const string Username = "/name:";
        private const string Keyword = "/keyword:";
        private const string Blacklist = "/blacklist:";
        private const string Encrypt = "/encrypt";
        private const string HiddenNums = "/hidenums";

        /// <summary>
        /// Parses the given <paramref name="arguments"/> into the exporter configuration.
        /// </summary>
        /// <param name="arguments">
        /// The command line arguments.
        /// </param>
        /// <returns>
        /// A <see cref="ConversationExporterConfiguration"/> representing the command line arguments.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the arguments is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when any of the given arguments is empty.
        /// </exception>
        public ConversationExporterConfiguration ParseCommandLineArguments(string[] arguments)
        {
            // Added check for null arguments.
            if (arguments == null)
            {
                throw new ArgumentNullException("arguments", "Command Line arguments missing.");
            }

            // Added check to ensure only a minimum of 2 or maximum of 6 arguments.
            if (arguments.Length < 2 || arguments.Length > 6)
            {
                throw new ArgumentOutOfRangeException("arguments", "Command Line arguments are invalid. Application can only accept a minimum of 2 or maximum of 6 arguments.");
            }

            string input = arguments[0];
            string output = arguments[1];
            string username = null;
            string keyword = null;
            string blacklist = null;
            bool encryptUsernames = false;
            bool hideNumbers = false;

            SetAdditionalParameters(arguments, out username, out keyword, out blacklist, out encryptUsernames, out hideNumbers);

            return new ConversationExporterConfiguration(input, output, encryptUsernames, hideNumbers, username, keyword, blacklist);
        }

        /// <summary>
        /// Sets the additional filter and encryption parameters.
        /// </summary>
        /// <param name="arguments">
        /// The command line arguments.
        /// </param>
        /// <param name="username">
        /// The username filter to be set in configuration.
        /// </param>
        /// <param name="keyword">
        /// The keyword filter to be set in configuration.
        /// </param>
        /// <param name="blacklist">
        /// The blacklist filter to be set in configuration.
        /// </param>
        /// <param name="encryptUsernames">
        /// The encryption boolean to be set in configuration.
        /// </param>
        /// <param name="hideNumbers">
        /// Boolean flag indicating whether to hide numbers or not.
        /// </param>
        private static void SetAdditionalParameters(
            string[] arguments, 
            out string username, 
            out string keyword, 
            out string blacklist, 
            out bool encryptUsernames,
            out bool hideNumbers)
        {
            username = null;
            keyword = null;
            blacklist = null;
            encryptUsernames = false;
            hideNumbers = false;

            // Iterating through arguments exluding first 2 which are always input/output filenames.
            for (int i = 2; i < arguments.Length; i++)
            {
                // checking if argument is username filter
                username = arguments[i].StartsWith(Username, StringComparison.OrdinalIgnoreCase) ? arguments[i].Replace(Username, "") : username;

                // checking if argument is keyword filter
                keyword = arguments[i].StartsWith(Keyword, StringComparison.OrdinalIgnoreCase) ? arguments[i].Replace(Keyword, "") : keyword;

                // checking if argument is blacklist filter
                blacklist = arguments[i].StartsWith(Blacklist, StringComparison.OrdinalIgnoreCase) ? arguments[i].Replace(Blacklist, "") : blacklist;

                // checking if encryption is required for usernames.
                // if this is set to true before additional arguments, keep it true.
                encryptUsernames = encryptUsernames ? true : arguments[i] == Encrypt;

                hideNumbers = hideNumbers ? true : arguments[i] == HiddenNums;
            }
        }
    }
}
