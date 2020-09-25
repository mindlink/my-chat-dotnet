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

        /// <summary>
        /// Tests that exporting the conversation exports conversation.
        /// </summary>
       /* [Test]
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
        }*/

        [Test]
        public void GeneralTest()
        {
            var exporter = new ConversationExporter();

            exporter.ExportConversation("chat.txt", "chat.json");

            var serializedConversation = new StreamReader(new FileStream("chat.json", FileMode.Open)).ReadToEnd();

            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.That(savedConversation.name, Is.EqualTo("My Conversation"));

        }


        [Test]
        public void NameFilterTest()
        {
            // instance

            var exporter = new ConversationExporter();

            exporter.ExportConversation("chat.txt", "chat.json");

            var serializedConversation = new StreamReader(new FileStream("chat.json", FileMode.Open)).ReadToEnd();

            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            var messages = savedConversation.messages.ToList();

            TextFilter filter = new TextFilter();
            string user = "bob";

            //actual

            var actual1 = filter.NameFilter(messages, user);
            var actualString1 = actual1[1].senderId;

            //assert(actual == expected)

            Assert.That(actualString1, Is.EqualTo("bob"));


        }

        [Test]
        public void KeywordFilterTest()
        {
            var exporter = new ConversationExporter();

            exporter.ExportConversation("chat.txt", "chat.json");

            var serializedConversation = new StreamReader(new FileStream("chat.json", FileMode.Open)).ReadToEnd();

            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            var messages = savedConversation.messages.ToList();

            TextFilter filter = new TextFilter();
            string keyword = "angus";

            var actual1 = filter.KeywordFilter(messages, keyword);
            string actualString1 = actual1[0].content;
            Assert.That(actualString1, Is.EqualTo("no, let me ask Angus..."));

        }

        [Test]
        public void RedactedFilterTest()
        {
            var exporter = new ConversationExporter();

            exporter.ExportConversation("chat.txt", "chat.json");

            var serializedConversation = new StreamReader(new FileStream("chat.json", FileMode.Open)).ReadToEnd();

            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            var messages = savedConversation.messages.ToList();

            TextFilter filter = new TextFilter();
            string redacted = "*Redacted*";

            var actual1 = filter.RedactedWordFilter(messages, redacted);
            string actualString1 = actual1[2].content;

            var actual2 = filter.RedactedWordFilter(messages, redacted);
            string actualString2 = actual2[4].content;

            Assert.That(actualString1, Is.EqualTo("I'm good thanks, do you like *Redacted*?"));
            Assert.That(actualString2, Is.EqualTo("Hell yes! Are we buying some *Redacted*?"));
        }
    }
}