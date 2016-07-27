namespace MindLink.Recruitment.MyChat.Domain.Test.Specifications
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Domain.Specifications;
    using Domain.Conversations;

    [TestClass]
    public class MessageOfUserSpecificationTest
    {
        [TestMethod]
        public void It_should_be_satisfied_when_message_belongs_to_the_specified_sender()
        {
            var specification = new MessageOfUserSpecification("senderId1");

            var message = new Message(new DateTime(2000, 1, 1), "senderId1", "content");
            Assert.IsTrue(specification.IsSatisfiedBy(message));
        }

        [TestMethod]
        public void It_should_not_be_satisfied_when_message_belongs_to_another_sender()
        {
            var specification = new MessageOfUserSpecification("senderId1");

            var message = new Message(new DateTime(2000, 1, 1), "senderId2", "content");
            Assert.IsFalse(specification.IsSatisfiedBy(message));
        }
    }
}
