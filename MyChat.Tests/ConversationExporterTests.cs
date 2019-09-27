using System.IO;
using System.Linq;
using MyChat;
using Newtonsoft.Json;
using System.Collections.Generic;

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

        }

        /// <summary>
        /// Tests that exporting the conversation exports conversation.
        /// </summary>
        /// 
        [Fact]
        public void Test_CheckConversationName()
        {
            var output = "chat5.json";

            var exporter = new ConversationExporter();
            var configuration = new ConversationExporterConfiguration("chat.txt", output);
            configuration.writeUserActivity = false;

            exporter.ExportConversation(configuration);

            var serializedConversation = new StreamReader(new FileStream(output, FileMode.Open)).ReadToEnd();
            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.Equal("My Conversation", savedConversation.name);
        }

        [Fact]
        public void Test_CheckMessages()
        {

            var output = "chat0.json";

            var exporter = new ConversationExporter();
            var configuration = new ConversationExporterConfiguration("chat.txt", output);
            configuration.writeUserActivity = false;


            exporter.ExportConversation(configuration);

            var serializedConversation = new StreamReader(new FileStream(output, FileMode.Open)).ReadToEnd();
            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

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

            var exporter = new ConversationExporter();

            Assert.True(exporter.LineValidator("48464655 matas hello pie ".Split(' ')));
            Assert.False(exporter.LineValidator("5555 hi".Split(' ')));
            Assert.False(exporter.LineValidator("5555 hi ".Split(' ')));
            Assert.False(exporter.LineValidator(" ".Split(' ')));
            Assert.False(exporter.LineValidator("9999 matas".Split(' ')));

        }

        [Fact]

        public void Test_Modifier_ModifyByName()
        {
            var output = "chat1.json";

            var exporter = new ConversationExporter();
            var configuration = new ConversationExporterConfiguration("chat.txt", output);
            configuration.user = "matas";
            configuration.writeUserActivity = false;

            exporter.ExportConversation(configuration);

            var serializedConversation = new StreamReader(new FileStream(output, FileMode.Open)).ReadToEnd();
            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            var messages = savedConversation.messages.ToList();

            Assert.Equal("Hello am I late?", messages[0].content);
            Assert.Equal("I like pie", messages[1].content);
            
        }

        [Fact]
        public void Test_Modifier_ModifyByKeyword()
        {
            var output = "chat2.json";

            var exporter = new ConversationExporter();
            var configuration = new ConversationExporterConfiguration("chat.txt", output);
            configuration.keyword = "pie";
            configuration.writeUserActivity = false;

            exporter.ExportConversation(configuration);

            var serializedConversation = new StreamReader(new FileStream(output, FileMode.Open)).ReadToEnd();
            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            var messages = savedConversation.messages.ToList();

            Assert.Equal("I'm good thanks, do you like pie?", messages[0].content);
            Assert.Equal("Hell yes! Are we buying some pie?", messages[1].content);
            Assert.Equal("No, just want to know if there's anybody else in the pie society...", messages[2].content);
            Assert.Equal("YES! I'm the head pie eater there...", messages[3].content);
            Assert.Equal("I like pie", messages[4].content);
               
        }

        [Fact]
        public void Test_Modifier_ModifyByBlacklist()
        {
            var output = "chat3.json";

            var exporter = new ConversationExporter();
            var configuration = new ConversationExporterConfiguration("chat.txt", output);
            configuration.blacklist = new List<string>();

            configuration.blacklist.Add("like");
            configuration.blacklist.Add("pie");

            configuration.writeUserActivity = false;

            exporter.ExportConversation(configuration);

            var serializedConversation = new StreamReader(new FileStream(output, FileMode.Open)).ReadToEnd();
            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);
            
            var messages = savedConversation.messages.ToList();

            Assert.Equal("Hello there!", messages[0].content);
            Assert.Equal("I'm good thanks, do you \\*redacted*\\ \\*redacted*\\?", messages[2].content);          
            Assert.Equal("Hell yes! Are we buying some \\*redacted*\\?", messages[4].content);
            Assert.Equal("No, just want to know if there's anybody else in the \\*redacted*\\ society...", messages[5].content);
            Assert.Equal("YES! I'm the head \\*redacted*\\ eater there...", messages[6].content);
            Assert.Equal("I \\*redacted*\\ \\*redacted*\\", messages[8].content);
            Assert.Equal("I mean what;s not to \\*redacted*\\ about \\*redacted*\\", messages[9].content);

        }

        [Fact]
        public void Test_Modifier_ModifyBySensitiveData()
        {
            var output = "chat6.json";

            var exporter = new ConversationExporter();
            var configuration = new ConversationExporterConfiguration("chat.txt", output);
            configuration.hideSensitiveData = true;
            configuration.writeUserActivity = false;


            exporter.ExportConversation(configuration);

            var serializedConversation = new StreamReader(new FileStream(output, FileMode.Open)).ReadToEnd();
            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            var messages = savedConversation.messages.ToList();

            Assert.Equal("Hello there!", messages[0].content); 
            Assert.Equal("hello my credit card number is \\*redacted*\\", messages[10].content);
            Assert.Equal("Hello my phone number is \\*redacted*\\", messages[11].content);            
        }

        [Fact]

        public void Test_Modifier_Obfisticate_IDS()
        {
            var output = "chat7.json";

            var exporter = new ConversationExporter();
            var configuration = new ConversationExporterConfiguration("chat.txt", output);
            configuration.obfuscateUserIDs = true;
            configuration.writeUserActivity = false;

            exporter.ExportConversation(configuration);

            var serializedConversation = new StreamReader(new FileStream(output, FileMode.Open)).ReadToEnd();
            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            var messages = savedConversation.messages.ToList();


            //bob mike angus matas
            Assert.Equal("user1", messages[0].senderId);
            Assert.Equal("user2", messages[1].senderId);
            Assert.Equal("user3", messages[4].senderId);
            Assert.Equal("user4", messages[7].senderId);

        }

        [Fact]
        public void Test_Exporter_CalculateActivity()
        {
            var output = "chat8.json";

            var exporter = new ConversationExporter();
            var configuration = new ConversationExporterConfiguration("chat.txt", output);
            configuration.obfuscateUserIDs = false;
            configuration.writeUserActivity = false;

            exporter.ExportConversation(configuration);

            var serializedConversation = new StreamReader(new FileStream(output, FileMode.Open)).ReadToEnd();
            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            var messages = savedConversation.messages.ToList();

            var userActivityList = exporter.CalculateActivity(savedConversation);

            Assert.Equal(5, userActivityList.First(x => x.userID == "matas").messageCount);
            Assert.Equal(1, userActivityList.First(x => x.userID == "chris").messageCount);
            Assert.Equal(1, userActivityList.First(x => x.userID == "thomas").messageCount);
            Assert.Equal(2, userActivityList.First(x => x.userID == "angus").messageCount);
            Assert.Equal(2, userActivityList.First(x => x.userID == "mike").messageCount);
            Assert.Equal(3, userActivityList.First(x => x.userID == "bob").messageCount);

        }







    }
}