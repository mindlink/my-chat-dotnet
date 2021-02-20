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
        private Conversation _dummyConversation;

        /// <summary>
        /// Sets up the messages and conversation to be used within the tests.
        /// </summary>
        [SetUp]
        public void SetupDummyConversation()
        {
            List<Message> dummyMessages = new List<Message>()
            {
                new Message(DateTime.Now, "Patrick", "I've completed my interview test and have submitted a pull request!"),
                new Message(DateTime.Now, "MindLink", "Many thanks! The recruitment team will review your test submission and get back to you with feedback."),
                new Message(DateTime.Now, "Patrick", "What are some things you didn't like with my test submission?"),
                new Message(DateTime.Now, "MindLink", "Unit tests actually involve reading from the file system instead of using a dummy Conversation instance"),
                new Message(DateTime.Now, "Patrick", "Okay thanks, working on it!")
            };

            Conversation dummyConversation = new Conversation("Dummy Conversation", dummyMessages);

            _dummyConversation = dummyConversation;
        }

        /// <summary>
        /// Tests that the Report command issues the correct report.
        /// </summary>
        [Test]
        public void ReportIssuesReport()
        {
            var reportFilter = new Report();
            
            Assert.That(_dummyConversation.name, Is.EqualTo("Dummy Conversation"));

            var filteredConversation = reportFilter.Filter(_dummyConversation);

            var filteredActivity = filteredConversation.activity.ToList();

            Assert.That(filteredActivity.Count == 2);

            Assert.That(filteredActivity[0].count == 3);
            Assert.That(filteredActivity[0].sender == "Patrick");

            Assert.That(filteredActivity[1].count == 2);
            Assert.That(filteredActivity[1].sender == "MindLink");
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
