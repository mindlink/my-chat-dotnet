using System.IO;
using System.Linq;
using MyChat;
using Newtonsoft.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MindLink.Recruitment.MyChat.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Text.RegularExpressions;
    using System.Collections.Generic;
    [TestClass]
    public class ConversationExporterTests
    {
        private ProgramOptions ProgramOptionsBasic()
        {
            return new ProgramOptions
            {
                InputFile = "chat.txt",
                OutputFile = "chat.json",
            };
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            File.Delete("chat.json");
        }

        [TestMethod]
        public void TestSenderFilter()
        {
            ProgramOptions options = ProgramOptionsBasic();
            options.UserFilter = "bob";

            ConversationExporter exporter = new ConversationExporter();
            bool exportResult = exporter.ExportConversation(options);
            Assert.IsTrue(exportResult);

            using (var serialisedConversation = new StreamReader("chat.json"))
            {
                var savedConversation = JsonConvert.DeserializeObject<Conversation>(serialisedConversation.ReadToEnd());
                Assert.AreEqual(3, savedConversation.Messages.Count());
                Assert.IsTrue(savedConversation.Messages.All(m => m.Sender == "bob"));
            }
        }

        [TestMethod]
        public void TestSenderFilterSenderDoesntExist()
        {
            ProgramOptions options = ProgramOptionsBasic();
            options.UserFilter = "anna";

            ConversationExporter exporter = new ConversationExporter();
            bool exportResult = exporter.ExportConversation(options);
            Assert.IsTrue(exportResult);

            using (var serialisedConversation = new StreamReader("chat.json"))
            {
                var savedConversation = JsonConvert.DeserializeObject<Conversation>(serialisedConversation.ReadToEnd());
                Assert.AreEqual(0, savedConversation.Messages.Count());
            }
        }

        [TestMethod]
        public void TestCreditCardAndPhoneNumberObfuscation()
        {
            ProgramOptions options = ProgramOptionsBasic();
            options.HideCCAndPhoneNumbers = true;

            ConversationExporter exporter = new ConversationExporter();
            bool exportResult = exporter.ExportConversation(options);
            Assert.IsTrue(exportResult);

            using (var serialisedConversation = new StreamReader("chat.json"))
            {
                var savedConversation = JsonConvert.DeserializeObject<Conversation>(serialisedConversation.ReadToEnd());
                Assert.AreEqual(7, savedConversation.Messages.Count());

                var redactedMessage = savedConversation.Messages.ElementAt(0);
                Assert.AreEqual(2, Regex.Matches(redactedMessage.Content, @"\*redacted\*").Count);
            }
        }

        [TestMethod]
        public void TestBlackListWordFiltering()
        {
            ProgramOptions options = ProgramOptionsBasic();
            options.BlacklistedWords = new List<string>() { "pie" };

            ConversationExporter exporter = new ConversationExporter();
            bool exportResult = exporter.ExportConversation(options);
            Assert.IsTrue(exportResult);

            using (var serialisedConversation = new StreamReader("chat.json"))
            {
                var savedConversation = JsonConvert.DeserializeObject<Conversation>(serialisedConversation.ReadToEnd());
                Assert.AreEqual(7, savedConversation.Messages.Count());

                int actualRedacted = 0;
                foreach(var m in savedConversation.Messages)
                {
                    actualRedacted += Regex.Matches(m.Content, @"\*redacted\*").Count;
                }

                Assert.AreEqual(4, actualRedacted);
            }
        }

        [TestMethod]
        public void TestKeywordFiltering()
        {
            ProgramOptions options = ProgramOptionsBasic();
            options.KeywordFilter = "Hello";

            ConversationExporter exporter = new ConversationExporter();
            bool exportResult = exporter.ExportConversation(options);
            Assert.IsTrue(exportResult);

            using (var serialisedConversation = new StreamReader("chat.json"))
            {
                var savedConversation = JsonConvert.DeserializeObject<Conversation>(serialisedConversation.ReadToEnd());
                Assert.AreEqual(1, savedConversation.Messages.Count());
            }
        }

        [TestMethod]
        public void TestUserObfuscation()
        {
            ProgramOptions options = ProgramOptionsBasic();
            options.ObfuscateUserIDs = true;

            ConversationExporter exporter = new ConversationExporter();
            bool exportResult = exporter.ExportConversation(options);
            Assert.IsTrue(exportResult);

            using (var serialisedConversation = new StreamReader("chat.json"))
            {
                var savedConversation = JsonConvert.DeserializeObject<Conversation>(serialisedConversation.ReadToEnd());
                Assert.AreEqual(7, savedConversation.Messages.Count());

                Assert.IsTrue(savedConversation.Messages.All(m => m.Sender.All(c => char.IsDigit(c))));
            }
        }

        [TestMethod]
        public void TestReportGeneration()
        {
            ProgramOptions options = ProgramOptionsBasic();
            options.GenerateReport = true;

            ConversationExporter exporter = new ConversationExporter();
            bool exportResult = exporter.ExportConversation(options);
            Assert.IsTrue(exportResult);

            using (var serialisedConversation = new StreamReader("chat.json"))
            {
                var savedConversation = JsonConvert.DeserializeObject<Conversation>(serialisedConversation.ReadToEnd());
                Assert.IsTrue(savedConversation.ReportMostActiveUser == "bob");
                //Test it's sorted by who has the most activity (i.e descending by message count)
                Assert.IsTrue(savedConversation.UserMessageCount.First().Key == "bob");
            }

        }

        [TestMethod]
        public void TestFailWhenNoOptions()
        {
            ConversationExporter exporter = new ConversationExporter();
            Assert.IsFalse(exporter.ExportConversation(null));
        }

        [TestMethod]
        public void TestFailBadFileInputPath()
        {
            ProgramOptions options = ProgramOptionsBasic();
            options.InputFile = "qwerty.txt";
            ConversationExporter exporter = new ConversationExporter();
            Assert.IsFalse(exporter.ExportConversation(options));
        }

        [TestMethod]
        public void TestFailBadFileOutputPath()
        {
            ProgramOptions options = ProgramOptionsBasic();
            options.OutputFile = "\\ThisDirectoryIsntReal\\asdf.json";
            ConversationExporter exporter = new ConversationExporter();
            Assert.IsFalse(exporter.ExportConversation(options));
        }
    }
}
