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
        /// Tests that exporting the conversation exports conversation.
        /// </summary>
        [TestMethod]
        public void ExportingConversationExportsConversation()
        {

            ExportCreteria exportCreteria = new ExportCreteria("chat.txt", "output2.json");
            ExportHandler.ExportConversation(exportCreteria);

            var serializedConversation = new StreamReader(new FileStream("output2.json", FileMode.Open)).ReadToEnd();

            Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            var messages = savedConversation.Messages.ToList();

            Assert.AreEqual("My Conversation", savedConversation.Name);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470901), messages[0].Timestamp);
            Assert.AreEqual("bob", messages[0].SenderId);
            Assert.AreEqual("Hello there!", messages[0].Content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470905), messages[1].Timestamp);
            Assert.AreEqual("mike", messages[1].SenderId);
            Assert.AreEqual("how are you?", messages[1].Content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470906), messages[2].Timestamp);
            Assert.AreEqual("bob", messages[2].SenderId);
            Assert.AreEqual("I'm good thanks, do you like pie?", messages[2].Content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470910), messages[3].Timestamp);
            Assert.AreEqual("mike", messages[3].SenderId);
            Assert.AreEqual("no, let me ask Angus...", messages[3].Content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470912), messages[4].Timestamp);
            Assert.AreEqual("angus", messages[4].SenderId);
            Assert.AreEqual("Hell yes! Are we buying some pie?", messages[4].Content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[5].Timestamp);
            Assert.AreEqual("bob", messages[5].SenderId);
            Assert.AreEqual("No, just want to know if there's anybody else in the pie society...", messages[5].Content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470915), messages[6].Timestamp);
            Assert.AreEqual("angus", messages[6].SenderId);
            Assert.AreEqual("YES! I'm the head pie eater there...", messages[6].Content);




            //--------------Testing export by user-----------------
            //We randomly selected a user and we export the conversation 
            //part regarding his messages
            //-----------------------------------------------------

            ExportCreteria export_by_user = new ExportCreteria("chat.txt", "output3.json");
            export_by_user.Export_by_User = new User("angus");
            ExportHandler.ExportConversation(export_by_user);


            serializedConversation = new StreamReader(new FileStream("output3.json", FileMode.Open)).ReadToEnd();

            savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.AreEqual("My Conversation", savedConversation.Name);

            messages = savedConversation.Messages.ToList();


            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470912), messages[0].Timestamp);
            Assert.AreEqual("angus", messages[0].SenderId);
            Assert.AreEqual("Hell yes! Are we buying some pie?", messages[0].Content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470915), messages[1].Timestamp);
            Assert.AreEqual("angus", messages[1].SenderId);
            Assert.AreEqual("YES! I'm the head pie eater there...", messages[1].Content);


            //--------------Testing export by user-----------------
            //We randomly selected a user and we export the conversation 
            //part regarding his messages
            //-----------------------------------------------------

            ExportCreteria export_by_keyword = new ExportCreteria("chat.txt", "output4.json");
            export_by_keyword.Export_by_Keyword = "buying";
            ExportHandler.ExportConversation(export_by_keyword);


            serializedConversation = new StreamReader(new FileStream("output4.json", FileMode.Open)).ReadToEnd();

            savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.AreEqual("My Conversation", savedConversation.Name);

            messages = savedConversation.Messages.ToList();
            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470912), messages[0].Timestamp);
            Assert.AreEqual("angus", messages[0].SenderId);
            Assert.AreEqual("Hell yes! Are we buying some pie?", messages[0].Content);


            //--------------Hiding user id's-----------------
            //-----------------------------------------------------

            ExportCreteria hide_user_id = new ExportCreteria("chat.txt", "output5.json");
            hide_user_id.HideUserName = true;

            ExportHandler.ExportConversation(hide_user_id);


            serializedConversation = new StreamReader(new FileStream("output5.json", FileMode.Open)).ReadToEnd();

            savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.AreEqual("My Conversation", savedConversation.Name);

            messages = savedConversation.Messages.ToList();
            Assert.AreEqual((new User("bob")).idHash, messages[0].SenderId);
            Assert.AreEqual((new User("mike")).idHash, messages[1].SenderId);
            Assert.AreEqual((new User("angus")).idHash, messages[4].SenderId);


        }
    }
}
