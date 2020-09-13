using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Xunit;

namespace MindLink.Recruitment.MyChat.Tests
{
    /// <summary>
    /// Run tests on the ConversationFilter class.
    /// </summary>
    public class ConversationFilterTests
    {

        ConversationReader reader = new ConversationReader();
        ConversationWriter writer = new ConversationWriter();
        

        /// <summary>
        /// Check if the correct number of messages is returned.
        /// </summary>
        [Fact]
        public void MessagesFilterByName()
        {
            // Arrange
            var args = new string[] { "chat.txt", "out.json", "name", "bob" };
            
            var configuration = new CLAParser().ParseCommandLineArguments(args);
            var conversation = reader.ReadConversation(configuration);
            ConversationFilter filter = new ConversationFilter(configuration, conversation);
            conversation = filter.newConversation;
            
            writer.WriteConversation(conversation, configuration.OutputFilePath);
            // Act
            var serializedConversation = new StreamReader(new FileStream("out.json", FileMode.Open)).ReadToEnd();
            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);
            var messages = savedConversation.Messages.ToList();

            // Assert
            Assert.Equal(4, messages.Count);
        }
        /// <summary>
        /// Checks if the correct number of messages is returned when filtered by a keyword.
        /// </summary>
        [Fact]
        public void MessagesFilterByWord()
        {
            // Arrange
            var args = new string[] { "chat.txt", "out2.json", "word", "here" };

            var configuration = new CLAParser().ParseCommandLineArguments(args);
            var conversation = reader.ReadConversation(configuration);
            ConversationFilter filter = new ConversationFilter(configuration, conversation);
            //conversation = filter.newConversation;
            writer.WriteConversation(filter.newConversation, configuration.OutputFilePath);
            // Act
            var serializedConversation = new StreamReader(new FileStream("out2.json", FileMode.Open)).ReadToEnd();
            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);
            var messages = savedConversation.Messages.ToList();

            // Assert
            Assert.Equal(3, messages.Count);
        }
        /// <summary>
        /// Checks if the correct words are *redacted* using a blacklist
        /// </summary>
        [Fact]
        public void MessageIsRedacted()
        {
            // Arrange
            var args = new string[] { "chat.txt", "out3.json", "blacklist", "blacklist.txt" };

            var configuration = new CLAParser().ParseCommandLineArguments(args);
            var conversation = reader.ReadConversation(configuration);
            ConversationFilter filter = new ConversationFilter(configuration, conversation);
            writer.WriteConversation(filter.newConversation, configuration.OutputFilePath);
            // Act
            var serializedConversation = new StreamReader(new FileStream("out3.json", FileMode.Open)).ReadToEnd();
            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);
            var mesasges = savedConversation.Messages.ToList();

            // Assert
            Assert.Equal("*redacted* *redacted*", mesasges[0].Content);
        }
    }
}
