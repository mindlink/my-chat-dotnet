using System;
using System.Collections.Generic;
using System.Linq;
using MindLink.Recruitment.MyChat.Data;
using MindLink.Recruitment.MyChat.Exceptions;
using MindLink.Recruitment.MyChat.Filters;
using NUnit.Framework;

namespace MindLink.Recruitment.MyChat.Tests
{
    /// <summary>
    /// Tests for the <see cref="KeywordFilter"/>.
    /// </summary>
    [TestFixture]
    public class KeywordFilterTests
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
        /// Tests that the Keyword Filter filters each user correctly.
        /// </summary>
        [Test]
        public void FilteringKeywordFiltersKeyword()
        {
            var keywordsToTest = new string[] { "thanks" };

            var keywordFilter = new KeywordFilter(keywordsToTest);

            Assert.That(_dummyConversation.name, Is.EqualTo("Dummy Conversation"));

            var filteredConversation = keywordFilter.Filter(_dummyConversation);

            var filteredMessages = filteredConversation.messages.ToList();

            Assert.That(filteredMessages.Count == 2);

            Assert.That(filteredMessages[0].senderId, Is.EqualTo("MindLink"));
            Assert.That(filteredMessages[0].content, Is.EqualTo("Many thanks! The recruitment team will review your test submission and get back to you with feedback."));

            Assert.That(filteredMessages[1].senderId, Is.EqualTo("Patrick"));
            Assert.That(filteredMessages[1].content, Is.EqualTo("Okay thanks, working on it!"));
        }


        /// <summary>
        /// Tests for a null conversation.
        /// </summary>
        [Test]
        public void NullConversationThrowsArgumentNullException()
        {
            var keywordFilter = new KeywordFilter(new string[] { "thanks" });

            Assert.Throws(typeof(ArgumentNullException), () => { keywordFilter.Filter(null); });
        }

        /// <summary>
        /// Tests for an empty conversation.
        /// </summary>
        [Test]
        public void NoMessagesThrowsNoMessagesException()
        {
            var keywordFilter = new KeywordFilter(new string[] { "thanks" });

            var conversation = new Conversation("conversation", new List<Message>());

            Assert.Throws(typeof(NoMessagesException), () => { keywordFilter.Filter(conversation); });
        }

        /// <summary>
        /// Tests for no keywords input.
        /// </summary>
        [Test]
        public void NoKeywordsThrowsNoKeywordsException()
        {
            var keywordFilter = new KeywordFilter(new string[] { });

            var conversation = new Conversation("conversation", new List<Message>() { new Message(DateTimeOffset.FromUnixTimeSeconds(1448470901), "senderId", "content") });

            Assert.Throws(typeof(NoKeywordsException), () => { keywordFilter.Filter(conversation); });
        }
    }
}
