using MindLink.Recruitment.MyChat.Controllers;
using MindLink.Recruitment.MyChat.Interfaces.ControllerInterfaces;
using MyChatModel.ModelData;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MindLink.Recruitment.MyChat.Tests.IntegrationTests
{
    /// <summary>
    /// Testing the ExportController class and whether it exports
    /// the files as intended
    /// </summary>
    [TestFixture]
    public class ExportControllerTests
    {
        // IExportController called exportController to be tested
        private IExportController exportController;

        IReadController readController;
        IWriteController writeController;
        IFilterController filterController;
        IReportController reportController;

        private ConversationExporterConfiguration conversationConfig;

        /// <summary>
        /// Tests whether the ExportController works as intended, with intended data
        /// </summary>
        [Test]
        public void ExportControllerExportsFiles() 
        {

            // INITIALISE an IReadController, IWriteController, IFilterController, IReportController as their respective
            // concrete implementations, ReadController, WriteController, FilterController and ReportController
            readController = new ReadController();
            writeController = new WriteController();
            filterController = new FilterController();
            reportController = new ReportController();

            // INITIALISE the IExportController
            exportController = new ExportController(readController, writeController, filterController, reportController);

            conversationConfig = new CommandLineArgumentParser().ParseCommandLineArguments(new string[]{"chat.txt", "chatE.json" }, filterController);

            exportController.ExportConversation(conversationConfig.inputFilePath, conversationConfig.outputFilePath);

            var serializedConversation = new StreamReader(new FileStream("chatE.json", FileMode.Open)).ReadToEnd();

            Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.That(savedConversation.Name, Is.EqualTo("My Conversation"));

            var messages = savedConversation.Messages.ToList();

            Assert.That(savedConversation.Report.mostActiveUser, Is.EqualTo("angus"));

            Assert.That(savedConversation.Report.userActivityRanking[0], Is.EqualTo("Rank 1 is angus with 4 messages"));
            Assert.That(savedConversation.Report.userActivityRanking[1], Is.EqualTo("Rank 2 is bob with 3 messages"));
            Assert.That(savedConversation.Report.userActivityRanking[2], Is.EqualTo("Rank 3 is mike with 2 messages"));

            Assert.That(messages[0].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470901)));
            Assert.That(messages[0].SenderId, Is.EqualTo("bob"));
            Assert.That(messages[0].Content, Is.EqualTo("Hello there!"));

            Assert.That(messages[1].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470905)));
            Assert.That(messages[1].SenderId, Is.EqualTo("mike"));
            Assert.That(messages[1].Content, Is.EqualTo("how are you?"));

            Assert.That(messages[2].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470906)));
            Assert.That(messages[2].SenderId, Is.EqualTo("bob"));
            Assert.That(messages[2].Content, Is.EqualTo("I'm good thanks, do you like pie?"));

            Assert.That(messages[3].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470910)));
            Assert.That(messages[3].SenderId, Is.EqualTo("mike"));
            Assert.That(messages[3].Content, Is.EqualTo("no, let me ask Angus..."));

            Assert.That(messages[4].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470912)));
            Assert.That(messages[4].SenderId, Is.EqualTo("angus"));
            Assert.That(messages[4].Content, Is.EqualTo("Hell yes! Are we buying some pie?"));

            Assert.That(messages[5].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470914)));
            Assert.That(messages[5].SenderId, Is.EqualTo("bob"));
            Assert.That(messages[5].Content, Is.EqualTo("No, just want to know if there's anybody else in the pie society..."));

            Assert.That(messages[6].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470915)));
            Assert.That(messages[6].SenderId, Is.EqualTo("angus"));
            Assert.That(messages[6].Content, Is.EqualTo("YES! I'm the head pie eater there..."));
        }
        /// <summary>
        /// Tests whether the ExportController works as intended, with intended data
        /// </summary>
        [Test]
        public void ExportControllerExportsFilesWithFilters()
        {

            // INITIALISE an IReadController, IWriteController, IFilterController, IReportController as their respective
            // concrete implementations, ReadController, WriteController, FilterController and ReportController
            readController = new ReadController();
            writeController = new WriteController();
            filterController = new FilterController();
            reportController = new ReportController();

            // INITIALISE the IExportController
            exportController = new ExportController(readController, writeController, filterController, reportController);

            conversationConfig = new CommandLineArgumentParser().ParseCommandLineArguments(new string[] { "chat.txt", "chatF.json", "-filter-user", "bob" }, filterController);

            exportController.ExportConversation(conversationConfig.inputFilePath, conversationConfig.outputFilePath);

            var serializedConversation = new StreamReader(new FileStream("chatF.json", FileMode.Open)).ReadToEnd();

            Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.That(savedConversation.Name, Is.EqualTo("My Conversation"));

            var messages = savedConversation.Messages.ToList();

            Assert.That(savedConversation.Report.mostActiveUser, Is.EqualTo("bob"));

            Assert.That(savedConversation.Report.userActivityRanking[0], Is.EqualTo("Rank 1 is bob with 3 messages"));

            Assert.That(messages[0].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470901)));
            Assert.That(messages[0].SenderId, Is.EqualTo("bob"));
            Assert.That(messages[0].Content, Is.EqualTo("Hello there!"));

            Assert.That(messages[1].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470906)));
            Assert.That(messages[1].SenderId, Is.EqualTo("bob"));
            Assert.That(messages[1].Content, Is.EqualTo("I'm good thanks, do you like pie?"));

            Assert.That(messages[2].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470914)));
            Assert.That(messages[2].SenderId, Is.EqualTo("bob"));
            Assert.That(messages[2].Content, Is.EqualTo("No, just want to know if there's anybody else in the pie society..."));

        }

        /// <summary>
        /// Tests whether the ExportController works as intended, with intended data
        /// </summary>
        [Test]
        public void ExportControllerExportsFilesWithMultipleFilters1()
        {

            // INITIALISE an IReadController, IWriteController, IFilterController, IReportController as their respective
            // concrete implementations, ReadController, WriteController, FilterController and ReportController
            readController = new ReadController();
            writeController = new WriteController();
            filterController = new FilterController();
            reportController = new ReportController();

            // INITIALISE the IExportController
            exportController = new ExportController(readController, writeController, filterController, reportController);

            conversationConfig = 
                new CommandLineArgumentParser().ParseCommandLineArguments(
                    new string[] { "chat.txt", "chatG.json", "-filter-user", "bob", "-filter-blacklist-word", "pie" },
                    filterController);

            exportController.ExportConversation(conversationConfig.inputFilePath, conversationConfig.outputFilePath);

            var serializedConversation = new StreamReader(new FileStream("chatG.json", FileMode.Open)).ReadToEnd();

            Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.That(savedConversation.Name, Is.EqualTo("My Conversation"));

            var messages = savedConversation.Messages.ToList();

            Assert.That(savedConversation.Report.mostActiveUser, Is.EqualTo("bob"));

            Assert.That(savedConversation.Report.userActivityRanking[0], Is.EqualTo("Rank 1 is bob with 3 messages"));

            Assert.That(messages[0].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470901)));
            Assert.That(messages[0].SenderId, Is.EqualTo("bob"));
            Assert.That(messages[0].Content, Is.EqualTo("Hello there!"));

            Assert.That(messages[1].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470906)));
            Assert.That(messages[1].SenderId, Is.EqualTo("bob"));
            Assert.That(messages[1].Content, Is.EqualTo("I'm good thanks, do you like \\*redacted\\*?"));

            Assert.That(messages[2].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470914)));
            Assert.That(messages[2].SenderId, Is.EqualTo("bob"));
            Assert.That(messages[2].Content, Is.EqualTo("No, just want to know if there's anybody else in the \\*redacted\\* society..."));

        }

        /// <summary>
        /// Tests whether the ExportController works as intended, with intended data
        /// </summary>
        [Test]
        public void ExportControllerExportsFilesWithMultipleFilters2()
        {

            // INITIALISE an IReadController, IWriteController, IFilterController, IReportController as their respective
            // concrete implementations, ReadController, WriteController, FilterController and ReportController
            readController = new ReadController();
            writeController = new WriteController();
            filterController = new FilterController();
            reportController = new ReportController();

            // INITIALISE the IExportController
            exportController = new ExportController(readController, writeController, filterController, reportController);

            conversationConfig =
                new CommandLineArgumentParser().ParseCommandLineArguments(
                    new string[] { "chat.txt", "chatH.json", "-filter-user", "bob", "-filter-blacklist-word", "pie|society", "-filter-obfuscate" },
                    filterController);

            exportController.ExportConversation(conversationConfig.inputFilePath, conversationConfig.outputFilePath);

            var serializedConversation = new StreamReader(new FileStream("chatH.json", FileMode.Open)).ReadToEnd();

            Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.That(savedConversation.Name, Is.EqualTo("My Conversation"));

            var messages = savedConversation.Messages.ToList();

            Assert.That(savedConversation.Report.mostActiveUser, Is.EqualTo("User3071"));

            Assert.That(savedConversation.Report.userActivityRanking[0], Is.EqualTo("Rank 1 is User3071 with 3 messages"));

            Assert.That(messages[0].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470901)));
            Assert.That(messages[0].SenderId, Is.EqualTo("User3071"));
            Assert.That(messages[0].Content, Is.EqualTo("Hello there!"));

            Assert.That(messages[1].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470906)));
            Assert.That(messages[1].SenderId, Is.EqualTo("User3071"));
            Assert.That(messages[1].Content, Is.EqualTo("I'm good thanks, do you like \\*redacted\\*?"));

            Assert.That(messages[2].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470914)));
            Assert.That(messages[2].SenderId, Is.EqualTo("User3071"));
            Assert.That(messages[2].Content, Is.EqualTo("No, just want to know if there's anybody else in the \\*redacted\\* \\*redacted\\*..."));

        }

    }
}
