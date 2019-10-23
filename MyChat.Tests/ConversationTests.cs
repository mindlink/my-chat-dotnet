using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindLink.Recruitment.MyChat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat.Tests
{
    [TestClass()]
    public class ConversationTests
    {
        private List<Message> test_messages;
        private string test_name;

        [TestInitialize]
        public void ConversationTestsSetup()
        {
            this.test_messages = new List<Message>();
            this.test_messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(1571755721), "bob", "Hello everyone this is a test message."));
            this.test_messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(1571759321), "phil", "Hi Bob, I got your message."));
            this.test_messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(1571759921), "bob", "great to hear that Phil!"));
            this.test_messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(1571846321), "jane", "I got your message too Bob"));
            this.test_messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(1571847881), "bob", "thanks for getting back to me Jane"));
            this.test_messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(1571857881), "phil", "By the way my phone number is 01234 123 234"));
            this.test_messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(1571857881), "bob", "great mine is 07654765443, is the correct credit card number 1235234534564567"));
            this.test_messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(1571857881), "jane", "no the correct credit card number is 1234 2345 3456 4567"));

            this.test_name = "Test Conversation";
        }

        [TestMethod]
        public void FilterByUserTests()
        {
            Conversation conversation = new Conversation(this.test_name, this.test_messages);

            conversation.FilterByUser("bob");

            List<Message> conversation_messages = conversation.messages.ToList();

            Assert.AreEqual(this.test_messages[0], conversation_messages[0]);
            Assert.AreEqual(this.test_messages[2], conversation_messages[1]);
            Assert.AreEqual(this.test_messages[4], conversation_messages[2]);

            conversation = new Conversation(this.test_name, this.test_messages);

            conversation.FilterByUser("jane");
            
            conversation_messages = conversation.messages.ToList();

            Assert.AreEqual(this.test_messages[3], conversation_messages[0]);
        }

        [TestMethod]
        public void FilterByKeywordTests()
        {
            Conversation conversation = new Conversation(this.test_name, this.test_messages);

            conversation.FilterByKeyword("message");

            List<Message> conversation_messages = conversation.messages.ToList();
            
            Assert.AreEqual(this.test_messages[0], conversation_messages[0]);
            Assert.AreEqual(this.test_messages[1], conversation_messages[1]);
            Assert.AreEqual(this.test_messages[3], conversation_messages[2]);
        }

        [TestMethod]
        public void FilterBlacklistTests()
        {
            Conversation conversation = new Conversation(this.test_name, this.test_messages);
            conversation.FilterBlacklist(new string[] { "bob", "phil", "jane", "message" });

            List<Message> conversation_messages = conversation.messages.ToList();

            Assert.AreEqual("Hello everyone this is a test *redacted*.", conversation_messages[0].content);
            Assert.AreEqual("Hi *redacted*, I got your *redacted*.", conversation_messages[1].content);
            Assert.AreEqual("great to hear that *redacted*!", conversation_messages[2].content);
            Assert.AreEqual("I got your *redacted* too *redacted*", conversation_messages[3].content);
            Assert.AreEqual("thanks for getting back to me *redacted*", conversation_messages[4].content);
        }

        [TestMethod]
        public void HideCreditCardAndPhoneNumbersTests ()
        {
            Conversation conversation = new Conversation(this.test_name, this.test_messages);
            conversation.HideCreditCardAndPhoneNumbers();

            List<Message> conversation_messages = conversation.messages.ToList();

            Assert.AreEqual("By the way my phone number is *redacted*", conversation_messages[5].content);
            Assert.AreEqual("great mine is *redacted*, is the correct credit card number *redacted*", conversation_messages[6].content);
            Assert.AreEqual("no the correct credit card number is *redacted*", conversation_messages[7].content);
        }
    }
}