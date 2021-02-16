using System;
using System.Linq;
using NUnit.Framework;
using MindLink.Recruitment.MyChat.Filters;
using MindLink.Recruitment.MyChat.Exceptions;
using MindLink.Recruitment.MyChat.Data;
using System.Collections.Generic;

namespace MindLink.Recruitment.MyChat.Tests
{
    /// <summary>
    /// Tests for the <see cref="Report"/>.
    /// </summary>
    [TestFixture]
    public class ReportTests
    {
        /// <summary>
        /// Tests that the Report command issues the correct report.
        /// </summary>
        [Test]
        public void ReportIssuesReport()
        {
            var conversationReader = new ConversationReader();

            var readConversation = conversationReader.ReadConversation("chat.txt");

            var reportFilter = new Report();
            
            Assert.That(readConversation.name, Is.EqualTo("My Conversation"));

            var filteredConversation = reportFilter.Filter(readConversation);

            var filteredActivity = filteredConversation.activity.ToList();

            Assert.That(filteredActivity.Count == 3);

            Assert.That(filteredActivity[0].count == 3);
            Assert.That(filteredActivity[0].sender == "bob");

            Assert.That(filteredActivity[1].count == 2);
            Assert.That(filteredActivity[1].sender == "mike");

            Assert.That(filteredActivity[2].count == 2);
            Assert.That(filteredActivity[2].sender == "angus");
        }

        /// <summary>
        /// Tests for a null conversation.
        /// </summary>
        [Test]
        public void NullConversationThrowsArgumentNullException()
        {
            var reportFilter = new Report();

            Assert.Throws(typeof(ArgumentNullException), () => { reportFilter.Filter(null); });
        }

        /// <summary>
        /// Tests for an empty conversation.
        /// </summary>
        [Test]
        public void NoMessagesThrowsNoMessagesException()
        {
            var reportFilter = new Report();

            var conversation = new Conversation("conversation", new List<Message>());

            Assert.Throws(typeof(NoMessagesException), () => { reportFilter.Filter(conversation); });
        }
    }
}
