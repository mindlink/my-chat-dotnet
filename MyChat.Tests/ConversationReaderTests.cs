using System.Linq;
using Xunit;
using System;

namespace MindLink.Recruitment.MyChat.Tests
{
    public class ConversationReaderTests
    {
        [Fact]
        public void ConversationObjectsLoaded()
        {
            // Arrange
            var reader = new ConversationReader();
            var configuration = new ConversationExporterConfiguration();

            // Act
            configuration.InputFilePath = "chat.txt";
            configuration.OutputFilePath = "chat.json";
            var conversation = reader.ReadConversation(configuration);

            // Assert
            Assert.Equal(10, conversation.Messages.ToList().Count);
        }
        [Fact]
        public void ReadConversationLine()
        {
            // Arrange
            var reader = new ConversationReader();
            var configuration = new ConversationExporterConfiguration();

            // Act
            configuration.InputFilePath = "chat.txt";
            configuration.OutputFilePath = "chat.json";
            var conversation = reader.ReadConversation(configuration);

            // Assert
            Assert.Equal("My Conversation", conversation.Name);
        }
        [Fact]
        public void CatchFileNotFound()
        {
            // Arrange
            var reader = new ConversationReader();
            var configuration = new ConversationExporterConfiguration();

            // Act
            configuration.InputFilePath = "fake.txt";
            configuration.OutputFilePath = "chat.json";
            
            // Assert
            var ex = Assert.Throws<ArgumentException>(() => reader.ReadConversation(configuration));
            Assert.Equal("Input file not found.", ex.Message);
        }
        [Fact]
        public void CatchBadFileLoad()
        {
            // Arrange
            var reader = new ConversationReader();
            var configuration = new ConversationExporterConfiguration();

            // Act
            configuration.InputFilePath = "CLAParserTests.cs";
            configuration.OutputFilePath = "chat.json";
            
            // Assert
            Assert.Throws<ArgumentException>(() => reader.ReadConversation(configuration));
        }
    }
}
