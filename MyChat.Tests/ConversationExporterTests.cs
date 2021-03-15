using System.IO;
using System.Linq;
using MyChat;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MindLink.Recruitment.MyChat.Tests
{
    using System;

    /// <summary>
    /// Tests for the <see cref="ConversationExporter"/>.
    /// </summary>
    [TestFixture]
    public class ConversationExporterTests
    {
        // Helper functions to simplify tests by autogenerating exporter parameters

        private void ExportConversation(string InputFilePath, string OutputFilePath)
        {
            ExportConversation(InputFilePath, OutputFilePath, null, null, null, false);
        }

        private void ExportConversation(string inputFilePath, string outputFilePath, string filterByUser, string filterByKeyword, string[] blacklist, bool report)
        {
            var exporterParameters = new ConversationExporterParameters(inputFilePath, outputFilePath, filterByUser, filterByKeyword, blacklist, report);
            var exporter = new ConversationExporter();

            exporter.ExportConversation(exporterParameters);
        }

        // GENERAL TESTS

        /// <summary>
        /// Tests that exporting the conversation exports conversation.
        /// </summary>
        [Test]
        public void ExportingConversationExportsConversation()
        {
            ExportConversation("chat.txt", "chat.json");

            var serializedConversation = new StreamReader(new FileStream("chat.json", FileMode.Open)).ReadToEnd();

            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.That(savedConversation.name, Is.EqualTo("My Conversation"));

            var messages = savedConversation.messages.ToList();

            Assert.That(messages[0].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470901)));
            Assert.That(messages[0].senderId, Is.EqualTo("bob"));
            Assert.That(messages[0].content, Is.EqualTo("Hello there!"));

            Assert.That(messages[1].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470905)));
            Assert.That(messages[1].senderId, Is.EqualTo("mike"));
            Assert.That(messages[1].content, Is.EqualTo("how are you?"));

            Assert.That(messages[2].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470906)));
            Assert.That(messages[2].senderId, Is.EqualTo("bob"));
            Assert.That(messages[2].content, Is.EqualTo("I'm good thanks, do you like pie?"));

            Assert.That(messages[3].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470910)));
            Assert.That(messages[3].senderId, Is.EqualTo("mike"));
            Assert.That(messages[3].content, Is.EqualTo("no, let me ask Angus..."));

            Assert.That(messages[4].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470912)));
            Assert.That(messages[4].senderId, Is.EqualTo("angus"));
            Assert.That(messages[4].content, Is.EqualTo("Hell yes! Are we buying some pie?"));

            Assert.That(messages[5].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470914)));
            Assert.That(messages[5].senderId, Is.EqualTo("bob"));
            Assert.That(messages[5].content, Is.EqualTo("No, just want to know if there's anybody else in the pie society..."));

            Assert.That(messages[6].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470915)));
            Assert.That(messages[6].senderId, Is.EqualTo("angus"));
            Assert.That(messages[6].content, Is.EqualTo("YES! I'm the head pie eater there..."));
        }

        // MANAGEARGUMENTS TESTS

        /// <summary>
        /// Tests that providing no input file path produces an argument null error
        /// </summary>
        [Test]
        public void ExportingConversationWithNoInputFileThrowsArgumentNullError()
        {
            var exporterConfig = new ConversationExporterConfiguration();

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => ConversationExporter.ManageArguments(exporterConfig));

            Assert.AreEqual("Value cannot be null. (Parameter 'Input File Path')", exception.Message);
        }

        // READCONVERSATION TESTS

        /// <summary>
        /// Tests that a non-existent file path returns a Argument Exception
        /// </summary>
        [Test]
        public void ExportingNonExistentFileThrowsArgumentException()
        {
            ArgumentException exception = Assert.Throws<ArgumentException>(() => ExportConversation("null.txt", null));

            Assert.AreEqual("The file was not found.", exception.Message);
        }

        /// <summary>
        /// Tests that message conversion works with only 1 word.
        /// </summary>
        [Test]
        public void ExportingOneWordMessageExportsMessage()
        {
            ExportConversation("OneWord.txt", "OneWord.json");

            var serializedConversation = new StreamReader(new FileStream("OneWord.json", FileMode.Open)).ReadToEnd();

            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.That(savedConversation.name, Is.EqualTo("My Conversation"));

            var messages = savedConversation.messages.ToList();

            Assert.That(messages[0].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470901)));
            Assert.That(messages[0].senderId, Is.EqualTo("bob"));
            Assert.That(messages[0].content, Is.EqualTo("Hello!"));
        }

        /// <summary>
        /// Tests that any file which does not meet the correct format throws a FormatException.
        /// </summary>
        [Test]
        public void ExportingAnUnconvertableFileThrowsFormatException()
        {
            FormatException exception = Assert.Throws<FormatException>(() => ExportConversation("chatIncorrect.txt", null));

            Assert.AreEqual("The file could not be converted.", exception.Message);
        }

        /// <summary>
        /// Tests that exporting the filtering the conversation does not alter the report.
        /// </summary>
        [Test]
        public void FilteringConversationFiltersReport()
        {
            var exporter = new ConversationExporter();
            var exporterParameters = new ConversationExporterParameters("chat.txt", null, "bob", null, null, true);

            var conversation = exporter.ReadConversation(exporterParameters);
            var activity = conversation.activity.ToList<Activity>();

            Assert.That(activity[0].count, Is.EqualTo(3));
            Assert.That(activity[0].sender, Is.EqualTo("bob"));
        }

    }
}
