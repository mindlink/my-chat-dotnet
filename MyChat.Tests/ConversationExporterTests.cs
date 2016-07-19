using System.IO;
using System.Linq;
using MyChat;
using Newtonsoft.Json;

namespace MindLink.Recruitment.MyChat.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for the <see cref="ConversationExporter"/>.
    /// </summary>
    [TestClass]
    public class ConversationExporterTests
    {

        /// <summary>
        /// Method that tests if the ReadConversation method reads the conversation correct from text file
        /// </summary>
        [TestMethod]
        public void ReadConversationTest()
        {
            ConversationExporter exporter = new ConversationExporter();
            ConversationExporterConfiguration configuration = new ConversationExporterConfiguration("chat.txt", "chat.json");
            Conversation conversation = exporter.ReadConversation(configuration);

            Assert.AreEqual("My Conversation", conversation.name);

            var messages = conversation.messages.ToList();

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470901), messages[0].timestamp);
            Assert.AreEqual("bob", messages[0].senderId);
            Assert.AreEqual("Hello there!", messages[0].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470905), messages[1].timestamp);
            Assert.AreEqual("mike", messages[1].senderId);
            Assert.AreEqual("how are you?", messages[1].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470906), messages[2].timestamp);
            Assert.AreEqual("bob", messages[2].senderId);
            Assert.AreEqual("I'm good thanks, do you like pie?", messages[2].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470910), messages[3].timestamp);
            Assert.AreEqual("mike", messages[3].senderId);
            Assert.AreEqual("no, let me ask Angus...", messages[3].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470912), messages[4].timestamp);
            Assert.AreEqual("angus", messages[4].senderId);
            Assert.AreEqual("Hell yes! Are we buying some pie?", messages[4].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[5].timestamp);
            Assert.AreEqual("bob", messages[5].senderId);
            Assert.AreEqual("No, just want to know if there's anybody else in the pie society...", messages[5].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470915), messages[6].timestamp);
            Assert.AreEqual("angus", messages[6].senderId);
            Assert.AreEqual("YES! I'm the head pie eater there...", messages[6].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470918), messages[7].timestamp);
            Assert.AreEqual("angus", messages[7].senderId);
            Assert.AreEqual("", messages[7].content);

        }

        /// <summary>
        /// Checks how app reacts if a wrong file is given for conversation
        /// </summary>
        [TestMethod]
        public void FailToReadConversation()
        {
            ConversationExporter exporter = new ConversationExporter();
            ConversationExporterConfiguration configuration = new ConversationExporterConfiguration(null, "chat.json");
            try
            {
                exporter.ReadConversation(configuration);
            }
            catch (Exception)
            {
                configuration.inputFilePath="chat.txt";
                exporter.ReadConversation(configuration);
            }

        }

        /// <summary>
        /// Checks how app reacts if a file that cannot write on is given
        /// </summary>
        [TestMethod]
        public void FailToExport()
        {
            ConversationExporter exporter = new ConversationExporter();
            ConversationExporterConfiguration configuration = new ConversationExporterConfiguration("chat.txt", "fileNotAllowedToAccess.json");
            try
            {
                exporter.ExportConversation(configuration);
            }
            catch (Exception)
            {
                //catching the exception and correcting the file
                configuration.outputFilePath = "chat.json";
                exporter.ExportConversation(configuration);
            }
        }


        /// <summary>
        /// Checks if the username filter works properly
        /// </summary>
        [TestMethod]
        public void CheckUsername()
        {
            ConversationExporter exporter = new ConversationExporter();
            ConversationExporterConfiguration configuration = new ConversationExporterConfiguration("chat.txt", "chat.json");
            configuration.usernameFilter = "bob";
            configuration.filtersActive = true;
            Conversation conversation = exporter.ReadConversation(configuration);
            var messages = conversation.messages.ToList();

            foreach (Message msg in messages)
            {
                Assert.AreEqual(configuration.usernameFilter, msg.senderId);
            }

        }


        /// <summary>
        /// Checks if the keyword filter works properly
        /// </summary>
        [TestMethod]
        public void CheckKeyword()
        {
            ConversationExporter exporter = new ConversationExporter();
            ConversationExporterConfiguration configuration = new ConversationExporterConfiguration("chat.txt", "chat.json");
            configuration.keyword = "pie";
            configuration.filtersActive = true;
            Conversation conversation = exporter.ReadConversation(configuration);
            var messages = conversation.messages.ToList();

            foreach (Message msg in messages)
            {
                Assert.IsTrue(msg.content.ToLower().Contains(configuration.keyword.ToLower()));
            }

        }

        /// <summary>
        /// Method Compines all the filters that choose which messages to be exported. 
        /// Specific user and keyword filter.
        /// </summary>
        [TestMethod]
        public void CompineUsernameAndKeyWordFilter()
        {
            ConversationExporter exporter = new ConversationExporter();
            ConversationExporterConfiguration configuration = new ConversationExporterConfiguration("chat.txt", "chat.json");
            configuration.keyword = "pie";
            configuration.usernameFilter = "bob";
            configuration.filtersActive = true;
            Conversation conversation = exporter.ReadConversation(configuration);
            var messages = conversation.messages.ToList();
            //given conversation should return only 2 messages
            Assert.IsTrue(messages.Count == 2);
            foreach (Message msg in messages)
            {
                Assert.IsTrue(msg.senderId == "bob");
                Assert.IsTrue(msg.content.ToLower().Contains(configuration.keyword.ToLower()));
            }
            //exporter.ExportConversation(configuration);
        }

        /// <summary>
        /// Checks method for hidding specific words. 
        /// </summary>
        [TestMethod]
        public void HideSpecificWordsCheck()
        {
            ConversationExporter exporter = new ConversationExporter();
            ConversationExporterConfiguration configuration = new ConversationExporterConfiguration("chat.txt", "chat.json");
            configuration.wordsBlacklist = new string[2] { "pie", "hell" };
            configuration.filtersActive = true;

            Conversation conversation = exporter.ReadConversation(configuration);
            var messages = conversation.messages.ToList();

            Assert.AreEqual("*redacted*o there!", messages[0].content);
            Assert.AreEqual("I'm good thanks, do you like *redacted*?", messages[2].content);
            Assert.AreEqual("*redacted* yes! Are we buying some *redacted*?", messages[4].content);
            Assert.AreEqual("No, just want to know if there's anybody else in the *redacted* society...", messages[5].content);
            Assert.AreEqual("YES! I'm the head *redacted* eater there...", messages[6].content);

        }


        /// <summary>
        /// Tests that exporting the conversation exports conversation.
        /// </summary>
        [TestMethod]
        public void ExportingConversationExportsConversation()
        {
            ConversationExporter exporter = new ConversationExporter();
            ConversationExporterConfiguration configuration = new ConversationExporterConfiguration("chat.txt", "chat.json");
            exporter.ExportConversation(configuration);

            var serializedConversation = new StreamReader(new FileStream("chat.json", FileMode.Open)).ReadToEnd();

            Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.AreEqual("My Conversation", savedConversation.name);
            Assert.IsTrue(savedConversation.messages.ToList().Count > 0);

        }

        [TestMethod]
        public void CheckObfuscateUserIDS()
        {
            ConversationExporter exporter = new ConversationExporter();
            ConversationExporterConfiguration configuration = new ConversationExporterConfiguration("chat.txt", "chat.json");
            Conversation conversation = exporter.ReadConversation(configuration);
            var messages = conversation.messages.ToList();

            //messages with obfuscation
            ConversationExporter exporterObfuscate = new ConversationExporter();
            ConversationExporterConfiguration configurationObfuscate = new ConversationExporterConfiguration("chat.txt", "chat.json");
            configurationObfuscate.obfuscateUserIDsFlag = true;
            Conversation obfuscatedConversation = exporterObfuscate.ReadConversation(configurationObfuscate);
            var obfuscatedMsgs = obfuscatedConversation.messages.ToList();

            for(int i =0; i<obfuscatedMsgs.Count; i++)
            {
                Assert.IsTrue(obfuscatedMsgs[i].senderId == configurationObfuscate.usersMapper[messages[i].senderId]);
            }
            //exporterObfuscate.ExportConversation(configurationObfuscate);
        }

        /// <summary>
        /// Applying ALL filters
        /// </summary>
        [TestMethod]
        public void CheckAllFiltersTogether()
        {
            ConversationExporter exporter = new ConversationExporter();
            ConversationExporterConfiguration configuration = new ConversationExporterConfiguration("chat.txt", "chat.json");
            configuration.filtersActive = true;
            configuration.keyword = "society";
            configuration.usernameFilter = "bob";
            configuration.wordsBlacklist = new string[] { "pie" };
            configuration.obfuscateUserIDsFlag = true;
            
            Conversation conversation = exporter.ReadConversation(configuration);
            var messages = conversation.messages.ToList();
            //given conversation should return only 2 messages
            Assert.IsTrue(messages.Count == 1);
            foreach (Message msg in messages)
            {
                Assert.IsTrue(msg.senderId == "User1");
                Assert.IsTrue(msg.content.ToLower().Contains(configuration.keyword.ToLower()));
            }
            //exporter.ExportConversation(configuration);
        }


    }
}
