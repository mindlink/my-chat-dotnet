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
        public void ExportingConversationExportsConversation()
        {
            ConversationExporter exporter = new ConversationExporter();

            exporter.ExportConversation("chat.txt", "chat.json");

            var serializedConversation = new StreamReader(new FileStream("chat.json", FileMode.Open)).ReadToEnd();

            Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

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
    }
}
