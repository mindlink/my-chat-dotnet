namespace MindLink.Recruitment.MyChat
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a helper to parse command line arguments.
    /// </summary>
    public sealed class CommandLineParser : ICommandLineParser
    {
        private ConversationConfig config;

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
        /// <param name="arguments"></param>
        /// The command line arguments
        /// <returns></returns>
        public ConversationConfig ParseCommandLineArguments(string[] arguments)
        {
            try
            {
                config.InputFilePath = arguments[0];
                config.OutputFilePath = arguments[1];
            }
            catch (IndexOutOfRangeException)
            {
                throw new ArgumentException("Input and output file must be specified");
            }

            for (int i = 2; i < arguments.Length; i++)
            {
                switch (arguments[i])
                {
                    case "-uf":
                        IMessageFilter userFilter = new UserFilter(arguments[i + 1]);
                        config.Filters.Add(userFilter);
                        break;
                    case "-kf":
                        IMessageFilter keywordFilter = new KeywordFilter(arguments[i + 1]);
                        config.Filters.Add(keywordFilter);
                        break;
                    case "-kb":
                        string[] split = arguments[i + 1].Split(',');
                        List<string> blockedWordList = new List<string>();
                        foreach (string blockedWord in split)
                        {
                            blockedWordList.Add(blockedWord);
                        }
                        IMessageFilter blacklistFilter = new BlacklistFilter(blockedWordList.ToArray());
                        config.Filters.Add(blacklistFilter);
                        break;
                    case "-hcc":
                        IMessageFilter ccFilter = new CreditCardFilter();
                        config.Filters.Add(ccFilter);
                        break;
                    case "-hpn":
                        IMessageFilter pnFilter = new PhoneNumberFilter();
                        config.Filters.Add(pnFilter);
                        break;
                    case "-ou":
                        config.ObfuscateUserID = true;
                        break;
                }
            }
            return config;
        }
    }
}