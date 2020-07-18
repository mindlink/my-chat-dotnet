using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using MindLink.Recruitment.MyChat;

namespace MindLink.Recruitment.MyChat.Tests
{
    /// <summary>
    /// Tests for the <see cref="CommandLineParser"/>
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

            // Act
            config = parser.ParseCommandLineArguments(args);

            // Assert
            Assert.That(config.InputFilePath, Is.EqualTo(args[0]), "Input file path incorrectly parsed");
            Assert.That(config.OutputFilePath, Is.EqualTo(args[1]), "Output file path incorrectly parsed");
            Assert.That(config.UserFilter, Is.EqualTo(args[3]), "User filter incorrectly parsed");
            Assert.That(config.KeywordFilter, Is.EqualTo(args[5]), "Keyword filter incorrectly parsed");
            Assert.That(config.KeywordBlacklist, Is.EqualTo(new string[] { "pie", "yes" }), "Keyword blacklist incorrectly parsed");
            Assert.That(config.HideCreditCards, Is.EqualTo(true), "Credit card filter incorrectly parsed");
            Assert.That(config.HidePhoneNumbers, Is.EqualTo(true), "Phone number filter incorrectly parsed");
            Assert.That(config.ObfuscateUserID, Is.EqualTo(true), "Obfuscate user ID filter incorrectly parsed");
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
              .TypeOf<ArgumentException>(), "Argument exception not throw when too few arguments are passed to conversation parser");
        }
    }
}