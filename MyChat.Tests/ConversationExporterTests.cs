using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindLink.MyChat.Filters;
using MindLink.MyChat.Transformers;
using Newtonsoft.Json;

namespace MindLink.MyChat.Tests
{
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
        public void ExportingConversationExportsConversation()
        {
            var exporter = new ConversationExporter(new ConversationExporterConfiguration("chat.txt", "chat.json"));
            exporter.ExportConversation();
            var serializedConversation = File.ReadAllText("chat.json");
            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.AreEqual("My Conversation", savedConversation.Name);

            var messages = savedConversation.Messages.ToList();

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470901), messages[0].Timestamp);
            Assert.AreEqual("bob", messages[0].SenderId);
            Assert.AreEqual("Hello there!", messages[0].Content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470905), messages[1].Timestamp);
            Assert.AreEqual("mike", messages[1].SenderId);
            Assert.AreEqual("how are you?", messages[1].Content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470906), messages[2].Timestamp);
            Assert.AreEqual("bob", messages[2].SenderId);
            Assert.AreEqual("I'm good thanks, do you like pie?", messages[2].Content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470910), messages[3].Timestamp);
            Assert.AreEqual("mike", messages[3].SenderId);
            Assert.AreEqual("no, let me ask Angus...", messages[3].Content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470912), messages[4].Timestamp);
            Assert.AreEqual("angus", messages[4].SenderId);
            Assert.AreEqual("Hell yes! Are we buying some pie?", messages[4].Content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[5].Timestamp);
            Assert.AreEqual("bob", messages[5].SenderId);
            Assert.AreEqual("No, just want to know if there's anybody else in the pie society...", messages[5].Content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470915), messages[6].Timestamp);
            Assert.AreEqual("angus", messages[6].SenderId);
            Assert.AreEqual("YES! I'm the head pie eater there...", messages[6].Content);
        }

        [TestMethod]
        public void EnsureKeywordFilterWorks()
        {
            var config = new ConversationExporterConfiguration("chat.txt", "chat.json");
            config.AddFilter(new KeywordFilter("pie"));

            var exporter = new ConversationExporter(config);
            exporter.ExportConversation();
            var serializedConversation = File.ReadAllText("chat.json");
            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.AreEqual("My Conversation", savedConversation.Name);

            var messages = savedConversation.Messages.ToList();

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470906), messages[0].Timestamp);
            Assert.AreEqual("bob", messages[0].SenderId);
            Assert.AreEqual("I'm good thanks, do you like pie?", messages[0].Content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470912), messages[1].Timestamp);
            Assert.AreEqual("angus", messages[1].SenderId);
            Assert.AreEqual("Hell yes! Are we buying some pie?", messages[1].Content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[2].Timestamp);
            Assert.AreEqual("bob", messages[2].SenderId);
            Assert.AreEqual("No, just want to know if there's anybody else in the pie society...", messages[2].Content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470915), messages[3].Timestamp);
            Assert.AreEqual("angus", messages[3].SenderId);
            Assert.AreEqual("YES! I'm the head pie eater there...", messages[3].Content);
        }

        [TestMethod]
        public void EnsureUserFilterWorks()
        {
            var config = new ConversationExporterConfiguration("chat.txt", "chat.json");
            config.AddFilter(new UserFilter("angus"));

            var exporter = new ConversationExporter(config);
            exporter.ExportConversation();
            var serializedConversation = File.ReadAllText("chat.json");
            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.AreEqual("My Conversation", savedConversation.Name);

            var messages = savedConversation.Messages.ToList();

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470912), messages[0].Timestamp);
            Assert.AreEqual("angus", messages[0].SenderId);
            Assert.AreEqual("Hell yes! Are we buying some pie?", messages[0].Content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470915), messages[1].Timestamp);
            Assert.AreEqual("angus", messages[1].SenderId);
            Assert.AreEqual("YES! I'm the head pie eater there...", messages[1].Content);
        }

        [TestMethod]
        public void EnsureBlacklistWorks()
        {
            var config = new ConversationExporterConfiguration("chat.txt", "chat.json");
            config.AddTransformer(new BlacklistTransformer(new []{ "pie", "yes"}));

            var exporter = new ConversationExporter(config);
            exporter.ExportConversation();
            var serializedConversation = File.ReadAllText("chat.json");
            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.AreEqual("My Conversation", savedConversation.Name);

            var messages = savedConversation.Messages.ToList();

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470901), messages[0].Timestamp);
            Assert.AreEqual("bob", messages[0].SenderId);
            Assert.AreEqual("Hello there!", messages[0].Content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470905), messages[1].Timestamp);
            Assert.AreEqual("mike", messages[1].SenderId);
            Assert.AreEqual("how are you?", messages[1].Content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470906), messages[2].Timestamp);
            Assert.AreEqual("bob", messages[2].SenderId);
            Assert.AreEqual("I'm good thanks, do you like *redacted*?", messages[2].Content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470910), messages[3].Timestamp);
            Assert.AreEqual("mike", messages[3].SenderId);
            Assert.AreEqual("no, let me ask Angus...", messages[3].Content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470912), messages[4].Timestamp);
            Assert.AreEqual("angus", messages[4].SenderId);
            Assert.AreEqual("Hell *redacted*! Are we buying some *redacted*?", messages[4].Content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[5].Timestamp);
            Assert.AreEqual("bob", messages[5].SenderId);
            Assert.AreEqual("No, just want to know if there's anybody else in the *redacted* society...", messages[5].Content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470915), messages[6].Timestamp);
            Assert.AreEqual("angus", messages[6].SenderId);
            Assert.AreEqual("*redacted*! I'm the head *redacted* eater there...", messages[6].Content);
        }

        [TestMethod]
        public void EnsureCreditcardsWorks()
        {
            var config = new ConversationExporterConfiguration("chat_with_creditcard.txt", "chat_with_creditcard.json");
            config.AddTransformer(new CreditCardTransformer());

            var exporter = new ConversationExporter(config);
            exporter.ExportConversation();
            var serializedConversation = File.ReadAllText("chat_with_creditcard.json");
            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.AreEqual("My Conversation", savedConversation.Name);

            var messages = savedConversation.Messages.ToList();

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470901), messages[0].Timestamp);
            Assert.AreEqual("bob", messages[0].SenderId);
            Assert.AreEqual("Hello there!", messages[0].Content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470905), messages[1].Timestamp);
            Assert.AreEqual("mike", messages[1].SenderId);
            Assert.AreEqual("how are you?", messages[1].Content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470906), messages[2].Timestamp);
            Assert.AreEqual("bob", messages[2].SenderId);
            Assert.AreEqual("I'm good thanks, do you like pie?", messages[2].Content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470910), messages[3].Timestamp);
            Assert.AreEqual("mike", messages[3].SenderId);
            Assert.AreEqual("no, let me ask Angus...", messages[3].Content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470912), messages[4].Timestamp);
            Assert.AreEqual("angus", messages[4].SenderId);
            Assert.AreEqual("Hell yes! Are we buying some pie? my credit card *redacted*", messages[4].Content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[5].Timestamp);
            Assert.AreEqual("bob", messages[5].SenderId);
            Assert.AreEqual("No, just want to know if there's anybody else in the pie society...", messages[5].Content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470915), messages[6].Timestamp);
            Assert.AreEqual("angus", messages[6].SenderId);
            Assert.AreEqual("YES! I'm the head pie eater there... my another card *redacted*", messages[6].Content);
        }

        [TestMethod]
        public void EnsurePhonesWorks()
        {
            var config = new ConversationExporterConfiguration("chat_with_phones.txt", "chat_with_phones.json");
            config.AddTransformer(new PhoneNumberTransformer());

            var exporter = new ConversationExporter(config);
            exporter.ExportConversation();
            var serializedConversation = File.ReadAllText("chat_with_phones.json");
            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.AreEqual("My Conversation", savedConversation.Name);

            var messages = savedConversation.Messages.ToList();

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470901), messages[0].Timestamp);
            Assert.AreEqual("bob", messages[0].SenderId);
            Assert.AreEqual("Hello there!", messages[0].Content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470905), messages[1].Timestamp);
            Assert.AreEqual("mike", messages[1].SenderId);
            Assert.AreEqual("how are you?", messages[1].Content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470906), messages[2].Timestamp);
            Assert.AreEqual("bob", messages[2].SenderId);
            Assert.AreEqual("*redacted* I'm good thanks, do you like pie?", messages[2].Content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470910), messages[3].Timestamp);
            Assert.AreEqual("mike", messages[3].SenderId);
            Assert.AreEqual("no, let me ask Angus...", messages[3].Content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470912), messages[4].Timestamp);
            Assert.AreEqual("angus", messages[4].SenderId);
            Assert.AreEqual("Hell yes! Are we buying some pie? my phone *redacted*", messages[4].Content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[5].Timestamp);
            Assert.AreEqual("bob", messages[5].SenderId);
            Assert.AreEqual("No, just want to know if there's anybody else in the pie society...", messages[5].Content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470915), messages[6].Timestamp);
            Assert.AreEqual("angus", messages[6].SenderId);
            Assert.AreEqual("YES! I'm the head pie eater there... my another phone *redacted*", messages[6].Content);
        }
    }
}
