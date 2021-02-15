using System;
using System.Linq;
using MindLink.Recruitment.MyChat.Filters;
using NUnit.Framework;

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

            var filteredMessages = filteredConversation.messages.ToList();

            // Add assertions that would check that the report exprots correctly

        }
    }
}
