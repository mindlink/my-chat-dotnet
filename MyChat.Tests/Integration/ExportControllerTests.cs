namespace MindLink.Recruitment.MyChat.Tests
{
    using Newtonsoft.Json;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using MindLink.Recruitment.MyChat.CommandLineParsing;
    using MindLink.Recruitment.MyChat.ConversationReaders;
    using MindLink.Recruitment.MyChat.ConversationWriters;
    using MindLink.Recruitment.MyChat.ConversationFilters;
    using MindLink.Recruitment.MyChat.ReportGeneration;
    using MindLink.Recruitment.MyChat.ConversationData;

    /// <summary>
    /// Integration tests using the <see cref="ex"/>
    /// </summary>
    [TestFixture]
    class ExportControllerTests
    {
        private ExportController controller;
        private IConversationReader reader;
        private IConversationWriter writer;
        private IConversationFilter filter;
        private ICommandLineParser cmdParser;
        private IReportGenerator reportGenerator;
        private string serializedConversation;

        /// <summary>
        /// Tests the export function with minimum arguments passed.
        /// </summary>
        [Test]
        public void MinimumArgsExport()
        {
            // Arrange
            Reset();
            controller = new ExportController(reader, writer, filter, cmdParser, reportGenerator);
            string[] args = new string[] { "chat.txt", "chat.json" };
            IList<Message> messages;
            Conversation conversation;

            // Act
            controller.Export(args);

            using (StreamReader stream = new StreamReader(new FileStream(args[1], FileMode.Open)))
            {
                serializedConversation = stream.ReadToEnd();
            }

            conversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);
            messages = conversation.Messages.ToList();

            // Assert
            Assert.That(conversation.Name, Is.EqualTo("My Conversation"));

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
        /// Tests the export function with maximum arguments passed.
        /// </summary>
        [Test]
        public void MaximumArgsExport()
        {
            // Arrange
            Reset();
            controller = new ExportController(reader, writer, filter, cmdParser, reportGenerator);
            string[] args = new string[] { "chat.txt", "chat.json", "-uf", "bob", "-kf", "pie", "-kb", "pie,yes", "-hcc", "-hpn", "-ou" };
            IList<Message> messages;
            Conversation conversation;

            // Act
            controller.Export(args);

            using (StreamReader stream = new StreamReader(new FileStream(args[1], FileMode.Open)))
            {
                serializedConversation = stream.ReadToEnd();
            }

            conversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);
            messages = conversation.Messages.ToList();

            // Assert
            Assert.That(conversation.Name, Is.EqualTo("My Conversation"));

            Assert.That(messages[0].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470906)));
            Assert.That(messages[0].SenderId, Is.EqualTo("1"));
            Assert.That(messages[0].Content, Is.EqualTo("I'm good thanks, do you like *redacted*"));

            Assert.That(messages[1].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470914)));
            Assert.That(messages[1].SenderId, Is.EqualTo("1"));
            Assert.That(messages[1].Content, Is.EqualTo("No, just want to know if there's anybody else in the *redacted* society..."));
        }

        /// <summary>
        /// Helper method for preparing core components for use.
        /// </summary>
        private void Reset()
        {
            reader = new ConversationReader();
            writer = new ConversationWriter();
            filter = new ConversationFilter();
            cmdParser = new CommandLineParser();
            reportGenerator = new ReportGenerator();
        }
    }
}