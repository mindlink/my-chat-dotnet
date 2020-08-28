namespace MindLink.Recruitment.MyChat.Tests
{
    using System.IO;
    using System.Linq;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using System;
    using MindLink.Recruitment.MyChat;

    /// <summary>
    /// Tests for the <see cref="ConversationFilter"/>.
    /// </summary>
    [TestFixture]
    class ConversationFilterTest
    {
        [Test]
        public void ConversationFilterKeyword()
        {
            var exporter = new ConversationExporter();
            var conversationFilter = new ConversationFilter();

            exporter.ExportConversation("chat_censor.txt", "chat_censor.json", "BiLl", "no", "no", "no", conversationFilter.FilterKeyword);

            var serializedConversation = new StreamReader(new FileStream("chat_censor.json", FileMode.Open)).ReadToEnd();

            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.That(savedConversation.name, Is.EqualTo("My Conversation"));

            var messages = savedConversation.messages.ToList();

            Assert.That(messages[0].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470908)));
            Assert.That(messages[0].senderId, Is.EqualTo("jimmy"));
            Assert.That(messages[0].content, Is.EqualTo("I'm cool thanks, just checking if you still want those Bill Burr tickets?"));
        }

        [Test]
        public void ConversationFilterUser()
        {
            var exporter = new ConversationExporter();
            var conversationFilter = new ConversationFilter();

            exporter.ExportConversation("chat_censor.txt", "chat_censor.json", "chatBoT", "no", "no", "no", conversationFilter.FilterUser);

            var serializedConversation = new StreamReader(new FileStream("chat_censor.json", FileMode.Open)).ReadToEnd();

            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.That(savedConversation.name, Is.EqualTo("My Conversation"));

            var messages = savedConversation.messages.ToList();

            Assert.That(messages[0].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470916)));
            Assert.That(messages[0].senderId, Is.EqualTo("chatbot"));
            Assert.That(messages[0].content, Is.EqualTo("Recipient could not be found"));

            Assert.That(messages[1].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470917)));
            Assert.That(messages[1].senderId, Is.EqualTo("chatbot"));
            Assert.That(messages[1].content, Is.EqualTo("Recipient could not be found"));

            Assert.That(messages[2].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470917)));
            Assert.That(messages[2].senderId, Is.EqualTo("chatbot"));
            Assert.That(messages[2].content, Is.EqualTo("Recipient could not be found"));
        }
    }
}
