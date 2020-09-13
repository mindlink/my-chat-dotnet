using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Xunit;

namespace MindLink.Recruitment.MyChat.Tests
{
    /// <summary>
    /// Run tests on the ConversationWriter class.
    /// </summary>
    public class ConversationWriterTests
    {
        ConversationReader reader = new ConversationReader();
        ConversationWriter writer = new ConversationWriter();

        /// <summary>
        /// See if the list extracted from the json file is not null.
        /// </summary>
        [Fact]
        public void JsonFileNotEmpty()
        {
            // Arrange
            var args = new string[] { "chat.txt", "out1.json" };

            var configuration = new CLAParser().ParseCommandLineArguments(args);
            var conversation = reader.ReadConversation(configuration);
            writer.WriteConversation(conversation, configuration.OutputFilePath);

            // Act
            var serializedConversation = new StreamReader(new FileStream("out1.json", FileMode.Open)).ReadToEnd();

            // Assert
            Assert.NotNull(serializedConversation);
        }
        
        /// <summary>
        /// Checks if the written output is formatted correctly.
        /// </summary>
        [Fact]
        public void CorrectMessageFormatWritten()
        {
            // Arrange
            string[] args = { "chat.txt", "chat.json" };
            var parser = new CLAParser();
            var reader = new ConversationReader();
            var writer = new ConversationWriter();
            var configuration = new CLAParser().ParseCommandLineArguments(args);
            var conversation = reader.ReadConversation(configuration);

            // Act
            var filter = new ConversationFilter(configuration, conversation);
            conversation = filter.newConversation;
            writer.WriteConversation(conversation, configuration.OutputFilePath);
            var serializedConversation = new StreamReader(new FileStream("chat.json", FileMode.Open)).ReadToEnd();
            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);
            var messages = savedConversation.Messages.ToList();
            System.Console.WriteLine(messages);

            // Assert
            Assert.Equal(DateTimeOffset.FromUnixTimeSeconds(1448470901), messages[0].Timestamp);
            Assert.Equal("bob", messages[0].SenderId);
            Assert.Equal("Hello there!", messages[0].Content);

            Assert.Equal(DateTimeOffset.FromUnixTimeSeconds(1448470910), messages[3].Timestamp);
            Assert.Equal("mike", messages[3].SenderId);
            Assert.Equal("no, let me ask Angus...", messages[3].Content);

            Assert.Equal(DateTimeOffset.FromUnixTimeSeconds(1448470915), messages[6].Timestamp);
            Assert.Equal("angus", messages[6].SenderId);
            Assert.Equal("YES! I'm the head pie eater there...", messages[6].Content);
        }
    }
}
