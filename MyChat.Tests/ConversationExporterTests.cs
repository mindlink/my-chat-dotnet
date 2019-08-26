using System.IO;
using System.Linq;
using MyChat;
using Newtonsoft.Json;

namespace MindLink.Recruitment.MyChat.Tests
{
    using System;
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
            var arguments = new string[] {"-I", "chat.txt", "-O", "chat_BasicExportTest.json"};
            ConversationExporter exporter = new ConversationExporter(new CommandLineArgumentParser(arguments));

            exporter.ExportConversation();

            var serializedConversation = new StreamReader(new FileStream("chat_BasicExportTest.json", FileMode.Open)).ReadToEnd();

            Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.AreEqual("My Conversation", savedConversation.name);

            var messages = savedConversation.messages.ToList();

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470901), messages[0].timestamp);
            Assert.AreEqual("bob", messages[0].senderId);
            Assert.AreEqual("Hello there!", messages[0].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470905), messages[1].timestamp);
            Assert.AreEqual("mike", messages[1].senderId);
            Assert.AreEqual("how are you?", messages[1].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470906), messages[2].timestamp);
            Assert.AreEqual("bob", messages[2].senderId);
            Assert.AreEqual("I'm good thanks, do you like pie?", messages[2].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470910), messages[3].timestamp);
            Assert.AreEqual("mike", messages[3].senderId);
            Assert.AreEqual("no, let me ask Angus...", messages[3].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470912), messages[4].timestamp);
            Assert.AreEqual("angus", messages[4].senderId);
            Assert.AreEqual("Hell yes! Are we buying some pie?", messages[4].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[5].timestamp);
            Assert.AreEqual("bob", messages[5].senderId);
            Assert.AreEqual("No, just want to know if there's anybody else in the Pie Society...", messages[5].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470915), messages[6].timestamp);
            Assert.AreEqual("angus", messages[6].senderId);
            Assert.AreEqual("YES! I'm the head pie eater there...", messages[6].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470917), messages[7].timestamp);
            Assert.AreEqual("bob", messages[7].senderId);
            Assert.AreEqual("My credit card number is 4724860934565114 and my phone number is 07765 487576.", messages[7].content);
        }

        /// <summary>
        /// Tests that blacklisted words 'pie' and 'Angus' are redacted, and that the blacklist does not affect sendedId by design.
        /// </summary>
        [TestMethod]
        public void BlacklistFilterTest()
        {
            var arguments = new string[] { "-I", "chat.txt", "-O", "chat_BlacklistTest.json", "-B", "blacklist.txt" };
            ConversationExporter exporter = new ConversationExporter(new CommandLineArgumentParser(arguments));

            exporter.ExportConversation();

            var serializedConversation = new StreamReader(new FileStream("chat_BlacklistTest.json", FileMode.Open)).ReadToEnd();

            Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.AreEqual("My Conversation", savedConversation.name);

            var messages = savedConversation.messages.ToList();

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470901), messages[0].timestamp);
            Assert.AreEqual("bob", messages[0].senderId);
            Assert.AreEqual("Hello there!", messages[0].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470905), messages[1].timestamp);
            Assert.AreEqual("mike", messages[1].senderId);
            Assert.AreEqual("how are you?", messages[1].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470906), messages[2].timestamp);
            Assert.AreEqual("bob", messages[2].senderId);
            Assert.AreEqual("I'm good thanks, do you like *redacted*", messages[2].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470910), messages[3].timestamp);
            Assert.AreEqual("mike", messages[3].senderId);
            Assert.AreEqual("no, let me ask *redacted*", messages[3].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470912), messages[4].timestamp);
            Assert.AreEqual("angus", messages[4].senderId);
            Assert.AreEqual("Hell yes! Are we buying some *redacted*", messages[4].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[5].timestamp);
            Assert.AreEqual("bob", messages[5].senderId);
            Assert.AreEqual("No, just want to know if there's anybody else in the *redacted* Society...", messages[5].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470915), messages[6].timestamp);
            Assert.AreEqual("angus", messages[6].senderId);
            Assert.AreEqual("YES! I'm the head *redacted* eater there...", messages[6].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470917), messages[7].timestamp);
            Assert.AreEqual("bob", messages[7].senderId);
            Assert.AreEqual("My credit card number is 4724860934565114 and my phone number is 07765 487576.", messages[7].content);
        }

        /// <summary>
        /// Tests that only messages with specified keyword 'pie' are exported.
        /// </summary>
        [TestMethod]
        public void KeywordFilterTest()
        {
            var arguments = new string[] { "-I", "chat.txt", "-O", "chat_KeywordFilterTest.json", "-K", "pie" };
            ConversationExporter exporter = new ConversationExporter(new CommandLineArgumentParser(arguments));

            exporter.ExportConversation();

            var serializedConversation = new StreamReader(new FileStream("chat_KeywordFilterTest.json", FileMode.Open)).ReadToEnd();

            Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.AreEqual("My Conversation", savedConversation.name);

            var messages = savedConversation.messages.ToList();

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470906), messages[0].timestamp);
            Assert.AreEqual("bob", messages[0].senderId);
            Assert.AreEqual("I'm good thanks, do you like pie?", messages[0].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470912), messages[1].timestamp);
            Assert.AreEqual("angus", messages[1].senderId);
            Assert.AreEqual("Hell yes! Are we buying some pie?", messages[1].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[2].timestamp);
            Assert.AreEqual("bob", messages[2].senderId);
            Assert.AreEqual("No, just want to know if there's anybody else in the Pie Society...", messages[2].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470915), messages[3].timestamp);
            Assert.AreEqual("angus", messages[3].senderId);
            Assert.AreEqual("YES! I'm the head pie eater there...", messages[3].content);

            var activityReport = new string[] { "UserId: bob, Messages sent = 2", "UserId: angus, Messages sent = 2" };
            CollectionAssert.AreEqual(activityReport, savedConversation.activityReport);
        }

        /// <summary>
        /// Tests that only messages with sent by specified user 'bob' are exported.
        /// </summary>
        [TestMethod]
        public void UserIdFilterTest()
        {
            var arguments = new string[] { "-I", "chat.txt", "-O", "chat_UserIdFilterTest.json", "-U", "bob" };
            ConversationExporter exporter = new ConversationExporter(new CommandLineArgumentParser(arguments));

            exporter.ExportConversation();

            var serializedConversation = new StreamReader(new FileStream("chat_UserIdFilterTest.json", FileMode.Open)).ReadToEnd();

            Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.AreEqual("My Conversation", savedConversation.name);

            var messages = savedConversation.messages.ToList();

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470901), messages[0].timestamp);
            Assert.AreEqual("bob", messages[0].senderId);
            Assert.AreEqual("Hello there!", messages[0].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470906), messages[1].timestamp);
            Assert.AreEqual("bob", messages[1].senderId);
            Assert.AreEqual("I'm good thanks, do you like pie?", messages[1].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[2].timestamp);
            Assert.AreEqual("bob", messages[2].senderId);
            Assert.AreEqual("No, just want to know if there's anybody else in the Pie Society...", messages[2].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470917), messages[3].timestamp);
            Assert.AreEqual("bob", messages[3].senderId);
            Assert.AreEqual("My credit card number is 4724860934565114 and my phone number is 07765 487576.", messages[3].content);

            var activityReport = new string[] { "UserId: bob, Messages sent = 4" };
            CollectionAssert.AreEqual(activityReport, savedConversation.activityReport);
        }

        /// <summary>
        /// Tests that credit card and phone numbers are redacted when -N given as argument.
        /// </summary>
        [TestMethod]
        public void NumberFilterTest()
        {
            var arguments = new string[] { "-I", "chat.txt", "-O", "chat_NumberFilterTest.json", "-N" };
            ConversationExporter exporter = new ConversationExporter(new CommandLineArgumentParser(arguments));

            exporter.ExportConversation();

            var serializedConversation = new StreamReader(new FileStream("chat_NumberFilterTest.json", FileMode.Open)).ReadToEnd();

            Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.AreEqual("My Conversation", savedConversation.name);

            var messages = savedConversation.messages.ToList();

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470901), messages[0].timestamp);
            Assert.AreEqual("bob", messages[0].senderId);
            Assert.AreEqual("Hello there!", messages[0].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470905), messages[1].timestamp);
            Assert.AreEqual("mike", messages[1].senderId);
            Assert.AreEqual("how are you?", messages[1].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470906), messages[2].timestamp);
            Assert.AreEqual("bob", messages[2].senderId);
            Assert.AreEqual("I'm good thanks, do you like pie?", messages[2].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470910), messages[3].timestamp);
            Assert.AreEqual("mike", messages[3].senderId);
            Assert.AreEqual("no, let me ask Angus...", messages[3].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470912), messages[4].timestamp);
            Assert.AreEqual("angus", messages[4].senderId);
            Assert.AreEqual("Hell yes! Are we buying some pie?", messages[4].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[5].timestamp);
            Assert.AreEqual("bob", messages[5].senderId);
            Assert.AreEqual("No, just want to know if there's anybody else in the Pie Society...", messages[5].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470915), messages[6].timestamp);
            Assert.AreEqual("angus", messages[6].senderId);
            Assert.AreEqual("YES! I'm the head pie eater there...", messages[6].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470917), messages[7].timestamp);
            Assert.AreEqual("bob", messages[7].senderId);
            Assert.AreEqual("My credit card number is *redacted* and my phone number is *redacted*", messages[7].content);
        }

        /// <summary>
        /// Tests that user identities are obfuscated when -F given as argument.
        /// </summary>
        [TestMethod]
        public void ObfuscationTest()
        {
            var arguments = new string[] { "-I", "chat.txt", "-O", "chat_ObfuscationTest.json", "-F"};
            ConversationExporter exporter = new ConversationExporter(new CommandLineArgumentParser(arguments));

            exporter.ExportConversation();

            var serializedConversation = new StreamReader(new FileStream("chat_ObfuscationTest.json", FileMode.Open)).ReadToEnd();

            Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.AreEqual("My Conversation", savedConversation.name);

            var messages = savedConversation.messages.ToList();

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470901), messages[0].timestamp);
            Assert.AreEqual("User#0", messages[0].senderId);
            Assert.AreEqual("Hello there!", messages[0].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470905), messages[1].timestamp);
            Assert.AreEqual("User#1", messages[1].senderId);
            Assert.AreEqual("how are you?", messages[1].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470906), messages[2].timestamp);
            Assert.AreEqual("User#0", messages[2].senderId);
            Assert.AreEqual("I'm good thanks, do you like pie?", messages[2].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470910), messages[3].timestamp);
            Assert.AreEqual("User#1", messages[3].senderId);
            Assert.AreEqual("no, let me ask Angus...", messages[3].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470912), messages[4].timestamp);
            Assert.AreEqual("User#2", messages[4].senderId);
            Assert.AreEqual("Hell yes! Are we buying some pie?", messages[4].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[5].timestamp);
            Assert.AreEqual("User#0", messages[5].senderId);
            Assert.AreEqual("No, just want to know if there's anybody else in the Pie Society...", messages[5].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470915), messages[6].timestamp);
            Assert.AreEqual("User#2", messages[6].senderId);
            Assert.AreEqual("YES! I'm the head pie eater there...", messages[6].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470917), messages[7].timestamp);
            Assert.AreEqual("User#0", messages[7].senderId);
            Assert.AreEqual("My credit card number is 4724860934565114 and my phone number is 07765 487576.", messages[7].content);

            var activityReport = new string[] { "UserId: User#0, Messages sent = 4", "UserId: User#1, Messages sent = 2", "UserId: User#2, Messages sent = 2" };
            CollectionAssert.AreEqual(activityReport, savedConversation.activityReport);
        }

        /// <summary>
        /// Tests that user message frequency is exported in descending order.
        /// </summary>
        [TestMethod]
        public void ActivityReportTest()
        {
            var arguments = new string[] { "-I", "chat.txt", "-O", "chat_ActivityReportTest.json" };
            ConversationExporter exporter = new ConversationExporter(new CommandLineArgumentParser(arguments));

            exporter.ExportConversation();

            var serializedConversation = new StreamReader(new FileStream("chat_ActivityReportTest.json", FileMode.Open)).ReadToEnd();

            Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            var activityReport = new string[] { "UserId: bob, Messages sent = 4", "UserId: mike, Messages sent = 2", "UserId: angus, Messages sent = 2" };
            CollectionAssert.AreEqual(activityReport, savedConversation.activityReport);
        }
    }
}
