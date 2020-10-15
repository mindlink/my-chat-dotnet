using System.Linq;
using MyChat;
using NUnit.Framework;

namespace MindLink.Recruitment.MyChat.Tests
{
    using System;
    using System.Collections.Generic;
    using MindLink.Recruitment.MyChat;

    /// <summary>
    /// Tests for the <see cref="ConversationExporter - filter by sender"/>.
    /// </summary>
    [TestFixture]
    public class ReportTests
    {
        /// <summary>
        /// Tests that exporting the conversation exports conversation filtered by the given username.
        /// </summary>
        [Test]
        public void ReportTest()
        {
            var messages = new List<Message>();
            var messageOne = new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470901")), "stan", "i tested string");
            messages.Add(messageOne);
            var messageTwo = new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470901")), "stan", "i tested string");
            messages.Add(messageTwo);
            var messageThree = new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470901")), "bob", "i tested string");
            messages.Add(messageThree);     

            var testConversation = new Conversation("test", messages);
            string[] args = { "--report" };
            var editorCofig = new EditorConfiguration(args);
            var logCreator = new LogCreator(editorCofig);

            var reportList = logCreator.AddReport(testConversation);

            Assert.That(reportList[0].sender, Is.EqualTo("stan"));
            Assert.That(reportList[0].count, Is.EqualTo(2));

            Assert.That(reportList[1].sender, Is.EqualTo("bob"));
            Assert.That(reportList[1].count, Is.EqualTo(1));
        }
    }
}