namespace MindLink.Recruitment.MyChat.Tests
{
    using NUnit.Framework;
    using System;

    using MindLink.Recruitment.MyChat.CommandLineParsing;
    using MindLink.Recruitment.MyChat.ConversationFilters;

    /// <summary>
    /// Unit tests for the <see cref="CommandLineParser"/>
    /// </summary>
    [TestFixture]
    class CommandLineParserTests
    {
        /// <summary>
        /// Tests that input/output files are successfully parsed.
        /// </summary>
        [Test]
        public void MinimumArgsParsed()
        {
            // Arrange
            string[] args = new string[] { "chat.txt", "chat.json" };
            ICommandLineParser parser = new CommandLineParser();
            ConversationConfig config;

            // Act
            config = parser.ParseCommandLineArguments(args);

            // Assert
            Assert.That(config.InputFilePath, Is.EqualTo(args[0]), "Input file path incorrectly parsed");
            Assert.That(config.OutputFilePath, Is.EqualTo(args[1]), "Output file path incorrectly parsed");
        }

        /// <summary>
        /// Tests that all command line arguments are successfully parsed when used together.
        /// </summary>
        [Test]
        public void MaximumArgsParsed()
        {
            // Arrange
            string[] args = new string[] { "chat.txt", "chat.json", "-uf", "bob", "-kf", "pie", "-kb", "pie,yes", "-hcc", "-hpn", "-ou" };
            ICommandLineParser parser = new CommandLineParser();
            ConversationConfig config;
            bool ccFilter = false;
            bool pnFilter = false;

            // Act
            config = parser.ParseCommandLineArguments(args);

            // Assert
            Assert.That(config.InputFilePath, Is.EqualTo(args[0]), "Input file path incorrectly parsed");
            Assert.That(config.OutputFilePath, Is.EqualTo(args[1]), "Output file path incorrectly parsed");
            Assert.That(config.ObfuscateUserID, Is.EqualTo(true), "Obfuscate user ID filter incorrectly parsed");

            foreach (IMessageFilter filter in config.Filters)
            {
                switch (filter)
                {
                    case UserFilter uf:
                        Assert.That((filter as UserFilter).User, Is.EqualTo(args[3]), "User filter incorrectly parsed");
                        break;
                    case KeywordFilter kf:
                        Assert.That((filter as KeywordFilter).Keyword, Is.EqualTo(args[5]), "Keyword filter incorrectly parsed");
                        break;
                    case BlacklistFilter kb:
                        Assert.That((filter as BlacklistFilter).KeywordBlacklist, Is.EqualTo(new string[] { "pie", "yes" }), "Keyword blacklist incorrectly parsed");
                        break;
                    case CreditCardFilter hcc:
                        ccFilter = true;
                        break;
                    case PhoneNumberFilter hpn:
                        pnFilter = true;
                        break;
                }
            }

            Assert.That(ccFilter, Is.EqualTo(true), "Credit card filter incorrectly parsed");
            Assert.That(pnFilter, Is.EqualTo(true), "Phone number filter incorrectly parsed");
        }

        /// <summary>
        /// Tests that an argument exception is throw when too few arguments are passed to the conversation parser.
        /// </summary>
        [Test]
        public void NotEnoughArgs()
        {
            // Arrange
            string[] args = new string[] { "chat.txt" };
            ICommandLineParser parser = new CommandLineParser();

            // Act & Assert
            Assert.That(() => parser.ParseCommandLineArguments(args),
            Throws.Exception
              .TypeOf<ArgumentException>()
              .With.InnerException.TypeOf<IndexOutOfRangeException>(), "Argument exception not throw when too few arguments are passed to conversation parser");
        }
    }
}