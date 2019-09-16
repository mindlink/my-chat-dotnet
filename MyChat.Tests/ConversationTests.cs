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
            Conversation filteredConv = conversation.FilterByKeyword("pie");

            //Test conversation name is correct
            Assert.AreEqual("My Conversation-keyword:pie", filteredConv.name);

            List<Message> messages = (List<Message>)filteredConv.messages;

            //Test there are correct amount of messages
            Assert.AreEqual(4, messages.Count);

            //Test messages are correct
            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470906), messages[0].timestamp);
            Assert.AreEqual("bob", messages[0].senderId);
            Assert.AreEqual("I'm good thanks, do you like pie?", messages[0].content);

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
            conversation.BlacklistWord("pie");

            //Test conversation name is correct
            Assert.AreEqual("My Conversation", conversation.name);

            List<Message> messages = (List<Message>)conversation.messages;

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
    }
}
