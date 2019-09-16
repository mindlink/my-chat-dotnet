using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using MyChat;

namespace MindLink.Recruitment.MyChat.Tests
{
    [TestClass]
    public class ConversationTests
    {
        public Conversation readConversation()
        {
            return new ConversationExporter().ReadConversation("chat.txt");
        }

        [TestMethod]
        public void TestMessageFilterByUser()
        {
            Conversation conversation = readConversation();
            Conversation filteredConv = conversation.FilterByUser("bob");

            //Test conversation name is correct
            Assert.AreEqual("My Conversation-senderId:bob", filteredConv.name);

            List<Message> messages = (List<Message>)filteredConv.messages;

            //Test there are correct amount of messages
            Assert.AreEqual(3, messages.Count);

            //Test messages are correct
            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470901), messages[0].timestamp);
            Assert.AreEqual("bob", messages[0].senderId);
            Assert.AreEqual("Hello there!", messages[0].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470906), messages[1].timestamp);
            Assert.AreEqual("bob", messages[1].senderId);
            Assert.AreEqual("I'm good thanks, do you like pie?", messages[1].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[2].timestamp);
            Assert.AreEqual("bob", messages[2].senderId);
            Assert.AreEqual("No, just want to know if there's anybody else in the pie society...", messages[2].content);
        }

        [TestMethod]
        public void TestMessageFilterByKeyword()
        {
            Conversation conversation = readConversation();
            //change pie to Pie to test case ignoring
            ((List<Message>)conversation.messages)[2].content = "I'm good thanks, do you like Pie?";

            Conversation filteredConv = conversation.FilterByKeyword("pie");

            //Test conversation name is correct
            Assert.AreEqual("My Conversation-keyword:pie", filteredConv.name);

            List<Message> messages = (List<Message>)filteredConv.messages;

            //Test there are correct amount of messages
            Assert.AreEqual(4, messages.Count);

            //Test messages are correct
            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470906), messages[0].timestamp);
            Assert.AreEqual("bob", messages[0].senderId);
            Assert.AreEqual("I'm good thanks, do you like Pie?", messages[0].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470912), messages[1].timestamp);
            Assert.AreEqual("angus", messages[1].senderId);
            Assert.AreEqual("Hell yes! Are we buying some pie?", messages[1].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[2].timestamp);
            Assert.AreEqual("bob", messages[2].senderId);
            Assert.AreEqual("No, just want to know if there's anybody else in the pie society...", messages[2].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470915), messages[3].timestamp);
            Assert.AreEqual("angus", messages[3].senderId);
            Assert.AreEqual("YES! I'm the head pie eater there...", messages[3].content);
        }

        [TestMethod]
        public void TestBlacklistWord()
        {
            Conversation conversation = readConversation();
            //change pie to Pie to test case ignoring
            ((List<Message>)conversation.messages)[2].content = "I'm good thanks, do you like Pie?";

            Conversation blacklisted = conversation.BlacklistWord("pie");

            //Test conversation name is correct
            Assert.AreEqual("My Conversation", blacklisted.name);

            List<Message> messages = (List<Message>)blacklisted.messages;

            //Test there are correct amount of messages
            Assert.AreEqual(7, messages.Count);

            //Test messages are correct
            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470901), messages[0].timestamp);
            Assert.AreEqual("bob", messages[0].senderId);
            Assert.AreEqual("Hello there!", messages[0].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470905), messages[1].timestamp);
            Assert.AreEqual("mike", messages[1].senderId);
            Assert.AreEqual("how are you?", messages[1].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470906), messages[2].timestamp);
            Assert.AreEqual("bob", messages[2].senderId);
            Assert.AreEqual("I'm good thanks, do you like *redacted*?", messages[2].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470910), messages[3].timestamp);
            Assert.AreEqual("mike", messages[3].senderId);
            Assert.AreEqual("no, let me ask Angus...", messages[3].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470912), messages[4].timestamp);
            Assert.AreEqual("angus", messages[4].senderId);
            Assert.AreEqual("Hell yes! Are we buying some *redacted*?", messages[4].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[5].timestamp);
            Assert.AreEqual("bob", messages[5].senderId);
            Assert.AreEqual("No, just want to know if there's anybody else in the *redacted* society...", messages[5].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470915), messages[6].timestamp);
            Assert.AreEqual("angus", messages[6].senderId);
            Assert.AreEqual("YES! I'm the head *redacted* eater there...", messages[6].content);
        }

        [TestMethod]
        public void TestBlacklistNumber()
        {
            Message message1 = new Message(DateTimeOffset.FromUnixTimeSeconds(1448470901), "bob", "My phone number is +447248833902");
            Message message2 = new Message(DateTimeOffset.FromUnixTimeSeconds(1448470907), "dave", "My credit card number is 2938475858584939.");
            Message message3 = new Message(DateTimeOffset.FromUnixTimeSeconds(1448470912), "roger", "This is not my phone number: 07336621783, is your credit card number really 2938475858584939?");

            List<Message> messages = new List<Message>();
            messages.Add(message1);
            messages.Add(message2);
            messages.Add(message3);

            Conversation conversation = new Conversation("Number discussion", messages);

            Conversation newConversation = conversation.BlacklistPhoneAndCC();

            //Test conversation name is correct
            Assert.AreEqual("Number discussion", newConversation.name);

            messages = (List<Message>)newConversation.messages;

            //Test there is correct amount of messages
            Assert.AreEqual(3, messages.Count);

            //Test messages are correct
            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470901), messages[0].timestamp);
            Assert.AreEqual("bob", messages[0].senderId);
            Assert.AreEqual("My phone number is *redacted*", messages[0].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470907), messages[1].timestamp);
            Assert.AreEqual("dave", messages[1].senderId);
            Assert.AreEqual("My credit card number is *redacted*.", messages[01].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470912), messages[2].timestamp);
            Assert.AreEqual("roger", messages[2].senderId);
            Assert.AreEqual("This is not my phone number: *redacted*, is your credit card number really *redacted*?", messages[2].content);

        }

        [TestMethod]
        public void TestObfuscation()
        {
            Conversation conversation = readConversation();
            Conversation obfuscated = conversation.ObfuscateUserIds();

            //Test conversation name is correct
            Assert.AreEqual("My Conversation", obfuscated.name);

            List<Message> messages = (List<Message>)obfuscated.messages;

            //Test there are correct amount of messages
            Assert.AreEqual(7, messages.Count);

            //Test messages are correct
            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470901), messages[0].timestamp);
            Assert.AreEqual("0", messages[0].senderId);
            Assert.AreEqual("Hello there!", messages[0].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470905), messages[1].timestamp);
            Assert.AreEqual("1", messages[1].senderId);
            Assert.AreEqual("how are you?", messages[1].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470906), messages[2].timestamp);
            Assert.AreEqual("0", messages[2].senderId);
            Assert.AreEqual("I'm good thanks, do you like pie?", messages[2].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470910), messages[3].timestamp);
            Assert.AreEqual("1", messages[3].senderId);
            Assert.AreEqual("no, let me ask Angus...", messages[3].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470912), messages[4].timestamp);
            Assert.AreEqual("2", messages[4].senderId);
            Assert.AreEqual("Hell yes! Are we buying some pie?", messages[4].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[5].timestamp);
            Assert.AreEqual("0", messages[5].senderId);
            Assert.AreEqual("No, just want to know if there's anybody else in the pie society...", messages[5].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470915), messages[6].timestamp);
            Assert.AreEqual("2", messages[6].senderId);
            Assert.AreEqual("YES! I'm the head pie eater there...", messages[6].content);
        }
    }
}
