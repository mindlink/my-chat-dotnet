namespace MindLink.Recruitment.MyChat.Domain.Test.Specifications
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Domain.Specifications;
    using Domain.Conversations;

    [TestClass]
    public class MessageContainingKeywordSpecificationTest
    {
        [TestMethod]
        public void It_should_be_satisfied_when_keyword_appears_in_the_message_content()
        {
            var specification = new MessageContainingKeywordSpecification("needle");

            var message = new Message(new DateTime(2000, 1, 1), "senderId", "needle in a haystack");
            Assert.IsTrue(specification.IsSatisfiedBy(message));
        }

        [TestMethod]
        public void It_should_not_be_satisfied_when_keyword_does_not_appear_in_message_content()
        {
            var specification = new MessageOfUserSpecification("needle");

            var message = new Message(new DateTime(2000, 1, 1), "senderId", "hippo in a haystack");
            Assert.IsFalse(specification.IsSatisfiedBy(message));
        }
    }
}
