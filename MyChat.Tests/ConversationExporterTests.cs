using System.IO;
using System.Linq;
using MyChat;
using Newtonsoft.Json;

namespace MindLink.Recruitment.MyChat.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for the <see cref="ConversationExporter"/>.
    /// </summary>
    [TestClass]
    public class ConversationExporterTests
    {
        /// <summary>
        /// Tests that exporting the conversation exports conversation.
        /// </summary>
        [TestMethod]
        public void BasicExportTest()
        {
            var args = new string[] { "-I", "chat.txt", "-O", "chat_BasicExportTest.json" };
            ConversationExporter.ExportConversation(args);

            var serializedConversation = new StreamReader(new FileStream(@"..\..\chat_BasicExportTest.json", FileMode.Open)).ReadToEnd();
            Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.AreEqual("My Conversation", savedConversation.name);

            var messages = savedConversation.messages.ToList();

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470901), messages[0].timestamp);
            Assert.AreEqual("bob", messages[0].userId);
            Assert.AreEqual("Hello there!", messages[0].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470905), messages[1].timestamp);
            Assert.AreEqual("mike", messages[1].userId);
            Assert.AreEqual("how are you?", messages[1].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470906), messages[2].timestamp);
            Assert.AreEqual("bob", messages[2].userId);
            Assert.AreEqual("I'm good thanks, do you like pie?", messages[2].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470910), messages[3].timestamp);
            Assert.AreEqual("mike", messages[3].userId);
            Assert.AreEqual("no, let me ask Angus...", messages[3].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470912), messages[4].timestamp);
            Assert.AreEqual("angus", messages[4].userId);
            Assert.AreEqual("Hell yes! Are we buying some pie?", messages[4].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[5].timestamp);
            Assert.AreEqual("bob", messages[5].userId);
            Assert.AreEqual("No, just want to know if there's anybody else in the Pie Society...", messages[5].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470915), messages[6].timestamp);
            Assert.AreEqual("angus", messages[6].userId);
            Assert.AreEqual("YES! I'm the head pie eater there...", messages[6].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470917), messages[7].timestamp);
            Assert.AreEqual("bob", messages[7].userId);
            Assert.AreEqual("My credit card number is 4724860934565114 and my phone number is 07765 487576.", messages[7].content);
        }

        /// <summary>
        /// Tests that a blacklist text file is read and converted to a Blacklist / HashSet.
        /// </summary>
        [TestMethod]
        public void BlacklistReaderTest()
        {
            var inputFilePath = "blacklist.txt";
            var blacklist = BlacklistReader.TextToBlacklist(inputFilePath);
            Assert.IsTrue(blacklist.content.SequenceEqual(new HashSet<string>() {"pie", "angus"}));
        }

        /// <summary>
        /// Tests that blacklisted words 'pie' and 'Angus' are redacted, and that the blacklist does not affect sendedId by design.
        /// </summary>
        [TestMethod]
        public void BlacklistFilterTest()
        {
            var args = new string[] { "-I", "chat.txt", "-O", "chat_BlacklistTest.json", "-B", "blacklist.txt" };
            ConversationExporter.ExportConversation(args);

            var serializedConversation = new StreamReader(new FileStream(@"..\..\chat_BlacklistTest.json", FileMode.Open)).ReadToEnd();
            Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            var messages = savedConversation.messages.ToList();

            Assert.AreEqual("bob", messages[2].userId);
            Assert.AreEqual("I'm good thanks, do you like *redacted*", messages[2].content);

            Assert.AreEqual("mike", messages[3].userId);
            Assert.AreEqual("no, let me ask *redacted*", messages[3].content);

            Assert.AreEqual("angus", messages[4].userId);
            Assert.AreEqual("Hell yes! Are we buying some *redacted*", messages[4].content);

            Assert.AreEqual("bob", messages[5].userId);
            Assert.AreEqual("No, just want to know if there's anybody else in the *redacted* Society...", messages[5].content);

            Assert.AreEqual("angus", messages[6].userId);
            Assert.AreEqual("YES! I'm the head *redacted* eater there...", messages[6].content);

        }

        /// <summary>
        /// Tests that only messages with specified keyword 'pie' are exported.
        /// </summary>
        [TestMethod]
        public void KeywordFilterTest()
        {
            var args = new string[] { "-I", "chat.txt", "-O", "chat_KeywordFilterTest.json", "-K", "pie"};
            ConversationExporter.ExportConversation(args);

            var serializedConversation = new StreamReader(new FileStream(@"..\..\chat_KeywordFilterTest.json", FileMode.Open)).ReadToEnd();
            Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            var messages = savedConversation.messages.ToList();

            Assert.AreEqual("I'm good thanks, do you like pie?", messages[0].content);
            Assert.AreEqual("Hell yes! Are we buying some pie?", messages[1].content);
            Assert.AreEqual("No, just want to know if there's anybody else in the Pie Society...", messages[2].content);
            Assert.AreEqual("YES! I'm the head pie eater there...", messages[3].content);
        }

        /// <summary>
        /// Tests that only messages with sent by specified user 'bob' are exported.
        /// </summary>
        [TestMethod]
        public void UserIdFilterTest()
        {
            var args = new string[] { "-I", "chat.txt", "-O", "chat_UserIdFilterTest.json", "-U", "bob"};
            ConversationExporter.ExportConversation(args);

            var serializedConversation = new StreamReader(new FileStream(@"..\..\chat_UserIdFilterTest.json", FileMode.Open)).ReadToEnd();
            Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            var messages = savedConversation.messages.ToList();

            Assert.AreEqual("bob", messages[0].userId);
            Assert.AreEqual("bob", messages[1].userId);
            Assert.AreEqual("bob", messages[2].userId);
            Assert.AreEqual("bob", messages[3].userId);
        }

        /// <summary>
        /// Tests that user identities are obfuscated when -F given as argument.
        /// </summary>
        [TestMethod]
        public void ObfuscationTest()
        {
            var args = new string[] { "-I", "chat.txt", "-O", "chat_ObfuscationTest.json", "-F" };
            ConversationExporter.ExportConversation(args);

            var serializedConversation = new StreamReader(new FileStream(@"..\..\chat_ObfuscationTest.json", FileMode.Open)).ReadToEnd();
            Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            var messages = savedConversation.messages.ToList();

            Assert.AreEqual("User#0", messages[0].userId);
            Assert.AreEqual("User#1", messages[1].userId);
            Assert.AreEqual("User#0", messages[2].userId);
            Assert.AreEqual("User#1", messages[3].userId);
            Assert.AreEqual("User#2", messages[4].userId);
            Assert.AreEqual("User#0", messages[5].userId);
            Assert.AreEqual("User#2", messages[6].userId);
            Assert.AreEqual("User#0", messages[7].userId);
        }

        /// <summary>
        /// Tests that user message frequency is exported in descending order.
        /// </summary>
        [TestMethod]
        public void ActivityReportTest()
        {
            var args = new string[] { "-I", "chat.txt", "-O", "chat_ActivityReportTest.json" };
            ConversationExporter.ExportConversation(args);

            var serializedConversation = new StreamReader(new FileStream(@"..\..\chat_ActivityReportTest.json", FileMode.Open)).ReadToEnd();
            Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            var activityReport = new string[] { "UserId: bob, Messages sent = 4", "UserId: mike, Messages sent = 2", "UserId: angus, Messages sent = 2" };
            CollectionAssert.AreEqual(activityReport, savedConversation.activityReport);
        }
    }
}
