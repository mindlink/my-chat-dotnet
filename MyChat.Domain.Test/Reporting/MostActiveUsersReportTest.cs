namespace MindLink.Recruitment.MyChat.Domain.Test.Reporting
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MindLink.Recruitment.MyChat.Domain.Reporting;
    using System.Linq;

    [TestClass]
    public class MostActiveUsersReportTest
    {
        [TestMethod]
        public void It_create_an_activity_with_one_message_by_default()
        {
            var report = new MostActiveUsersReport();
            report.AddUserActivity("user1");
            Assert.AreEqual(1, report.Activities.First().NumberOfMessages);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void It_should_reject_negative_value_for_number_of_messages()
        {
            var report = new MostActiveUsersReport();
            report.AddUserActivity("user1", -1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void It_should_reject_zero_value_for_number_of_messages()
        {
            var report = new MostActiveUsersReport();
            report.AddUserActivity("user1", 0);
        }

        [TestMethod]
        public void It_should_retrieve_the_aggregated_activity_of_each_user()
        {
            var report = new MostActiveUsersReport();
            report.AddUserActivity("user1", 1);
            report.AddUserActivity("user1", 1);

            Assert.AreEqual(1, report.Generate().Count());
            Assert.AreEqual("user1", report.Generate().First().UserId);
        }

        [TestMethod]
        public void It_should_sort_the_result_based_on_the_most_active_user()
        {
            var report = new MostActiveUsersReport();
            report.AddUserActivity("user1", 1);
            report.AddUserActivity("user2", 2);

            Assert.AreEqual("user2", report.Generate().First().UserId);
            Assert.AreEqual("user1", report.Generate().ElementAt(1).UserId);
        }
    }
}
