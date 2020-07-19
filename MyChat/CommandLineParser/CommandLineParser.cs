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
                        config.UserFilter = arguments[i + 1];
                        break;
                    case "-kf":
                        config.KeywordFilter = arguments[i + 1];
                        break;
                    case "-kb":
                        string[] split = arguments[i + 1].Split(',');
                        List<string> blockedWordList = new List<string>();
                        foreach (string blockedWord in split)
                        {
                            blockedWordList.Add(blockedWord);
                        }
                        config.KeywordBlacklist = blockedWordList.ToArray();
                        break;
                    case "-hcc":
                        config.HideCreditCards = true;
                        break;
                    case "-hpn":
                        config.HidePhoneNumbers = true;
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