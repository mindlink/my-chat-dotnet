using System.IO;
using System.Linq;
using MyChat;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MindLink.Recruitment.MyChat.Tests
{
    using System;

    /// <summary>
    /// Tests for the <see cref="ConversationExporter - filter by sender"/>.
    /// </summary>
    [TestFixture]
    public class FilerBySenderNameTests
    {
        /// <summary>
        /// Tests that exporting the conversation exports conversation filtered by the given username.
        /// </summary>
        [Test]
        public void ExportingConversationFilterByName()
        {
             var exporter = new ConversationExporter();
            string[] args = { "--filterByUser", "bob" };
            var editorCofig = new EditingConfiguration(args);
            var editor = new ConversationEditor(editorCofig);

            exporter.ExportConversation("chat.txt", "chatNameFilter.json", editor);

            var serializedConversation = new StreamReader(new FileStream("chatNameFilter.json", FileMode.Open)).ReadToEnd();

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
    }
}
