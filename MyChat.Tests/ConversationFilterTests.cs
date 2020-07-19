namespace MindLink.Recruitment.MyChat.Tests
{
    using NUnit.Framework;
    using System;
    using System.Linq;

    /// <summary>
    /// Tests for the <see cref="ConversationFilter"/>
    /// </summary>
    [TestFixture]
    class ConversationFilterTests
    {
        private IConversationReader reader;
        private IConversationFilter filter;
        private ICommandLineParser cmdParser;
        private ConversationConfig config;

        /// <summary>
        /// Tests that users are filtered correctly
        /// </summary>
        [Test]
        public void UserFilterTest()
        {
            // Arrange
            Reset();
            string[] args = new string[] { "chat.txt", "chat.json", "-uf", "bob" };
            config = cmdParser.ParseCommandLineArguments(args);
            Conversation conversation;

            // Act
            conversation = reader.ReadConversation(config);
            conversation = filter.FilterConversation(config, conversation);

            // Assert
            foreach (Message message in conversation.Messages.ToList())
            {
                Assert.That(message.SenderId, Is.EqualTo("bob"), "Users filtered incorrectly");
            }
        }

        /// <summary>
        /// Tests that keywords are filtered correctly
        /// </summary>
        [Test]
        public void KeywordFilterTest()
        {
            // Arrange
            Reset();
            string[] args = new string[] { "chat.txt", "chat.json", "-kf", "pie" };
            config = cmdParser.ParseCommandLineArguments(args);
            Conversation conversation;

            // Act
            conversation = reader.ReadConversation(config);
            conversation = filter.FilterConversation(config, conversation);

            // Assert
            foreach (Message message in conversation.Messages.ToList())
            {
                Assert.That(message.Content.Contains("pie"), Is.EqualTo(true), "Keyword filtered incorrectly");
            }
        }

        /// <summary>
        /// Tests that keyword blacklists are filtered correctly.
        /// </summary>
        [Test]
        public void KeywordBlacklistFilterTest()
        {
            // Arrange
            Reset();
            string[] args = new string[] { "chat.txt", "chat.json", "-kb", "pie" };
            config = cmdParser.ParseCommandLineArguments(args);
            Conversation conversation;

            // Act
            conversation = reader.ReadConversation(config);
            conversation = filter.FilterConversation(config, conversation);

            // Assert
            foreach (Message message in conversation.Messages.ToList())
            {
                Assert.That(message.Content.Contains("pie"), Is.EqualTo(false), "Keyword filtered incorrectly");
            }
        }

        /// <summary>
        /// Tests that credit cards are redacted.
        /// </summary>
        [Test]
        public void HideCreditCardNumberTest()
        {
            // Arrange
            Reset();
            string[] args = new string[] { "chat.txt", "chat.json", "-hcc" };
            config = cmdParser.ParseCommandLineArguments(args);
            Conversation conversation;

            // Act
            conversation = reader.ReadConversation(config);
            conversation = filter.FilterConversation(config, conversation);

            // Assert
            foreach (Message message in conversation.Messages.ToList())
            {
                Assert.That(message.Content.Contains("36667983174669"), Is.EqualTo(false), "Failed to redact credit card number");
            }
        }

        /// <summary>
        /// Tests that phone numbers are redacted.
        /// </summary>
        [Test]
        public void HidePhoneNumberTest()
        {
            // Arrange
            Reset();
            string[] args = new string[] { "chat.txt", "chat.json", "-hpn" };
            config = cmdParser.ParseCommandLineArguments(args);
            Conversation conversation;

            // Act
            conversation = reader.ReadConversation(config);
            conversation = filter.FilterConversation(config, conversation);

            // Assert
            foreach (Message message in conversation.Messages.ToList())
            {
                Assert.That(message.Content.Contains("08450847319"), Is.EqualTo(false), "Failed to redact credit card number");
            }
        }

        /// <summary>
        /// Tests that users are redacted.
        /// </summary>
        [Test]
        public void ObfuscateUsersTest()
        {
            // Arrange
            Reset();
            string[] args = new string[] { "chat.txt", "chat.json", "-ou" };
            config = cmdParser.ParseCommandLineArguments(args);
            Conversation conversation;

            // Act
            conversation = reader.ReadConversation(config);
            conversation = filter.FilterConversation(config, conversation);

            // Assert
            foreach (Message message in conversation.Messages.ToList())
            {
                Assert.That(message.SenderId.All(char.IsDigit), Is.EqualTo(true), "Failed to redact credit card number");
            }
        }

        /// <summary>
        /// Tests that argument null exception is thrown if conversation messages are null.
        /// </summary>
        [Test]
        public void MessagesNull()
        {
            // Arrange
            Reset();
            string[] args = new string[] { "chat.txt", "chat.json", "-ou" };
            config = cmdParser.ParseCommandLineArguments(args);

            // Act / Assert
            Assert.That(() => filter.FilterConversation(config, new Conversation()),
            Throws.Exception
              .TypeOf<ArgumentNullException>(), "Argument null exception not thrown on invalid conversation.");
        }

        /// <summary>
        /// Helper method for preparing core components for use.
        /// </summary>
        private void Reset()
        {
            reader = new ConversationReader();
            filter = new ConversationFilter();
            cmdParser = new CommandLineParser();
        }
    }
}