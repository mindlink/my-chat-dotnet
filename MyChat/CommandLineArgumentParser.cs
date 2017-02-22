using System;
using System.Linq;

namespace MindLink.Recruitment.MyChat
{
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
        /// <returns>
        /// A <see cref="ConversationExporterConfiguration"/> representing the command line arguments.
        /// </returns>
        public ConversationExporterConfiguration ParseCommandLineArguments(string[] args)
        {
            ProgramArguments arguments = new ProgramArguments();
            
            if (!CommandLine.Parser.Default.ParseArguments(args, arguments))
            {
                throw new System.ArgumentException("Invalid Arguments");
            }
            return new ConversationExporterConfiguration(arguments);
        }
    }
}
