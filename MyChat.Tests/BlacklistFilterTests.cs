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

            Assert.That(filteredMessages.Count == 7);

            Assert.That(filteredMessages[0].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470901)));
            Assert.That(filteredMessages[0].senderId, Is.EqualTo("bob"));
            Assert.That(filteredMessages[0].content, Is.EqualTo("Hello there!"));

            Assert.That(filteredMessages[1].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470905)));
            Assert.That(filteredMessages[1].senderId, Is.EqualTo("mike"));
            Assert.That(filteredMessages[1].content, Is.EqualTo("how are you?"));

            Assert.That(filteredMessages[2].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470906)));
            Assert.That(filteredMessages[2].senderId, Is.EqualTo("bob"));
            Assert.That(filteredMessages[2].content, Is.EqualTo("I'm good thanks, do you like pie?"));

            Assert.That(filteredMessages[3].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470910)));
            Assert.That(filteredMessages[3].senderId, Is.EqualTo("mike"));
            Assert.That(filteredMessages[3].content, Is.EqualTo("*redacted*, let me ask Angus..."));

            Assert.That(filteredMessages[4].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470912)));
            Assert.That(filteredMessages[4].senderId, Is.EqualTo("angus"));
            Assert.That(filteredMessages[4].content, Is.EqualTo("Hell yes! Are we buying some pie?"));

            Assert.That(filteredMessages[5].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470914)));
            Assert.That(filteredMessages[5].senderId, Is.EqualTo("bob"));
            Assert.That(filteredMessages[5].content, Is.EqualTo("*redacted*, just want to know if there's anybody else in the pie society..."));

            Assert.That(filteredMessages[6].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470915)));
            Assert.That(filteredMessages[6].senderId, Is.EqualTo("angus"));
            Assert.That(filteredMessages[6].content, Is.EqualTo("YES! I'm the head pie eater there..."));
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
        /// Tests for an empty conversation.
        /// </summary>
        [Test]
        public void NoMessagesThrowsNoMessagesException()
        {
            var blacklistFilter = new Blacklist(new string[] { "no" });

            var conversation = new Conversation("conversation", new List<Message>());

            Assert.Throws(typeof(NoMessagesException), () => { blacklistFilter.Filter(conversation); });
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
