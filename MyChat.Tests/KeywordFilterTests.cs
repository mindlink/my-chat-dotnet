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
        /// <summary>
        /// Tests that the Keyword Filter filters each user correctly.
        /// </summary>
        [Test]
        public void FilteringKeywordFiltersKeyword()
        {
            var keywordsToTest = new string[] { "you" };

            var conversationReader = new ConversationReader();

            var readConversation = conversationReader.ReadConversation("chat.txt");

            var keywordFilter = new KeywordFilter(keywordsToTest);

            Assert.That(readConversation.name, Is.EqualTo("My Conversation"));

            var filteredConversation = keywordFilter.Filter(readConversation);

            var filteredMessages = filteredConversation.messages.ToList();

            Assert.That(filteredMessages.Count == 2);

            Assert.That(filteredMessages[0].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470905)));
            Assert.That(filteredMessages[0].senderId, Is.EqualTo("mike"));
            Assert.That(filteredMessages[0].content, Is.EqualTo("how are you?"));

            Assert.That(filteredMessages[1].senderId, Is.EqualTo("bob"));
            Assert.That(filteredMessages[1].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470906)));
            Assert.That(filteredMessages[1].content, Is.EqualTo("I'm good thanks, do you like pie?"));
        }


        /// <summary>
        /// Tests for a null conversation.
        /// </summary>
        [Test]
        public void NullConversationThrowsArgumentNullException()
        {
            var keywordFilter = new KeywordFilter(new string[] { "pie" });

            Assert.Throws(typeof(ArgumentNullException), () => { keywordFilter.Filter(null); });
        }

        /// <summary>
        /// Tests for an empty conversation.
        /// </summary>
        [Test]
        public void NoMessagesThrowsNoMessagesException()
        {
            var keywordFilter = new KeywordFilter(new string[] { "pie" });

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
