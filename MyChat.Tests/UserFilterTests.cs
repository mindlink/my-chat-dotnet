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
    /// Tests for the <see cref="UserFilter"/>.
    /// </summary>
    [TestFixture]
    public class UserFilterTests
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
        /// Tests the User Filter filters each user correctly.
        /// </summary>
        [Test]
        public void FilteringUserFiltersUser()
        {
            var usersToTest = new string[] { "Patrick" };

            var userFilter = new UserFilter(usersToTest);

            Assert.That(_dummyConversation.name, Is.EqualTo("Dummy Conversation"));

            var filteredConversation = userFilter.Filter(_dummyConversation);

            var filteredMessages = filteredConversation.messages.ToList();

            Assert.That(filteredMessages.Count == 3);

            Assert.That(filteredMessages[0].senderId, Is.EqualTo("Patrick"));
            Assert.That(filteredMessages[0].content, Is.EqualTo("I've completed my interview test and have submitted a pull request!"));

            Assert.That(filteredMessages[1].senderId, Is.EqualTo("Patrick"));
            Assert.That(filteredMessages[1].content, Is.EqualTo("What are some things you didn't like with my test submission?"));

            Assert.That(filteredMessages[2].senderId, Is.EqualTo("Patrick"));
            Assert.That(filteredMessages[2].content, Is.EqualTo("Okay thanks, working on it!"));
        }

        /// <summary>
        /// Tests for a null conversation.
        /// </summary>
        [Test]
        public void NullConversationThrowsArgumentNullException()
        {
            var userFilter = new UserFilter(new string[] { "Patrick" });

            Assert.Throws(typeof(ArgumentNullException), () => { userFilter.Filter(null); });
        }

        /// <summary>
        /// Tests for an empty conversation.
        /// </summary>
        [Test]
        public void NoMessagesThrowsNoMessagesException()
        {
            var userFilter = new UserFilter(new string[] { "Patrick" });

            var conversation = new Conversation("conversation", new List<Message>());

            Assert.Throws(typeof(NoMessagesException), () => { userFilter.Filter(conversation); });
        }

        /// <summary>
        /// Tests for no users input.
        /// </summary>
        [Test]
        public void NoUsersThrowsNoUsersException()
        {
            var userFilter = new UserFilter(new string[] {  });

            var conversation = new Conversation("conversation", new List<Message>() { new Message(DateTimeOffset.FromUnixTimeSeconds(1448470901), "senderId", "content") });

            Assert.Throws(typeof(NoUsersException), () => { userFilter.Filter(conversation); });
        }
    }
}
