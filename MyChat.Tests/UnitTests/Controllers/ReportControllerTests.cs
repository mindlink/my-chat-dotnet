namespace MindLink.Recruitment.MyChat.Tests.UnitTests.Controllers
{
    using MindLink.Recruitment.MyChat.Controllers;
    using MindLink.Recruitment.MyChat.Interfaces.ControllerInterfaces;
    using MyChatModel.ModelData;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Class to test the report functionality
    /// </summary>
    [TestFixture]
    public class ReportControllerTests
    {
        // IReportController to be tests
        private IReportController reportController;
        // Conversation to test the report with
        private Conversation conversation;

        /// <summary>
        /// Tests whether the report acts as intended
        /// </summary>
        [Test]
        public void ReportControllerReportsData()
        {
            // INITIALISE reportController variable
            reportController = new ReportController();
            // CALL MakeConversation method 
            MakeConverSation();
            // GenerateReport passing in the conversation
            conversation = reportController.GenerateReport(conversation);

            Assert.That(conversation.Report.mostActiveUser, Is.EqualTo("angus"));

            Assert.That(conversation.Report.userActivityRanking[0], Is.EqualTo("Rank 1 is angus with 4 messages"));
            Assert.That(conversation.Report.userActivityRanking[1], Is.EqualTo("Rank 2 is bob with 3 messages"));
            Assert.That(conversation.Report.userActivityRanking[2], Is.EqualTo("Rank 3 is mike with 2 messages"));

        }


        private void MakeConverSation()
        {
            // IList of type Message called msgs
            IList<Message> messages = new List<Message>();

            messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(1448470901)), "bob", "Hello there!"));
            messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(1448470905)), "mike", "how are you?"));
            messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(1448470906)), "bob", "I'm good thanks, do you like pie?"));
            messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(1448470910)), "mike", "no, let me ask Angus..."));
            messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(1448470912)), "angus", "Hell yes! Are we buying some pie?"));
            messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(1448470914)), "bob", "No, just want to know if there's anybody else in the pie society..."));
            messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(1448470915)), "angus", "YES! I'm the head pie eater there..."));
            messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(1448470916)), "angus", "4578075020647520"));
            messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(1448470916)), "angus", "My phone number is 07864275981"));


            conversation = new Conversation("Test conversation", messages);

        }
    }
}
