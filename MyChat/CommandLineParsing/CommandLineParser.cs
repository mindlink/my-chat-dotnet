namespace MindLink.Recruitment.MyChat.CommandLineParsing
{
    using System;
    using System.Collections.Generic;

    using MindLink.Recruitment.MyChat.ConversationFilters;

    /// <summary>
    /// Represents a helper to parse command line arguments.
    /// </summary>
    public sealed class CommandLineParser : ICommandLineParser
    {
        private ConversationConfig config;
        private IMessageFilter filter;

        /// <summary>
        /// Initialises a new instance of the <see cref="CommandLineParser"/> class.
        /// </summary>
        public CommandLineParser()
        {
            config = new ConversationConfig();
        }

        /// <summary>
        /// Returns a custom <see cref="ConversationConfig"/> object defined by command line arguments
        /// </summary>
        /// <param name="arguments">
        /// The command line arguments.
        /// </param>
        /// /// <exception cref="ArgumentException">
        /// Thrown if input and/or output files paths are not found.
        /// </exception>
        public ConversationConfig ParseCommandLineArguments(string[] arguments)
        {
            try
            {
                config.InputFilePath = arguments[0];
                config.OutputFilePath = arguments[1];
            }
            catch (IndexOutOfRangeException e)
            {
                throw new ArgumentException("Input and output file must be specified", e);
            }

            if (arguments.Length > 2)
                BuildFilters(arguments);

            return config;
        }

        /// <summary>
        /// Helper method for building and applying filters to the <see cref="ConversationConfig"/> object
        /// </summary>
        /// <param name="arguments"></param>
        private void BuildFilters(string[] arguments)
        {
            for (int i = 2; i < arguments.Length; i++)
            {
                switch (arguments[i])
                {
                    case "-uf":
                        filter = new UserFilter(arguments[i + 1]);
                        config.Filters.Add(filter);
                        break;
                    case "-kf":
                        filter = new KeywordFilter(arguments[i + 1]);
                        config.Filters.Add(filter);
                        break;
                    case "-kb":
                        string[] split = arguments[i + 1].Split(',');
                        List<string> blockedWordList = new List<string>();
                        foreach (string blockedWord in split)
                        {
                            blockedWordList.Add(blockedWord);
                        }
                        filter = new BlacklistFilter(blockedWordList.ToArray());
                        config.Filters.Add(filter);
                        break;
                    case "-hcc":
                        filter = new CreditCardFilter();
                        config.Filters.Add(filter);
                        break;
                    case "-hpn":
                        filter = new PhoneNumberFilter();
                        config.Filters.Add(filter);
                        break;
                    case "-ou":
                        config.ObfuscateUserID = true;
                        break;
                }
            }
        }
    }
}