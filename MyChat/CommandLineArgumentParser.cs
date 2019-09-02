namespace MindLink.Recruitment.MyChat
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Represents a helper to parse command line arguments.
    /// </summary>
    public static class CommandLineArgumentParser
    {
        #region Methods
        /// <summary>
        /// Parses the given <paramref name="arguments"/> and writes help information in console application if help keyword is preset.
        /// </summary>
        public static void ReturnHelp(string[] arguments)
        {
            if (Array.IndexOf(arguments, "help") > -1)
            {
                Console.Write("-I inputfile.txt\n" +
                              "-O outputfile.json\n" +
                              "Optional arguments:\n" +
                              "-U userId (Only messages sent by specified user will be exported)\n" +
                              "-B blacklist.txt (Any words matching those given in blacklist will be redacted)\n" +
                              "-K keyword (Only messages containing specified keyword will be exported)\n" +
                              "-F (If set, obfuscates user identities)\n");
            }
        }

        /// <summary>
        /// Parses the given <paramref name="arguments"/> and returns input file path as string.
        /// </summary>
        public static string GetInputFilePath(string[] arguments)
        {
            if (Array.IndexOf(arguments, "-I") > -1)
            {
                var outputFilePath = arguments[Array.IndexOf(arguments, "-I") + 1];
                return outputFilePath;
            }
            else
            {
                throw new ArgumentNullException("No input file given. Input file can be specified via '-I inputfile.txt'. Type 'help' for full list of args.");
            }
        }

        /// <summary>
        /// Parses the given <paramref name="arguments"/> and returns output file path as string.
        /// </summary>
        public static string GetOutputFilePath(string[] arguments)
        {
            if (Array.IndexOf(arguments, "-O") > -1)
            {
                var outputFilePath = arguments[Array.IndexOf(arguments, "-O") + 1];
                return outputFilePath;
            }
            else
            {
                throw new ArgumentNullException("No output file given. Output file can be specified via '-O outputfile.json'. Type 'help' for full list of args.");
            }
        }

        /// <summary>
        /// Parses the given <paramref name="arguments"/> and returns username as string.
        /// </summary>
        public static string GetUsernameFilter(string[] arguments)
        {
            if (Array.IndexOf(arguments, "-U") > -1)
            {
                try
                {
                    var userIdFilter = arguments[Array.IndexOf(arguments, "-U") + 1];
                    return userIdFilter;
                }
                catch (IndexOutOfRangeException exception)
                {
                    throw new ArgumentNullException("No username specified after -U", exception);
                }

            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Parses the given <paramref name="arguments"/> and returns keyword as string.
        /// </summary>
        public static string GetKeywordFilter(string[] arguments)
        {
            if (Array.IndexOf(arguments, "-K") > -1)
            {
                try
                {
                    var userIdFilter = arguments[Array.IndexOf(arguments, "-K") + 1];
                    return userIdFilter;
                }
                catch (IndexOutOfRangeException exception)
                {
                    throw new ArgumentNullException("No keyword specified after -K", exception);
                }

            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Parses the given <paramref name="arguments"/> and returns blacklist file path as string.
        /// </summary>
        public static string GetBlacklistFilePath(string[] arguments)
        {
            if (Array.IndexOf(arguments, "-B") > -1)
            {
                try
                {
                    var blacklistFilePath = arguments[Array.IndexOf(arguments, "-B") + 1];
                    return blacklistFilePath;
                }
                catch (IndexOutOfRangeException exception)
                {
                    throw new ArgumentNullException("No keyword specified after -B", exception);
                }

            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Parses the given <paramref name="arguments"/> and returns obfuscation state as boolean.
        /// </summary>
        public static bool GetObfuscationState(string[] arguments)
        {
            if (Array.IndexOf(arguments, "-F") > -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
