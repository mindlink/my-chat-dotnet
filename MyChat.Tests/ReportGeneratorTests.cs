namespace MindLink.Recruitment.MyChat.Tests
{
    using NUnit.Framework;
    using System;

    /// <summary>
    /// Tests for the <see cref="ReportGenerator"/>
    /// </summary>
    [TestFixture]
    class ReportGeneratorTests
    {
        private IConversationReader reader;
        private IConversationFilter filter;
        private IReportGenerator reportGenerator;
        private ICommandLineParser cmdParser;
        private ConversationConfig config;

        /// <summary>
        /// Tests that null reference exception is thrown on invalid conversation.
        /// </summary>
        [Test]
        public void MessagesNull()
        {
            // Arrange
            Reset();
            string[] args = new string[] { "chat.txt", "chat.json", "-ou" };
            config = cmdParser.ParseCommandLineArguments(args);
            Conversation conversation;
            conversation = reader.ReadConversation(config);
            conversation = filter.FilterConversation(config, conversation);

            // Act / Assert
            Assert.That(() => reportGenerator.Generate(new Conversation()),
            Throws.Exception
              .TypeOf<NullReferenceException>(), "Null reference exception not thrown on invalid conversation.");
        }

        /// <summary>
        /// Helper method for preparing core components for use.
        /// </summary>
        private void Reset()
        {
            reader = new ConversationReader();
            filter = new ConversationFilter();
            reportGenerator = new ReportGenerator();
            cmdParser = new CommandLineParser();
        }
    }
}