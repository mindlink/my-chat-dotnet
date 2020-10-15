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
    public class IntegrationFilterTests
    {
        /// <summary>
        /// Tests that exporting the conversation exports conversation filtered by the given username.
        /// </summary>
        [Test]
        public void ExportingConversationWithFilters()
        {
             var exporter = new ConversationExporter();
            string[] args = {"--filterByUser", "bob", "--filterByKeyword", "society", "--blacklist", "pie", "--report" };
            var editorCofig = new EditorConfiguration(args);
            var editor = new ConversationEditor(editorCofig);
            var logCreator = new LogCreator(editorCofig);

            exporter.ExportConversation("chat.txt", "chatFilter.json", editor, logCreator);

            var serializedConversation = new StreamReader(new FileStream("chatFilter.json", FileMode.Open)).ReadToEnd();

            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.That(savedConversation.name, Is.EqualTo("My Conversation"));

            var messages = savedConversation.messages.ToList();
            var reportList = savedConversation.activity.ToList();

            Assert.That(messages[0].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470914)));
            Assert.That(messages[0].senderId, Is.EqualTo("bob"));
            Assert.That(messages[0].content, Is.EqualTo("No, just want to know if there's anybody else in the *redacted* society..."));
        }
    }
}