using System;
using System.Collections.Generic;
using NUnit.Framework;


namespace MindLink.Recruitment.MyChat.Tests
{
    [TestFixture]
    public class BlacklistCheckerTests
    {
        [Test]
        public void BlackListCheckerRemovesRequiredWords()
        {
            Conversation conversation = new Conversation("TestConversation", new List<Message>(){new Message(DateTimeOffset.Now, "Bob", "I like pie because pie is good")});
            BlacklistChecker checker = new BlacklistChecker(new List<string> { "pie", "good"});
            var expectedResult = "I like *redacted* because *redacted* is *redacted*";


            var checkedConversation = checker.CheckConversation(conversation);

            Assert.That(checkedConversation.Messages[0].Content, Is.EqualTo(expectedResult));

        }

        

    }
}
