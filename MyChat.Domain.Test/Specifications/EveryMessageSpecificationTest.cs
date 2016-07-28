namespace MindLink.Recruitment.MyChat.Domain.Test.Specifications
{
    using Domain.Conversations;
    using Domain.Specifications;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;

    [TestClass]
    public class EveryMessageSpecificationTest
    {
        [TestMethod]
        public void Is_is_always_satisfied_by_every_message()
        {
            var spec = new EveryMessageSpecification();

            var candidates = new List<Message>
            {
                new Message(new DateTimeOffset(new DateTime(2000, 1, 1), TimeSpan.Zero), "senderId", "content"),
                new Message(new DateTimeOffset(new DateTime(2000, 1, 1), TimeSpan.Zero), "1", "1")
            };

            foreach (var candidate in candidates)
                Assert.IsTrue(spec.IsSatisfiedBy(candidate));
        }
    }
}
