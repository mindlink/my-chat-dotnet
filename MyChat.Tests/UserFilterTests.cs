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
        /// <summary>
        /// Tests the User Filter filters each user correctly.
        /// </summary>
        [Test]
        public void FilteringUserFiltersUser()
        {
            var usersToTest = new string[] { "bob" };

            var conversationReader = new ConversationReader();

            var readConversation = conversationReader.ReadConversation("chat.txt");

            var userFilter = new UserFilter(usersToTest);

            Assert.That(readConversation.name, Is.EqualTo("My Conversation"));

            var filteredConversation = userFilter.Filter(readConversation);

            var filteredMessages = filteredConversation.messages.ToList();

            Assert.That(filteredMessages.Count == 3);

            Assert.That(filteredMessages[0].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470901)));
            Assert.That(filteredMessages[0].senderId, Is.EqualTo("bob"));
            Assert.That(filteredMessages[0].content, Is.EqualTo("Hello there!"));

            Assert.That(filteredMessages[1].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470906)));
            Assert.That(filteredMessages[1].senderId, Is.EqualTo("bob"));
            Assert.That(filteredMessages[1].content, Is.EqualTo("I'm good thanks, do you like pie?"));

            Assert.That(filteredMessages[2].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470914)));
            Assert.That(filteredMessages[2].senderId, Is.EqualTo("bob"));
            Assert.That(filteredMessages[2].content, Is.EqualTo("No, just want to know if there's anybody else in the pie society..."));
        }

        /// <summary>
        /// Tests for a null conversation.
        /// </summary>
        [Test]
        public void NullConversationThrowsArgumentNullException()
        {
            var userFilter = new UserFilter(new string[] { "bob" });

            Assert.Throws(typeof(ArgumentNullException), () => { userFilter.Filter(null); });
        }

        /// <summary>
        /// Tests for an empty conversation.
        /// </summary>
        [Test]
        public void NoMessagesThrowsNoMessagesException()
        {
            var userFilter = new UserFilter(new string[] { "bob" });

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
