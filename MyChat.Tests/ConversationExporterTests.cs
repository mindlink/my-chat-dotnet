using System.IO;
using System.Linq;
using MyChat;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MindLink.Recruitment.MyChat.Tests
{
    using System;

    /// <summary>
    /// Tests for the <see cref="ConversationExporter"/>.
    /// </summary>
    [TestFixture]
    public class ConversationExporterTests
    {
        /// <summary>
        /// Tests that exporting the conversation exports conversation.
        /// </summary>
        [Test]
        public void ExportingConversationExportsConversation()
        {
            var exporter = new ConversationExporter();

            var serializedConversation = new StreamReader(new FileStream("chat.json", FileMode.Open)).ReadToEnd();

            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            var messages = savedConversation.messages.ToList();
            
            
            var blackList = new List<string>();
            blackList.Add("Pie");
            
            ConversationExporterConfiguration configuration = new ConversationExporterConfiguration("chat.txt", "chat.json", "pie");
            
            Assert.That(savedConversation.name, Is.EqualTo("My Conversation"));


        }
    }
}
