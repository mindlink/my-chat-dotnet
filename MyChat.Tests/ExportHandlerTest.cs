using System.IO;
using System.Collections.Generic;
using System.Linq;
using MyChat;
using Newtonsoft.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MindLink.Recruitment.MyChat.Tests
{
    [TestClass]
    public class ExportHandlerTest
    {
        /// <summary>
        /// We added two random words to the black list and exported 
        /// the conversation to test the result
        /// </summary>
        [TestMethod]
        public void RedactMessagesTest()
        {
            ConversationExporter exporter = new ConversationExporter();
            ExportCreteria exportCreteria = new ExportCreteria("chat.txt", "output2.json");
            exportCreteria.SetBlackListItems(new List<string>() { "how", "let" });

            ExportHandler.ExportConversation(exportCreteria);

            var serializedConversation = new StreamReader(new FileStream("output2.json", FileMode.Open)).ReadToEnd();
            Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);
            var messages = savedConversation.Messages.ToList();

            Assert.AreEqual("My Conversation", savedConversation.Name);
            Assert.AreEqual(System.DateTimeOffset.FromUnixTimeSeconds(1448470901), messages[0].Timestamp);
            Assert.AreEqual("bob", messages[0].SenderId);
            Assert.AreEqual("Hello there!", messages[0].Content);

            Assert.AreEqual(System.DateTimeOffset.FromUnixTimeSeconds(1448470905), messages[1].Timestamp);
            Assert.AreEqual("mike", messages[1].SenderId);
            Assert.AreEqual("\\*redacted\\* are you?", messages[1].Content);

            Assert.AreEqual(System.DateTimeOffset.FromUnixTimeSeconds(1448470906), messages[2].Timestamp);
            Assert.AreEqual("bob", messages[2].SenderId);
            Assert.AreEqual("I'm good thanks, do you like pie?", messages[2].Content);

            Assert.AreEqual(System.DateTimeOffset.FromUnixTimeSeconds(1448470910), messages[3].Timestamp);
            Assert.AreEqual("mike", messages[3].SenderId);
            Assert.AreEqual("no, \\*redacted\\* me ask Angus...", messages[3].Content);

            Assert.AreEqual(System.DateTimeOffset.FromUnixTimeSeconds(1448470912), messages[4].Timestamp);
            Assert.AreEqual("angus", messages[4].SenderId);
            Assert.AreEqual("Hell yes! Are we buying some pie?", messages[4].Content);

            Assert.AreEqual(System.DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[5].Timestamp);
            Assert.AreEqual("bob", messages[5].SenderId);
            Assert.AreEqual("No, just want to know if there's anybody else in the pie society...", messages[5].Content);

            Assert.AreEqual(System.DateTimeOffset.FromUnixTimeSeconds(1448470915), messages[6].Timestamp);
            Assert.AreEqual("angus", messages[6].SenderId);
            Assert.AreEqual("YES! I'm the head pie eater there...", messages[6].Content);

        }

        /// <summary>
        /// Test for the ReturnUserList
        /// </summary>
        [TestMethod]
        public void ReturnUserListTest()
        {
            ConversationExporter exporter = new ConversationExporter();
            ExportCreteria exportCreteria = new ExportCreteria("chat.txt", "output2.json");
            ExportHandler.ExportConversation(exportCreteria);

            var serializedConversation = new StreamReader(new FileStream("output2.json", FileMode.Open)).ReadToEnd();
            Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);
 
            List<User> userList = ExportHandler.ReturnUserList(savedConversation);

            Assert.AreEqual(userList[0].Name, "bob");
            Assert.AreEqual(userList[1].Name, "mike");
            Assert.AreEqual(userList[2].Name, "angus");
            Assert.AreEqual(userList[0].NumberOfMessages, 3);
            Assert.AreEqual(userList[1].NumberOfMessages, 2);
            Assert.AreEqual(userList[2].NumberOfMessages, 2);
        }
    }
}
