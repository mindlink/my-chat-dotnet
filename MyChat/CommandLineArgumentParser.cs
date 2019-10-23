using System;

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
            return new ConversationExporterConfiguration(arguments[0], arguments[1], this.ParseArgumentValue(arguments, "u"), this.ParseArgumentValue(arguments, "k"), this.ParseArgumentValue(arguments, "b"), this.ParseArgumentFlag(arguments, "hn"));
        }

        private string ParseArgumentValue (string[] arguments, string argument)
        {
            // Will need to put an exception here for out of bounds
            int argument_index = Array.IndexOf(arguments, '-' + argument);

            if (argument_index == - 1)
            {
                return null;
            }

            return arguments[argument_index + 1];
        }

        public bool ParseArgumentFlag (string[] arguments, string flag) 
        {
            return Array.IndexOf(arguments, "-" + flag) != -1;
        }
    }
}
