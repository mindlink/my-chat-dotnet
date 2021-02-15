using System;
using System.Linq;
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

            Assert.That(filteredMessages.Count() == 3);

            Assert.That(filteredMessages[0].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470901)));
            Assert.That(filteredMessages[1].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470906)));
            Assert.That(filteredMessages[2].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470914)));

            Assert.That(filteredMessages[0].senderId, Is.EqualTo("bob"));
            Assert.That(filteredMessages[1].senderId, Is.EqualTo("bob"));
            Assert.That(filteredMessages[2].senderId, Is.EqualTo("bob"));

            Assert.That(filteredMessages[0].content, Is.EqualTo("Hello there!"));
            Assert.That(filteredMessages[1].content, Is.EqualTo("I'm good thanks, do you like pie?"));
            Assert.That(filteredMessages[2].content, Is.EqualTo("No, just want to know if there's anybody else in the pie society..."));
        }
    }
}
