using System.IO;
using System.Linq;
using MyChat;
using Newtonsoft.Json;

namespace MindLink.Recruitment.MyChat.Tests
{
    using System;
    using System.Collections.Generic;
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
            // Create new ConversationExporter and export conversation in chat.txt to 
            // chat.json
            ConversationExporter exporter = new ConversationExporter();
            exporter.ExportConversation(new ConversationExporterConfiguration("chat.txt", "chat.json"));
            var serializedConversation = new StreamReader(new FileStream("chat.json", FileMode.Open)).ReadToEnd();
            Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            // Check that the name in chat.json is 'My Conversation' as expected
            Assert.AreEqual("My Conversation", savedConversation.name);

            var messages = savedConversation.messages.ToList();

            List<Message> expected_messages = new List<Message>
            {
                new Message(
                    DateTimeOffset.FromUnixTimeSeconds(1448470901),
                    "bob",
                    "Hello there!"
                    ),
                new Message(
                    DateTimeOffset.FromUnixTimeSeconds(1448470905),
                    "mike",
                    "how are you?"
                    ),
                new Message(
                    DateTimeOffset.FromUnixTimeSeconds(1448470906),
                    "bob",
                    "I'm good thanks, do you like pie?"
                    ),
                new Message(
                    DateTimeOffset.FromUnixTimeSeconds(1448470910),
                    "mike",
                    "no, let me ask Angus..."
                    ),
                new Message(
                    DateTimeOffset.FromUnixTimeSeconds(1448470912),
                    "angus",
                    "Hell yes! Are we buying some pie?"
                    ),
                new Message(
                    DateTimeOffset.FromUnixTimeSeconds(1448470914),
                    "bob",
                    "No, just want to know if there's anybody else in the pie society..."
                    ),
                new Message(
                    DateTimeOffset.FromUnixTimeSeconds(1448470915),
                    "angus",
                    "YES! I'm the head pie eater there..."
                    )
            };

            for (int i = 0; i < expected_messages.ToArray().Length; i++)
            {
                Assert.AreEqual(expected_messages[i].timestamp, messages[i].timestamp);
                Assert.AreEqual(expected_messages[i].senderId, messages[i].senderId);
                Assert.AreEqual(expected_messages[i].content, messages[i].content);
            }
        }

        /// <summary>
        /// Tests that an <see cref="ArgumentException">ArgumentException</see> is thrown if the input file path doesn't exist
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Allowed Invalid Input/Output File Argument")]
        public void InvalidInputPathArgumentsTest ()
        {
            ConversationExporter exporter = new ConversationExporter();
            exporter.ExportConversation(new ConversationExporterConfiguration("chatdoesntexist.txt", "chat.json"));
        }

        /// <summary>
        /// Tests whether an <see cref="ArgumentException">ArgumentException</see> is thrown when an invalid output path is provided.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Allowed Invalid Output File Path Argument")]
        public void InvalidOutputPathArgumentTest ()
        {
            ConversationExporter exporter = new ConversationExporter();
            exporter.ExportConversation(new ConversationExporterConfiguration("chat.txt", "<><><><>.json"));
        }
    }
}
