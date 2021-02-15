using System;
using System.Linq;
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
        /// <summary>
        /// Tests that the Blacklist Filter redacts each word in a message correctly.
        /// </summary>
        [Test]
        public void BlacklistRedactsWord()
        {
            var blacklistedWordsToTest = new string[] { "no" };

            var conversationReader = new ConversationReader();

            var readConversation = conversationReader.ReadConversation("chat.txt");

            var blacklistFilter = new Blacklist(blacklistedWordsToTest);

            Assert.That(readConversation.name, Is.EqualTo("My Conversation"));

            var filteredConversation = blacklistFilter.Filter(readConversation);

            var filteredMessages = filteredConversation.messages.ToList();

            Assert.That(filteredMessages.Count() == 7);

            Assert.That(filteredMessages[0].content, Is.EqualTo("Hello there!"));
            Assert.That(filteredMessages[1].content, Is.EqualTo("how are you?"));
            Assert.That(filteredMessages[2].content, Is.EqualTo("I'm good thanks, do you like pie?"));
            Assert.That(filteredMessages[3].content, Is.EqualTo("*redacted*, let me ask Angus..."));
            Assert.That(filteredMessages[4].content, Is.EqualTo("Hell yes! Are we buying some pie?"));
            Assert.That(filteredMessages[5].content, Is.EqualTo("*redacted*, just want to know if there's anybody else in the pie society..."));
            Assert.That(filteredMessages[6].content, Is.EqualTo("YES! I'm the head pie eater there..."));
        }
    }
}
