using System;
using System.Linq;
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

            Assert.That(filteredMessages.Count() == 2);

            Assert.That(filteredMessages[0].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470905)));
            Assert.That(filteredMessages[1].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470906)));

            Assert.That(filteredMessages[0].senderId, Is.EqualTo("mike"));
            Assert.That(filteredMessages[1].senderId, Is.EqualTo("bob"));

            Assert.That(filteredMessages[0].content, Is.EqualTo("how are you?"));
            Assert.That(filteredMessages[1].content, Is.EqualTo("I'm good thanks, do you like pie?"));
        }
    }
}
