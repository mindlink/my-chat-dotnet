namespace MindLink.Recruitment.MyChat
{
    using System;
    using System.Collections.Generic;
    /// <summary>
    /// Represents a helper to parse command line arguments.
    /// </summary>
    public sealed class CommandLineArgumentParser
    {

        /// <summary>
        /// Parses the given <paramref name="arguments"/> into the exporter configuration.
        /// </summary>
        /// <param name="arguments">
        /// The command line arguments.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when not enough command line arguments provided.
        /// </exception>
        /// <returns>
        /// A <see cref="ConversationExporterConfiguration"/> representing the command line arguments.
        /// </returns>
        public ConversationExporterConfiguration ParseCommandLineArguments(string[] arguments)
        {
            if (arguments.Length < 2)
            {
                throw new ArgumentException("Must provide at least an input file path and an output file path.");
            }

            string user = ParseParameterSingle("-u", arguments);
            string keyword = ParseParameterSingle("-k", arguments);
            var blacklist = ParseParameterList("-b", arguments);

            return new ConversationExporterConfiguration(arguments[0], arguments[1], user, keyword, blacklist);
        }

        /// <summary>
        /// Searches <paramref name="arguments"/> for the parameter corresponding to <paramref name="flag"/>
        /// </summary>
        /// <param name="flag">
        /// The flag indicating which parameter is to follow.
        /// </param>
        /// <param name="arguments">
        /// The list of commandline arguments provided.
        /// </param>
        /// <returns>
        /// The parameter corresponding the to flag, or empty string if flag is not present.
        /// </returns>
        private string ParseParameterSingle(string flag, string[] arguments)
        {
            int index = Array.IndexOf(arguments, flag); //Get the index of the flag.
            if (index + 1 < arguments.Length && index != -1) //If the flag is found and a parameter follows, return the parameter.
            {
                return arguments[index + 1];
            }
            return ""; //Otherwise return empty string.
        }

        /// <summary>
        /// Searches <paramref name="arguments"/> for multiple parameters corresponding to <paramref name="flag"/>
        /// </summary>
        /// <param name="flag">
        /// The flag indicating which parameter is to follow.
        /// </param>
        /// <param name="arguments">
        /// The list of commandline arguments provided.
        /// </param>
        /// <returns>
        /// The list of parameters corresponding to the flag, or null if the flag is not present.
        /// </returns>
        private List<string> ParseParameterList(string flag, string[] arguments)
        {
            var blacklist = new List<string>();
            int index = Array.IndexOf(arguments, flag); //Get the index of the flag.
            while (index + 1 < arguments.Length && index != -1) //While there are more arguments, add them to the list.
            {
                if (arguments[index + 1] == "-u" || arguments[index + 1] == "-k" || arguments[index + 1] == "-b") //Break from the loop if another flag is found.
                {
                    break;
                }
                blacklist.Add(arguments[index + 1]);
                ++index;
            }
            if (blacklist.Count == 0)
            {
                return null;
            }
            return blacklist;
        }
    }
}
