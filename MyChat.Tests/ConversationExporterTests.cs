using System.IO;
using System.Linq;
using MyChat;
using Newtonsoft.Json;

namespace MindLink.Recruitment.MyChat.Tests
{
    using System;
    using Xunit;

    /// <summary>
    /// Tests for the <see cref="ConversationExporter"/>.
    /// </summary>
 
    public class ConversationExporterTests
    {

        ConversationExporterConfiguration configuration;
        ConversationExporter exporter;
        Conversation savedConversation;

        ConversationModifier modifier;

        string serializedConversation;

        public ConversationExporterTests()
        {
            exporter = new ConversationExporter();
            configuration = new ConversationExporterConfiguration("chat.txt", "chat.json");

            exporter.ExportConversation(configuration);

            serializedConversation = new StreamReader(new FileStream("chat.json", FileMode.Open)).ReadToEnd();
            savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

        }

        /// <summary>
        /// Tests that exporting the conversation exports conversation.
        /// </summary>
        /// 
        [Fact]
        public void Test_CheckConversationName()
        {
            Assert.Equal("My Conversation", savedConversation.name);

        }

        [Fact]
        public void Test_CheckMessages()
        {
            var messages = savedConversation.messages.ToList();

            Assert.Equal(DateTimeOffset.FromUnixTimeSeconds(1448470901), messages[0].timestamp);
            Assert.Equal("bob", messages[0].senderId);
            Assert.Equal("Hello there!", messages[0].content);
            
            Assert.Equal(DateTimeOffset.FromUnixTimeSeconds(1448470905), messages[1].timestamp);
            Assert.Equal("mike", messages[1].senderId);
            Assert.Equal("how are you?", messages[1].content);

            Assert.Equal(DateTimeOffset.FromUnixTimeSeconds(1448470906), messages[2].timestamp);
            Assert.Equal("bob", messages[2].senderId);
            Assert.Equal("I'm good thanks, do you like pie?", messages[2].content);

            Assert.Equal(DateTimeOffset.FromUnixTimeSeconds(1448470910), messages[3].timestamp);
            Assert.Equal("mike", messages[3].senderId);
            Assert.Equal("no, let me ask Angus...", messages[3].content);

            Assert.Equal(DateTimeOffset.FromUnixTimeSeconds(1448470912), messages[4].timestamp);
            Assert.Equal("angus", messages[4].senderId);
            Assert.Equal("Hell yes! Are we buying some pie?", messages[4].content);

            Assert.Equal(DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[5].timestamp);
            Assert.Equal("bob", messages[5].senderId);
            Assert.Equal("No, just want to know if there's anybody else in the pie society...", messages[5].content);

            Assert.Equal(DateTimeOffset.FromUnixTimeSeconds(1448470915), messages[6].timestamp);
            Assert.Equal("angus", messages[6].senderId);
            Assert.Equal("YES! I'm the head pie eater there...", messages[6].content); 
        }

        [Fact]
        public void Test_CheckLineValidator()
        {

            Assert.False(exporter.LineValidator("48464655 matas hello pie ".Split(' ')));
            Assert.False(exporter.LineValidator("5555 hi".Split(' ')));
            Assert.False(exporter.LineValidator(" ".Split(' ')));
            Assert.False(exporter.LineValidator("9999 matas".Split(' ')));

        }

        [Fact]

        public void Test_Modifier_ModifyByName()
        {
            configuration = new ConversationExporterConfiguration("chat.txt", "chat.json");
            configuration.user = "matas";
            
            exporter.ExportConversation(configuration);

            serializedConversation = new StreamReader(new FileStream("chat.json", FileMode.Open)).ReadToEnd();
            savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            var messages = savedConversation.messages.ToList();

            Assert.Equal("Hello am I late ?", messages[0].content);
            Assert.Equal("I like pie", messages[1].content);
            


        }







    }
}