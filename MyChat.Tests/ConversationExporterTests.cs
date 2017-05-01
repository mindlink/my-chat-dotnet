using System.IO;
using System.Linq;
using System.Collections.Generic;
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

            ConversationExporterConfiguration configuration = new ConversationExporterConfiguration("chat.txt", "chat.json");

            exporter.ExportConversation(configuration);

            string serializedConversation;
            using (var reader = new StreamReader(new FileStream(configuration.outputFilePath, FileMode.Open)))
            {
                serializedConversation = reader.ReadToEnd();
            }

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
            Assert.AreEqual("No, just want to know if there's anybody else in the pie society...", messages[5].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470915), messages[6].timestamp);
            Assert.AreEqual("angus", messages[6].senderId);
            Assert.AreEqual("YES! I'm the head pie eater there...", messages[6].content);
        }

        [TestMethod]
        public void ExportingConversationExportsConversationWithFilters()
        {
            ConversationExporter exporter = new ConversationExporter();

            var blacklist = new List<string>();
            blacklist.Add("the");
            blacklist.Add("good");

            ConversationExporterConfiguration configuration = new ConversationExporterConfiguration("chat.txt", "chat.json", "bob", "pie", blacklist);

            exporter.ExportConversation(configuration);

            string serializedConversation;
            using (var reader = new StreamReader(new FileStream(configuration.outputFilePath, FileMode.Open)))
            {
                serializedConversation = reader.ReadToEnd();
            }

            Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.AreEqual("My Conversation", savedConversation.name);

            var messages = savedConversation.messages.ToList();

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470906), messages[0].timestamp);
            Assert.AreEqual("bob", messages[0].senderId);
            Assert.AreEqual("I'm *redacted* thanks, do you like pie?", messages[0].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[1].timestamp);
            Assert.AreEqual("bob", messages[1].senderId);
            Assert.AreEqual("No, just want to know if there's anybody else in *redacted* pie society...", messages[1].content);
        }

        [TestMethod]
        public void ParseCommandLineArgumentsParsesCommandLineArguments()
        {
            string[] args = { "chat.txt", "chat.json", "-u", "bob", "-k", "pie", "-b", "the", "good" };
            ConversationExporterConfiguration configuration = new CommandLineArgumentParser().ParseCommandLineArguments(args);

            Assert.AreEqual(configuration.inputFilePath, "chat.txt");
            Assert.AreEqual(configuration.outputFilePath, "chat.json");
            Assert.AreEqual(configuration.user, "bob");
            Assert.AreEqual(configuration.keyword, "pie");
            Assert.AreEqual(configuration.blacklist[0], "the");
            Assert.AreEqual(configuration.blacklist[1], "good");
        }
    }
}
