using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindLink.Recruitment.MyChat.Tests
{
    /// <summary>
    /// Tests for the <see cref="ConversationReader"/>
    /// </summary>
    [TestFixture]
    class ConversationReaderTests
    {
        private IConversationReader reader;
        private ICommandLineParser cmdParser;
        private ConversationConfig config;

        /// <summary>
        /// Tests that imported conversations match the source data.
        /// </summary>
        [Test]
        public void ReadConversationTest()
        {
            // Arrange
            Reset();
            string[] args = new string[] { "chat.txt", "chat.json" };
            IList<Message> messages;
            config = cmdParser.ParseCommandLineArguments(args);
            Conversation conversation;

            // Act
            conversation = reader.ReadConversation(config);
            messages = conversation.Messages.ToList();

            // Assert
            Assert.That(conversation.Name, Is.EqualTo("My Conversation"), "Conversation name incorrect");
            Assert.That(messages[0].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470901)));
            Assert.That(messages[0].SenderId, Is.EqualTo("bob"));
            Assert.That(messages[0].Content, Is.EqualTo("Hello there!"));

            Assert.That(messages[1].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470905)));
            Assert.That(messages[1].SenderId, Is.EqualTo("mike"));
            Assert.That(messages[1].Content, Is.EqualTo("how are you?"));

            Assert.That(messages[2].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470906)));
            Assert.That(messages[2].SenderId, Is.EqualTo("bob"));
            Assert.That(messages[2].Content, Is.EqualTo("I'm good thanks, do you like pie?"));

            Assert.That(messages[3].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470910)));
            Assert.That(messages[3].SenderId, Is.EqualTo("mike"));
            Assert.That(messages[3].Content, Is.EqualTo("no, let me ask Angus..."));

            Assert.That(messages[4].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470912)));
            Assert.That(messages[4].SenderId, Is.EqualTo("angus"));
            Assert.That(messages[4].Content, Is.EqualTo("Hell yes! Are we buying some pie?"));

            Assert.That(messages[5].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470914)));
            Assert.That(messages[5].SenderId, Is.EqualTo("bob"));
            Assert.That(messages[5].Content, Is.EqualTo("No, just want to know if there's anybody else in the pie society..."));

            Assert.That(messages[6].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470915)));
            Assert.That(messages[6].SenderId, Is.EqualTo("angus"));
            Assert.That(messages[6].Content, Is.EqualTo("YES! I'm the head pie eater there..."));
        }

        /// <summary>
        /// Tests that an exception is correctly thrown when an invalid file name is specified.
        /// </summary>
        [Test]
        public void FileNotFound()
        {
            // Arrange
            Reset();
            string[] args = new string[] { "chatt.txt", "chat.json" };
            config = cmdParser.ParseCommandLineArguments(args);

            // Act / Assert
            Assert.That(() => reader.ReadConversation(config),
            Throws.Exception
              .TypeOf<ArgumentException>(), "Argument exception thrown on valid file");
        }

        /// <summary>
        /// Tests that an exception is correctly thrown when an invalid file path is specified.
        /// </summary>
        [Test]
        public void DirectoryNotFound()
        {
            // Arrange
            Reset();
            string[] args = new string[] { @"test\chat.txt", "chat.json" };
            config = cmdParser.ParseCommandLineArguments(args);

            // Act / Assert
            Assert.That(() => reader.ReadConversation(config),
            Throws.Exception
              .TypeOf<ArgumentException>(), "Argument exception thrown on valid directory");
        }

        /// <summary>
        /// Helper method for preparing core components for use.
        /// </summary>
        private void Reset()
        {
            reader = new ConversationReader();
            cmdParser = new CommandLineParser();
        }
    }
}