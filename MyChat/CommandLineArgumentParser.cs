namespace MindLink.Recruitment.MyChat
{
    using System;

    /// <summary>
    /// Represents a helper to parse command line arguments.
    /// </summary>
    public static class CommandLineArgumentParser
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
        public static ConversationExporterConfiguration ParseCommandLineArguments(string[] arguments)
        {
            if (arguments.Length < 1)
            {
                throw new ArgumentException("Must specify file to read from.");
            }
            if (arguments.Length < 2)
            {
                throw new ArgumentException("Must specify file to write to.");
            }

            ConversationExporterConfiguration cec = new ConversationExporterConfiguration(arguments[0], arguments[1]);

            for (int i = 0; i < arguments.Length; i++)
            {
                if (arguments[i] == "-fu")
                {
                    try
                    {
                        cec.userToFilter = arguments[i + 1];
                    }
                    catch
                    {
                        throw new ArgumentException("Must include userId after -fu.");
                    }
                }
                else if (arguments[i] == "-fk")
                {
                    try
                    {
                        cec.keywordToFilter = arguments[i + 1];
                    }
                    catch
                    {
                        throw new ArgumentException("Must include keyword after -fk.");
                    }
                }
                else if (arguments[i] == "-bw")
                {
                    try
                    {
                        cec.wordToBlacklist = arguments[i + 1];
                    }
                    catch
                    {
                        throw new ArgumentException("Must include blacklist word after -bw.");
                    }
                }
                else if (arguments[i] == "-bn")
                {
                    cec.blacklistNumbers = true;
                }
                else if (arguments[i] == "-o")
                {
                    cec.obfuscate = true;
                }
            }

            return cec;
        }
    }
}
