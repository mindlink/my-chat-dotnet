using System;
using System.Collections.Generic;

namespace MindLink.Recruitment.MyChat
{
    /// <summary>
    /// Represents a helper to parse command line arguments.
    /// </summary>
    public sealed class CommandLineArgumentParser
    {
        /// <summary>
        ///   Parses the given <paramref name="arguments"/> into the exporter configuration.
        /// </summary>
        /// <param name="arguments">
        /// The command line arguments.
        /// </param>
        /// <returns>
        /// A <see cref="ConversationExporterConfiguration"/> representing the command line arguments.
        /// </returns>
        public ConversationExporterConfiguration ParseCommandLineArguments(string[] arguments)
        {
            try
            {
                Dictionary<string, string> argument_values = this.ParseArgumentValues(arguments, new string[] { "u", "k", "b" });

                return new ConversationExporterConfiguration(arguments[0], arguments[1], argument_values["u"], argument_values["k"], argument_values["b"], this.ParseArgumentFlag(arguments, "hn"), this.ParseArgumentFlag(arguments, "ouid"));
            }
            catch (IndexOutOfRangeException e) {
                throw new ArgumentException("Arguments are not specified in the correct format. Please consult the README for more information.");
            }
        }

        /// <summary>
        /// Retrieves values for a list of given arguments
        /// </summary>
        /// <param name="arguments">The arguments as passed at the command line</param>
        /// <param name="argument_list">The flags for each argument one wishes to retrieve a value for.</param>
        /// <returns>A dictionary where the keys are the flags for each argument and the values are the argument values.</returns>
        private Dictionary<string, string> ParseArgumentValues (string[] arguments, string[] argument_list)
        {
            Dictionary<string, string> argument_dict = new Dictionary<string, string>();

            for (int i = 0; i < arguments.Length; i++)
            {
                for (int arg = 0; arg < argument_list.Length; arg++)
                {
                    // Populate argument_dict with default null values
                    if (!argument_dict.ContainsKey(argument_list[arg]))
                    {
                        argument_dict[argument_list[arg]] = null;
                    }

                    if (arguments[i] == "-" + argument_list[arg])
                    {
                        argument_dict[argument_list[arg]] = arguments[i + 1];
                    }
                }
            }

            return argument_dict;
        }

        /// <summary>
        /// Determines whether a given flag is passed at the command line
        /// </summary>
        /// <param name="arguments">The arguments passed at the command line</param>
        /// <param name="flag">The flag to test for the presence of</param>
        /// <returns>True if the flag is present, false if it is not</returns>
        private bool ParseArgumentFlag (string[] arguments, string flag) 
        {
            return Array.IndexOf(arguments, "-" + flag) != -1;
        }
    }
}
