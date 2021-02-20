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
    /// Tests for the <see cref="Blacklist"/>.
    /// </summary>
    [TestFixture]
    public class BlacklistFilterTests
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
        /// Tests that the Blacklist Filter redacts each word in a message correctly.
        /// </summary>
        [Test]
        public void BlacklistRedactsWord()
        {
            var blacklistedWordsToTest = new string[] { "thanks", "you" };

            var blacklistFilter = new Blacklist(blacklistedWordsToTest);

            Assert.That(_dummyConversation.name, Is.EqualTo("Dummy Conversation"));

            var filteredConversation = blacklistFilter.Filter(_dummyConversation);

            var filteredMessages = filteredConversation.messages.ToList();

            Assert.That(filteredMessages.Count == 5);

            Assert.That(filteredMessages[0].senderId, Is.EqualTo("Patrick"));
            Assert.That(filteredMessages[0].content, Is.EqualTo("I've completed my interview test and have submitted a pull request!"));

            Assert.That(filteredMessages[1].senderId, Is.EqualTo("MindLink"));
            Assert.That(filteredMessages[1].content, Is.EqualTo("Many *redacted*! The recruitment team will review your test submission and get back to *redacted* with feedback."));

            Assert.That(filteredMessages[2].senderId, Is.EqualTo("Patrick"));
            Assert.That(filteredMessages[2].content, Is.EqualTo("What are some things *redacted* didn't like with my test submission?"));

            Assert.That(filteredMessages[3].senderId, Is.EqualTo("MindLink"));
            Assert.That(filteredMessages[3].content, Is.EqualTo("Unit tests actually involve reading from the file system instead of using a dummy Conversation instance"));

            Assert.That(filteredMessages[4].senderId, Is.EqualTo("Patrick"));
            Assert.That(filteredMessages[4].content, Is.EqualTo("Okay *redacted*, working on it!"));
        }

        /// <summary>
        /// Tests for a null conversation.
        /// </summary>
        [Test]
        public void NullConversationThrowsArgumentNullException()
        {
            var blacklistFilter = new Blacklist(new string[] { "no" });

            Assert.Throws(typeof(ArgumentNullException), () => { blacklistFilter.Filter(null); });
        }

        /// <summary>
        /// Tests for no keywords input.
        /// </summary>
        [Test]
        public void NoBlacklistedWordsThrowsNoBlacklistedWordsException()
        {
            var blacklistFilter = new Blacklist(new string[] { });

            var conversation = new Conversation("conversation", new List<Message>() { new Message(DateTimeOffset.FromUnixTimeSeconds(1448470901), "senderId", "content") });

            Assert.Throws(typeof(NoBlacklistedWordsException), () => { blacklistFilter.Filter(conversation); });
        }
    }
}
