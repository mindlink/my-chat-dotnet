using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindLink.Recruitment.MyChat.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat.Tests
{
    /// <summary>
    /// Tests for the Elements.
    /// </summary>
    [TestClass]
    public class ElementsTest
    {
        #region Conversation Test 
        /// <summary>
        /// Tests that Conversation constractor trow exception when conversation name is empty
        /// </summary>
        [TestMethod]
        public void ConversationNameNull()
        {
            try
            {
                Conversation conv = new Conversation(" ");
            }
            catch (ArgumentNullException e)
            {
                // assert  
                StringAssert.Contains(e.Message, "Conversation name can not be empty when creating new conversation");
                return;
            }
            Assert.Fail("No exception was thrown.");


        }

        /// <summary>
        /// Tests that addMessage function trow exception when message sendr id is empty
        /// </summary>
        [TestMethod]
        public void AddMessageSenderIdEmpty()
        {
            try
            {
                Conversation conv = new Conversation("test");
                conv.addMessage(new DateTimeOffset(DateTime.Now), " ", "test");
            }
            catch (ArgumentNullException e)
            {
                // assert  
                StringAssert.Contains(e.Message, "SenderId can not be empty when adding a new message to the conversation");
                return;
            }
            Assert.Fail("No exception was thrown.");


        }

        /// <summary>
        /// Tests that addMessage function trow exception when message sendr id is null
        /// </summary>
        [TestMethod]
        public void AddMessageSenderIdNull()
        {
            try
            {
                Conversation conv = new Conversation("test");
                conv.addMessage(new DateTimeOffset(DateTime.Now), null, "test");
            }
            catch (ArgumentNullException e)
            {
                // assert  
                StringAssert.Contains(e.Message, "SenderId can not be empty when adding a new message to the conversation");
                return;
            }
            Assert.Fail("No exception was thrown.");


        }

        /// <summary>
        /// Tests that addMessage function trow exception when message content is null
        /// </summary>
        [TestMethod]
        public void AddMessageContentNull()
        {
            try
            {
                Conversation conv = new Conversation("test");
                conv.addMessage(new DateTimeOffset(DateTime.Now), "test", null);
            }
            catch (ArgumentNullException e)
            {
                // assert  
                StringAssert.Contains(e.Message, "content can not be null when adding a new message to the conversation");
                return;
            }
            Assert.Fail("No exception was thrown.");


        }

        /// <summary>
        /// Tests that addMessage function adds a new msg
        /// </summary>
        [TestMethod]
        public void AddMessageNewMessages()
        {

                Conversation conv = new Conversation("test");
                conv.addMessage(new DateTimeOffset(DateTime.Now), "nik", "test");

            Assert.AreEqual(conv.Messages.Count(x => x.Content.Contains("test")), 1, "Message was not added to the conversation");
            Assert.AreEqual(conv.Messages.First().msgSender.senderID, "nik", "Message was not added to the conversation");

        }


        #endregion

        #region Message Test 

        /// <summary>
        /// Tests that Message constractor trow exception when sender is null
        /// </summary>
        [TestMethod]
        public void MessageSenderNull()
        {
            try
            {
                Message msg = new Message(new DateTimeOffset(DateTime.Now), null, "test");
            }
            catch (ArgumentNullException e)
            {
                // assert  
                StringAssert.Contains(e.Message, "Sender can not be null when creating a new message");
                return;
            }
            Assert.Fail("No exception was thrown.");


        }

        /// <summary>
        /// Tests that Message constractor trow exception when content is null
        /// </summary>
        [TestMethod]
        public void MessageContentNull()
        {
            try
            {
                Message msg = new Message(new DateTimeOffset(DateTime.Now),new Sender("test") , null);
            }
            catch (ArgumentNullException e)
            {
                // assert  
                StringAssert.Contains(e.Message, "content can not be null when creating a new message");
                return;
            }
            Assert.Fail("No exception was thrown.");
        }

        #endregion

        #region Sender Test 

        /// <summary>
        /// Tests that Sender constractor trow exception when sender id is null
        /// </summary>
        [TestMethod]
        public void SenderSenderIdNull()
        {
            try
            {
                Sender sender = new Sender(null);
            }
            catch (ArgumentNullException e)
            {
                // assert  
                StringAssert.Contains(e.Message, "Sender id can not be empty when creating new sender");
                return;
            }
            Assert.Fail("No exception was thrown.");


        }

        /// <summary>
        /// Tests that Sender constractor trow exception when sender id is empty
        /// </summary>
        [TestMethod]
        public void SenderSenderIdEmpty()
        {
            try
            {
                Sender sender = new Sender(" ");
            }
            catch (ArgumentNullException e)
            {
                // assert  
                StringAssert.Contains(e.Message, "Sender id can not be empty when creating new sender");
                return;
            }
            Assert.Fail("No exception was thrown.");


        }

        #endregion
    }
}
