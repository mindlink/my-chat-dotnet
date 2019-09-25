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
        public void ExportingConversationExportsConversation()
        {
            Assert.Equal("My Conversation", savedConversation.name);

        }



    }
}