using System;
using NUnit.Framework;


namespace MindLink.Recruitment.MyChat.Tests
{
    [TestFixture]
    public class KeywordFilterTests
    {
        [Test]
        public void ConversationIsFilteredByKeyword()
        {
            Conversation actualConversation = new ConversationReader("chat.txt").Conversation;
            Conversation expectedConversation = new ConversationReader("ConversationPie.txt").Conversation;

            var filter = new IDFilter() { Word = "pie" };

            actualConversation = filter.Filter(actualConversation);

            Assert.That(actualConversation.ToString(), Is.EqualTo(expectedConversation.ToString())); ;

        }
    }
}
