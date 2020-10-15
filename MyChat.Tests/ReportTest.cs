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
    public class ReporTests
    {
        /// <summary>
        /// Tests that exporting the conversation exports conversation filtered by the given username.
        /// </summary>
        [Test]
        public void ExportingConversationIncludingReport()
        {
             var exporter = new ConversationExporter();
            string[] args = { "--report" };
            var editorCofig = new EditorConfiguration(args);
            var editor = new ConversationEditor(editorCofig);
            var writer = new LogWriter(editorCofig);

            exporter.ExportConversation("chat.txt", "chatReport.json", editor, writer);

            var serializedConversation = new StreamReader(new FileStream("chatReport.json", FileMode.Open)).ReadToEnd();

            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.That(savedConversation.name, Is.EqualTo("My Conversation"));

            var messages = savedConversation.messages.ToList();
            var reportList = savedConversation.activity.ToList();

            Assert.That(messages[0].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470901)));
            Assert.That(messages[0].senderId, Is.EqualTo("bob"));
            Assert.That(messages[0].content, Is.EqualTo("Hello there!"));

            Assert.That(reportList[0].sender, Is.EqualTo("bob"));
            Assert.That(reportList[0].count, Is.EqualTo(3));

            Assert.That(reportList[1].sender, Is.EqualTo("mike"));
            Assert.That(reportList[1].count, Is.EqualTo(2));

            Assert.That(reportList[2].sender, Is.EqualTo("angus"));
            Assert.That(reportList[2].count, Is.EqualTo(2));

        }
    }
}