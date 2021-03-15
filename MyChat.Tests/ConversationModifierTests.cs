using System.IO;
using System.Linq;
using MyChat;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MindLink.Recruitment.MyChat.Tests
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Tests for the <see cref="ConversationExporter"/>.
    /// </summary>
    [TestFixture]
    public class ConversationModifierTests
    {
        // Pre-converted list of messages to save reading a file when testing.
        List<Message> messages = new List<Message>() {
                new Message(DateTimeOffset.FromUnixTimeSeconds(1448470901), "bob", "Hello there!"),
                new Message(DateTimeOffset.FromUnixTimeSeconds(1448470905), "mike", "how are you?"),
                new Message(DateTimeOffset.FromUnixTimeSeconds(1448470906), "bob", "I'm good thanks, do you like pie?"),
                new Message(DateTimeOffset.FromUnixTimeSeconds(1448470910), "mike", "no, let me ask Angus..."),
                new Message(DateTimeOffset.FromUnixTimeSeconds(1448470912), "angus", "Hell yes! Are we buying some pie?"),
                new Message(DateTimeOffset.FromUnixTimeSeconds(1448470914), "bob", "No, just want to know if there's anybody else in the pie society...")};

        // APPLYBLACKLIST TESTS

        /// <summary>
        /// Tests that applying a blacklist to a message redacts those words from the message.
        /// </summary>
        [Test]
        public void ApplyingBlacklistAppliesBlacklist()
        {
            var content = "Hell yes! Are we buying some pie ?";
            var blacklist = new string[] { "pie", "yes" };
            content = ConversationModifier.ApplyBlacklist(content, blacklist);

            Assert.AreEqual(content, "Hell \\*redacted\\*! Are we buying some \\*redacted\\* ?");
        }

        // ISINFILTERS TESTS

        /// <summary>
        /// Tests that IsInFilter returns true if the senderId matches the FilterByUser.
        /// </summary>
        [Test]
        public void MessageContainingFilteredUserReturnsTrue()
        {
            var timestamp = DateTimeOffset.FromUnixTimeSeconds(1448470912);
            var senderID = "angus";
            var content = "Hell yes! Are we buying some pie ?";
            var exporterParameters = new ConversationExporterParameters(null, null, "angus", null, null, false);

            Assert.IsTrue(ConversationModifier.IsInFilters(timestamp, senderID, content, exporterParameters));
        }


        /// <summary>
        /// Tests that IsInFilter returns false if the senderId doesn't match the FilterByUser.
        /// </summary>
        [Test]
        public void MessageNotContainingFilteredUserReturnsFalse()
        {
            var timestamp = DateTimeOffset.FromUnixTimeSeconds(1448470912);
            var senderID = "bob";
            var content = "Hell yes! Are we buying some pie ?";
            var exporterParameters = new ConversationExporterParameters(null, null, "angus", null, null, false);

            Assert.IsFalse(ConversationModifier.IsInFilters(timestamp, senderID, content, exporterParameters));
        }

        /// <summary>
        /// Tests that IsInFilter returns true if the content contains the FilterByKeyword.
        /// </summary>
        [Test]
        public void MessageContainingFilteredKeywordReturnsTrue()
        {
            var timestamp = DateTimeOffset.FromUnixTimeSeconds(1448470912);
            var senderID = "angus";
            var content = "Hell yes! Are we buying some pie ?";
            var exporterParameters = new ConversationExporterParameters(null, null, null, "pie", null, false);

            Assert.IsTrue(ConversationModifier.IsInFilters(timestamp, senderID, content, exporterParameters));
        }

        /// <summary>
        /// Tests that IsInFilter returns false if the content doesn't contain the FilterByKeyword.
        /// </summary>
        [Test]
        public void MessageNotContainingFilteredKeywordReturnsFalse()
        {
            var timestamp = DateTimeOffset.FromUnixTimeSeconds(1448470912);
            var senderID = "bob";
            var content = "Hell yes! Are we buying some pie ?";
            var exporterParameters = new ConversationExporterParameters(null, null, null, "null" , null, false);

            Assert.IsFalse(ConversationModifier.IsInFilters(timestamp, senderID, content, exporterParameters));
        }

        /// <summary>
        /// Tests that IsInFilter returns true if the senderId matches FilterByUser and the content contains FilterByKeyword.
        /// </summary>
        [Test]
        public void FilteringByUserAndKeywordFiltersByUserAndKeyword()
        {
            var exporterParameters = new ConversationExporterParameters(null, null, "bob", "Hello", null, false);

            Assert.IsTrue(ConversationModifier.IsInFilters(messages[0].timestamp, messages[0].senderId, messages[0].content, exporterParameters));

        }

        /// <summary>
        /// Tests that IsInFilter returns false if the senderId doesn't match FilterByUser or the content doesn't contain FilterByKeyword.
        /// </summary>
        [Test]
        public void MessageNotMatchingBothFiltersReturnsFalse()
        {
            var exporterParameters = new ConversationExporterParameters(null, null, "bob", "pie", null, false);

            Assert.IsFalse(ConversationModifier.IsInFilters(messages[0].timestamp, messages[0].senderId, messages[0].content, exporterParameters));
            Assert.IsFalse(ConversationModifier.IsInFilters(messages[4].timestamp, messages[4].senderId, messages[4].content, exporterParameters));
        }

        // GENERATEREPORT TESTS

        /// <summary>
        /// Tests that exporting the conversation with a report generates a report.
        /// </summary>
        [Test]
        public void GeneratingReportGeneratesReport()
        {
            var exporterParameters = new ConversationExporterParameters(null, null, null, null, null, true);

            var activity = ConversationModifier.GenerateReport(messages, exporterParameters);

            Assert.That(activity[0].count, Is.EqualTo(3));
            Assert.That(activity[0].sender, Is.EqualTo("bob"));

            Assert.That(activity[1].count, Is.EqualTo(2));
            Assert.That(activity[1].sender, Is.EqualTo("mike"));

            Assert.That(activity[2].count, Is.EqualTo(1));
            Assert.That(activity[2].sender, Is.EqualTo("angus"));
        }

        /// <summary>
        /// Tests that no report is generated when Report is set to false.
        /// </summary>
        [Test]
        public void GeneratingReportWithReportBeingFalseReturnsNull()
        {
            var exporterParameters = new ConversationExporterParameters(null, null, null, null, null, false);

            var activity = ConversationModifier.GenerateReport(messages, exporterParameters);

            Assert.IsNull(activity);
        }

        // APPLYMESSAGEMODIFIERS TESTS

        /// <summary>
        /// Tests that ApplyMessageModifiers checks filters and applies blacklist to messages.
        /// </summary>
        [Test]
        public void ApplyingMessageModifiersAppliesMessageModifiers()
        {
            var exporterParameters = new ConversationExporterParameters(null, null, null, "pie", new string[] { "pie" }, false);

            var messagesOutput = ConversationModifier.ApplyMessageModifiers(messages[2].timestamp, messages[2].senderId, messages[2].content, exporterParameters);

            Assert.That(messagesOutput.senderId, Is.EqualTo("bob"));
            Assert.That(messagesOutput.content, Is.EqualTo("I'm good thanks, do you like \\*redacted\\*?"));

            messagesOutput = ConversationModifier.ApplyMessageModifiers(messages[3].timestamp, messages[3].senderId, messages[3].content, exporterParameters);

            Assert.IsNull(messagesOutput);
        }

    }
}
