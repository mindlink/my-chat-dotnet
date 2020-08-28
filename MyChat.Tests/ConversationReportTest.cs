namespace MindLink.Recruitment.MyChat.Tests
{
    using System.IO;
    using System.Linq;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using MindLink.Recruitment.MyChat;

    /// <summary>
    /// Tests for the <see cref="ConversationReport"/>.
    /// </summary>
    [TestFixture]
    class ConversationReportTest
    {
        /// <summary>
        /// Test to see frequency of users in conversation in a listed report.
        /// </summary>
        [Test]
        public void CreateReportFromConversation()
        {
            var exporter = new ConversationExporter();
            var report = new ConversationReport();

            exporter.ExportConversation("chat.txt", "chat.json");

            var serializedConversation = new StreamReader(new FileStream("chat.json", FileMode.Open)).ReadToEnd();

            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.That(savedConversation.name, Is.EqualTo("My Conversation"));

            var messages = savedConversation.messages.ToList();
            var conversationReport = report.CreateReport(messages);

            Assert.That(conversationReport[0].senderId, Is.EqualTo("bob"));
            Assert.That(conversationReport[0].messageCount, Is.EqualTo(3));

            Assert.That(conversationReport[1].senderId, Is.EqualTo("mike"));
            Assert.That(conversationReport[1].messageCount, Is.EqualTo(2));

            Assert.That(conversationReport[2].senderId, Is.EqualTo("angus"));
            Assert.That(conversationReport[2].messageCount, Is.EqualTo(2));
        }
    }
}
