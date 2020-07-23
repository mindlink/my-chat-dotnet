namespace MindLink.Recruitment.MyChat.Tests
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Unit tests for the <see cref="ConversationWriter"/>
    /// </summary>
    [TestFixture]
    class ConversationWriterTests
    {
        private IConversationWriter writer;
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
            config = ParseCommandLineArguments(args);

            // Act / Assert
            Assert.That(() => writer.WriteConversation(conversation, @"test\chat.json"),
            Throws.Exception
              .TypeOf<DirectoryNotFoundException>(), "Argument exception thrown on valid directory");
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
            writer = new ConversationWriter();
            config = new ConversationConfig();

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