using System;
using NUnit.Framework;

namespace MindLink.Recruitment.MyChat.Tests
{
    [TestFixture]
    public class IDFilterTests
    {
        [Test]
        public void ConversationShouldBeFilteredByID()
        {
            Conversation actualConversation = new ConversationReader("chat.txt").Conversation;
            Conversation expectedConversation = new ConversationReader("ConversationBob.txt").Conversation;

            var filter = new IDFilter() { Word = "bob" };

            actualConversation = filter.Filter(actualConversation);

            Assert.That(actualConversation.ToString(), Is.EqualTo(expectedConversation.ToString()));
        }
    }
}
