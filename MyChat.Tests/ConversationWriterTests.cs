namespace MindLink.Recruitment.MyChat.Tests
{
    using NUnit.Framework;
    using System;
    using System.IO;

    /// <summary>
    /// Tests for the <see cref="ConversationWriter"/>
    /// </summary>
    [TestFixture]
    class ConversationWriterTests
    {
        private IConversationWriter writer;
        private ICommandLineParser cmdParser;
        private ConversationConfig config;
        private Conversation conversation;

        /// <summary>
        /// Tests that an exception is correctly thrown when an invalid file path is specified.
        /// </summary>
        [Test]
        public void DirectoryNotFound()
        {
            // Arrange
            Reset();
            string[] args = new string[] { "chat.txt", "chat.json" };
            config = cmdParser.ParseCommandLineArguments(args);

            // Act / Assert
            Assert.That(() => writer.WriteConversation(conversation, @"test\chat.json"),
            Throws.Exception
              .TypeOf<DirectoryNotFoundException>(), "Argument exception thrown on valid directory");
        }

        /// <summary>
        /// Helper method for preparing core components for use.
        /// </summary>
        private void Reset()
        {
            writer = new ConversationWriter();
            cmdParser = new CommandLineParser();

            Message[] messages = new Message[8] 
            {
                new Message 
                {
                    Timestamp = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470901")),
                    SenderId = "bob",
                    Content = "Hello there!"                                    
                },
                new Message
                {
                    Timestamp = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470905")),
                    SenderId = "mike",
                    Content = "how are you?"
                },
                new Message
                {
                    Timestamp = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470906")),
                    SenderId = "bob",
                    Content = "I'm good thanks, do you like pie?"
                },
                new Message
                {
                    Timestamp = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470910")),
                    SenderId = "mike",
                    Content = "no, let me ask Angus..."
                },
                new Message
                {
                    Timestamp = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470912")),
                    SenderId = "angus",
                    Content = "Hell yes! Are we buying some pie?"
                },
                new Message
                {
                    Timestamp = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470914")),
                    SenderId = "bob",
                    Content = "No, just want to know if there's anybody else in the pie society..."
                },
                new Message
                {
                    Timestamp = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470915")),
                    SenderId = "angus",
                    Content = "YES! I'm the head pie eater there..."
                },
                new Message
                {
                    Timestamp = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470915")),
                    SenderId = "angus",
                    Content = "Call me on 08450847319, my cc is 36667983174669."
                }
            };

            conversation = new Conversation
            {
                Name = "My Conversation",
                Messages = messages
            };
        }
    }
}