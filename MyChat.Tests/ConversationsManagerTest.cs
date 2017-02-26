using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindLink.Recruitment.MyChat.Actions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat.Tests
{
    /// <summary>
    /// Tests for the ConversationsManager.
    /// </summary>
    [TestClass]
    public class ConversationsManagerTest
    {
        /// <summary>
        /// Tests that addAction trow exception when action is null
        /// </summary>
        [TestMethod]
        public void addActionActionNull()
        {
            try
            {
                ConversationsManager cmanger = ConversationsManager.GetInstance;
                cmanger.addAction(null);
            }
            catch (ArgumentNullException e)
            {
                // assert  
                StringAssert.Contains(e.Message, "myAction can nto be bull when adding a new action to the action to perform list");
                return;
            }
            Assert.Fail("No exception was thrown.");


        }

        /// <summary>
        /// Tests that addAction adds a new action
        /// </summary>
        [TestMethod]
        public void addActionAction()
        {

                ConversationsManager cmanger = ConversationsManager.GetInstance;
                cmanger.addAction(new CreateReport());

            Assert.AreEqual(cmanger.Actions.First().ActionID, "/r", "Action was not added to the actions to perform list");


        }

        /// <summary>
        /// Tests that ConversationToJson returns converts the conversation to json odject
        /// </summary>
        [TestMethod]
        public void ConversationToJson()
        {

            ConversationsManager cmanger = ConversationsManager.GetInstance;
                cmanger.InputFilePath = "chat.txt";
                cmanger.loadConversation();
                JObject conv = cmanger.converedConversationToJson();

            Assert.AreEqual((string)conv.SelectToken("name"), "My Conversation", "Conversation name was not exported correctly to json object");

            Assert.AreEqual((string)conv.SelectToken("messages[0].senderId"), "bob", "Message's Sender id was not not exported correctly to json object");
            Assert.AreEqual(DateTimeOffset.Parse((string)conv.SelectToken("messages[0].timestamp")), DateTimeOffset.Parse("2015-11-25T17:01:41+00:00"), "Message's Timestamp was not not exported correctly to json object");
            Assert.AreEqual((string)conv.SelectToken("messages[0].content"), "Hello there!", "Message's Timestamp was not not exported correctly to json object");

        }
    }
}
