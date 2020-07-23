namespace MindLink.Recruitment.MyChat.Tests
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    using MindLink.Recruitment.MyChat.CommandLineParsing;
    using MindLink.Recruitment.MyChat.ConversationFilters;
    using MindLink.Recruitment.MyChat.ReportGeneration;
    using MindLink.Recruitment.MyChat.ConversationData;

    /// <summary>
    /// Unit tests for the <see cref="ReportGenerator"/>
    /// </summary>
    [TestFixture]
    class ReportGeneratorTests
    {
        private IReportGenerator reportGenerator;
        private ConversationConfig config;

        /// <summary>
        /// Tests that null reference exception is thrown on invalid conversation.
        /// </summary>
        [Test]
        public void MessagesNull()
        {
            // Arrange
            Reset();
            string[] args = new string[] { "chat.txt", "chat.json" };
            config = ParseCommandLineArguments(args);

            // Act / Assert
            Assert.That(() => reportGenerator.Generate(new Conversation()),
            Throws.Exception
              .TypeOf<ArgumentException>()
              .With.InnerException.TypeOf<NullReferenceException>(), "Null reference exception not thrown on invalid conversation.");
        }

        /// <summary>
        /// Generates a <see cref="ConversationConfig"/> object without depending on a <see cref="ICommandLineParser"/> implementation.
        /// </summary>
        /// <param name="arguments">
        /// The command line arguments to be parsed.
        /// </param>
        /// <returns></returns>
        private ConversationConfig ParseCommandLineArguments(string[] arguments)
        {
            config.InputFilePath = arguments[0];
            config.OutputFilePath = arguments[1];

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

        /// <summary>
        /// Helper method for preparing core components for use.
        /// </summary>
        private void Reset()
        {
            reportGenerator = new ReportGenerator();
            config = new ConversationConfig();
        }
    }
}