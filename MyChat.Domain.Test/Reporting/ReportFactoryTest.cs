namespace MindLink.Recruitment.MyChat.Domain.Test.Reporting
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Generic;
    using Domain.Reporting;
    using System.Linq;
    using Domain.Conversations;

    [TestClass]
    public class ReportFactoryTest
    {
        [TestMethod]
        public void It_should_be_able_to_create_a_report_based_on_the_messages_in_the_conversation()
        {
            var messages = new List<Message>
            {
                new Message(new DateTimeOffset(new DateTime(2000, 1, 1), TimeSpan.Zero), "user1", "content"),
                new Message(new DateTimeOffset(new DateTime(2000, 1, 1), TimeSpan.Zero), "user2", "content"),
                new Message(new DateTimeOffset(new DateTime(2000, 1, 1), TimeSpan.Zero), "user1", "content"),
                new Message(new DateTimeOffset(new DateTime(2000, 1, 1), TimeSpan.Zero), "user1", "content"),
                new Message(new DateTimeOffset(new DateTime(2000, 1, 1), TimeSpan.Zero), "user3", "content"),
                new Message(new DateTimeOffset(new DateTime(2000, 1, 1), TimeSpan.Zero), "user3", "content"),
            };

            var conversation = new Conversation("Conversation Name", messages);

            var reportFactory = new ReportFactory();
            var report = reportFactory.CreateMostActiveUsersReport(conversation);
            var result = report.Generate();

            Assert.AreEqual(3, result.Count());

            var first = result.ElementAt(0);
            Assert.AreEqual("user1", first.UserId);
            Assert.AreEqual(3, first.NumberOfMessages);

            var second = result.ElementAt(1);
            Assert.AreEqual("user3", second.UserId);
            Assert.AreEqual(2, second.NumberOfMessages);

            var third = result.ElementAt(2);
            Assert.AreEqual("user2", third.UserId);
            Assert.AreEqual(1, third.NumberOfMessages);
        }
    }
}
