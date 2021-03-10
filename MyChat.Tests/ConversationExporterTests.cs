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

        // GENERAL TESTS
        
        /// <summary>
        /// Tests that exporting the conversation exports conversation.
        /// </summary>
        [Test]
        public void ExportingConversationExportsConversation()
        {
            var exporter = new ConversationExporter();

            exporter.ExportConversation("chat.txt", "chat.json");

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
            exporterConfig.OutputFilePath = "chat.json";

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
            var exporter = new ConversationExporter();

            ArgumentException exception = Assert.Throws<ArgumentException>(() => exporter.ExportConversation("null.txt", "null.json"));

            Assert.AreEqual("The file was not found.", exception.Message);
        }

        /// <summary>
        /// Tests that message conversion works with only 1 word.
        /// </summary>
        [Test]
        public void ExportingOneWordMessageExportsMessage()
        {
            var exporter = new ConversationExporter();

            exporter.ExportConversation("OneWord.txt", "OneWord.json");

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
            var exporter = new ConversationExporter();

            FormatException exception = Assert.Throws<FormatException>(() => exporter.ExportConversation("chatIncorrect.txt", null));

            Assert.AreEqual("The file could not be converted.", exception.Message);
        }

        // FILTER BY USER TESTS

        /// <summary>
        /// Tests that supplying a user to filter by converts only messages from that user
        /// </summary>
        [Test]
        public void FilteringByUserFiltersByUser()
        {
            var exporter = new ConversationExporter();

            exporter.ExportConversation("chat.txt", "chatFilterBob.json", "bob", null, null, false);

            var serializedConversation = new StreamReader(new FileStream("chatFilterBob.json", FileMode.Open)).ReadToEnd();

            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.That(savedConversation.name, Is.EqualTo("My Conversation"));

            var messages = savedConversation.messages.ToList();

            Assert.That(messages[0].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470901)));
            Assert.That(messages[0].senderId, Is.EqualTo("bob"));
            Assert.That(messages[0].content, Is.EqualTo("Hello there!"));

            Assert.That(messages[1].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470906)));
            Assert.That(messages[1].senderId, Is.EqualTo("bob"));
            Assert.That(messages[1].content, Is.EqualTo("I'm good thanks, do you like pie?"));

            Assert.That(messages[2].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470914)));
            Assert.That(messages[2].senderId, Is.EqualTo("bob"));
            Assert.That(messages[2].content, Is.EqualTo("No, just want to know if there's anybody else in the pie society..."));
        }

        // FILTER BY KEYWORD TESTS

        /// <summary>
        /// Tests that supplying a user to filter by converts only messages from that user
        /// </summary>
        [Test]
        public void FilteringByKeywordFiltersByKeyword()
        {
            var exporter = new ConversationExporter();

            exporter.ExportConversation("chat.txt", "chatFilterPie.json", null, "pie" , null, false);

            var serializedConversation = new StreamReader(new FileStream("chatFilterPie.json", FileMode.Open)).ReadToEnd();

            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.That(savedConversation.name, Is.EqualTo("My Conversation"));

            var messages = savedConversation.messages.ToList();

            Assert.That(messages[0].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470906)));
            Assert.That(messages[0].senderId, Is.EqualTo("bob"));
            Assert.That(messages[0].content, Is.EqualTo("I'm good thanks, do you like pie?"));

            Assert.That(messages[1].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470912)));
            Assert.That(messages[1].senderId, Is.EqualTo("angus"));
            Assert.That(messages[1].content, Is.EqualTo("Hell yes! Are we buying some pie?"));

            Assert.That(messages[2].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470914)));
            Assert.That(messages[2].senderId, Is.EqualTo("bob"));
            Assert.That(messages[2].content, Is.EqualTo("No, just want to know if there's anybody else in the pie society..."));

            Assert.That(messages[3].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470915)));
            Assert.That(messages[3].senderId, Is.EqualTo("angus"));
            Assert.That(messages[3].content, Is.EqualTo("YES! I'm the head pie eater there..."));
        }

        // BLACKLIST TESTS

        /// <summary>
        /// Tests that blacklisting one word blacklists that word in the conversation, irrespective of case.
        /// </summary>
        [Test]
        public void BlacklistingOneWordRedactsWord()
        {
            var exporter = new ConversationExporter();

            exporter.ExportConversation("chat.txt", "chatBlYes.json", null, null, new string[] {"yes"}, false);

            var serializedConversation = new StreamReader(new FileStream("chatBlYes.json", FileMode.Open)).ReadToEnd();

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
            Assert.That(messages[4].content, Is.EqualTo("Hell \\*redacted\\*! Are we buying some pie?"));

            Assert.That(messages[5].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470914)));
            Assert.That(messages[5].senderId, Is.EqualTo("bob"));
            Assert.That(messages[5].content, Is.EqualTo("No, just want to know if there's anybody else in the pie society..."));

            Assert.That(messages[6].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470915)));
            Assert.That(messages[6].senderId, Is.EqualTo("angus"));
            Assert.That(messages[6].content, Is.EqualTo("\\*redacted\\*! I'm the head pie eater there..."));
        }

        /// <summary>
        /// Tests that blacklisting more than one word blacklists all words in the blacklist, irrespective of case.
        /// </summary>
        [Test]
        public void BlacklistingMultipleWordsRedactsAllWord()
        {
            var exporter = new ConversationExporter();

            exporter.ExportConversation("chat.txt", "chatBlPieYes.json", null, null, new string[] { "pie", "yes" }, false);

            var serializedConversation = new StreamReader(new FileStream("chatBlPieYes.json", FileMode.Open)).ReadToEnd();

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
            Assert.That(messages[2].content, Is.EqualTo("I'm good thanks, do you like \\*redacted\\*?"));

            Assert.That(messages[3].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470910)));
            Assert.That(messages[3].senderId, Is.EqualTo("mike"));
            Assert.That(messages[3].content, Is.EqualTo("no, let me ask Angus..."));

            Assert.That(messages[4].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470912)));
            Assert.That(messages[4].senderId, Is.EqualTo("angus"));
            Assert.That(messages[4].content, Is.EqualTo("Hell \\*redacted\\*! Are we buying some \\*redacted\\*?"));

            Assert.That(messages[5].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470914)));
            Assert.That(messages[5].senderId, Is.EqualTo("bob"));
            Assert.That(messages[5].content, Is.EqualTo("No, just want to know if there's anybody else in the \\*redacted\\* society..."));

            Assert.That(messages[6].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470915)));
            Assert.That(messages[6].senderId, Is.EqualTo("angus"));
            Assert.That(messages[6].content, Is.EqualTo("\\*redacted\\*! I'm the head \\*redacted\\* eater there..."));
        }

        // REPORT TESTS

        /// <summary>
        /// Tests that exporting the conversation with a report generates a report.
        /// </summary>
        [Test]
        public void ExportingConversationWithReportExportsConversationWithReport()
        {
            var exporter = new ConversationExporter();

            exporter.ExportConversation("chat.txt", "chatReport.json", null, null, null, true);

            var serializedConversation = new StreamReader(new FileStream("chatReport.json", FileMode.Open)).ReadToEnd();

            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.That(savedConversation.name, Is.EqualTo("My Conversation"));

            var activity = savedConversation.activity.ToList();

            Assert.That(activity[0].count, Is.EqualTo(3));
            Assert.That(activity[0].sender, Is.EqualTo("bob"));

            Assert.That(activity[1].count, Is.EqualTo(2));
            Assert.That(activity[1].sender, Is.EqualTo("mike"));

            Assert.That(activity[2].count, Is.EqualTo(2));
            Assert.That(activity[2].sender, Is.EqualTo("angus"));
        }

        // "INTEGRATION" TESTS

        /// <summary>
        /// Tests that filtering a word that has been blacklisted will still filter to messages that had that word in.
        /// </summary>
        [Test]
        public void FilteringByBlackwordFiltersBlackword()
        {
            var exporter = new ConversationExporter();

            exporter.ExportConversation("chat.txt", "chatBlFilterPie.json", null, "pie", new string[] { "pie" }, false);

            var serializedConversation = new StreamReader(new FileStream("chatBlfilterPie.json", FileMode.Open)).ReadToEnd();

            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.That(savedConversation.name, Is.EqualTo("My Conversation"));

            var messages = savedConversation.messages.ToList();

            Assert.That(messages[0].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470906)));
            Assert.That(messages[0].senderId, Is.EqualTo("bob"));
            Assert.That(messages[0].content, Is.EqualTo("I'm good thanks, do you like \\*redacted\\*?"));

            Assert.That(messages[1].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470912)));
            Assert.That(messages[1].senderId, Is.EqualTo("angus"));
            Assert.That(messages[1].content, Is.EqualTo("Hell yes! Are we buying some \\*redacted\\*?"));

            Assert.That(messages[2].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470914)));
            Assert.That(messages[2].senderId, Is.EqualTo("bob"));
            Assert.That(messages[2].content, Is.EqualTo("No, just want to know if there's anybody else in the \\*redacted\\* society..."));

            Assert.That(messages[3].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470915)));
            Assert.That(messages[3].senderId, Is.EqualTo("angus"));
            Assert.That(messages[3].content, Is.EqualTo("YES! I'm the head \\*redacted\\* eater there..."));
        }

        /// <summary>
        /// Tests that filtering by user and by keyword converts only messages from that user with that keyword in.
        /// </summary>
        [Test]
        public void FilteringByUserAndkeywordFiltersByUserAndKeyword()
        {
            var exporter = new ConversationExporter();

            exporter.ExportConversation("chat.txt", "chatFilterBobPie.json", "bob", "pie", null, false);

            var serializedConversation = new StreamReader(new FileStream("chatFilterBobPie.json", FileMode.Open)).ReadToEnd();

            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.That(savedConversation.name, Is.EqualTo("My Conversation"));

            var messages = savedConversation.messages.ToList();

            Assert.That(messages[0].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470906)));
            Assert.That(messages[0].senderId, Is.EqualTo("bob"));
            Assert.That(messages[0].content, Is.EqualTo("I'm good thanks, do you like pie?"));

            Assert.That(messages[1].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470914)));
            Assert.That(messages[1].senderId, Is.EqualTo("bob"));
            Assert.That(messages[1].content, Is.EqualTo("No, just want to know if there's anybody else in the pie society..."));
        }

        /// <summary>
        /// Tests that filtering by user and by a blacklisted word converts only messages from that user with that redacted keyword in.
        /// </summary>
        [Test]
        public void FilteringByUserAndBlackwordFiltersByUserAndBlackword()
        {
            var exporter = new ConversationExporter();

            exporter.ExportConversation("chat.txt", "chatBlFilterBobPie.json", "bob", "pie", new string[] { "pie" }, false);

            var serializedConversation = new StreamReader(new FileStream("chatBlFilterBobPie.json", FileMode.Open)).ReadToEnd();

            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.That(savedConversation.name, Is.EqualTo("My Conversation"));

            var messages = savedConversation.messages.ToList();

            Assert.That(messages[0].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470906)));
            Assert.That(messages[0].senderId, Is.EqualTo("bob"));
            Assert.That(messages[0].content, Is.EqualTo("I'm good thanks, do you like \\*redacted\\*?"));

            Assert.That(messages[1].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470914)));
            Assert.That(messages[1].senderId, Is.EqualTo("bob"));
            Assert.That(messages[1].content, Is.EqualTo("No, just want to know if there's anybody else in the \\*redacted\\* society..."));
        }

        /// <summary>
        /// Tests that exporting the filtering the conversation does not alter the report.
        /// </summary>
        [Test]
        public void FilteringConversationDoesNotFilterReport()
        {
            var exporter = new ConversationExporter();

            exporter.ExportConversation("chat.txt", "chatFilterReport.json", "bob", null, null, true);

            var serializedConversation = new StreamReader(new FileStream("chatFilterReport.json", FileMode.Open)).ReadToEnd();

            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.That(savedConversation.name, Is.EqualTo("My Conversation"));

            var messages = savedConversation.messages.ToList();

            Assert.That(messages[0].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470901)));
            Assert.That(messages[0].senderId, Is.EqualTo("bob"));
            Assert.That(messages[0].content, Is.EqualTo("Hello there!"));

            Assert.That(messages[1].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470906)));
            Assert.That(messages[1].senderId, Is.EqualTo("bob"));
            Assert.That(messages[1].content, Is.EqualTo("I'm good thanks, do you like pie?"));

            Assert.That(messages[2].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470914)));
            Assert.That(messages[2].senderId, Is.EqualTo("bob"));
            Assert.That(messages[2].content, Is.EqualTo("No, just want to know if there's anybody else in the pie society..."));

            var activity = savedConversation.activity.ToList();

            Assert.That(activity[0].count, Is.EqualTo(3));
            Assert.That(activity[0].sender, Is.EqualTo("bob"));

            Assert.That(activity[1].count, Is.EqualTo(2));
            Assert.That(activity[1].sender, Is.EqualTo("mike"));

            Assert.That(activity[2].count, Is.EqualTo(2));
            Assert.That(activity[2].sender, Is.EqualTo("angus"));
        }

        // WRITECONVERSATION TESTS
    }
}
