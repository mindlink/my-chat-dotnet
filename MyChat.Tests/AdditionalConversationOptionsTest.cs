namespace MindLink.Recruitment.MyChat.Tests
{
    using global::MyChat;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using System;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Tests for the <see cref="AdditionalConversationOptions"/>.
    /// </summary>
    [TestFixture]
    public class AdditionalConversationOptionsTest
    {
        /// <summary>
        /// Test on filtering conversation by name
        /// </summary>
        [Test]
        public void FilterConversationByName()
        {
            var exporter = new ConversationExporter();

            var args = new string[2];
            args[0] = "--filterByUser"; args[1] = "bob";

            var configuration = new ConfigurationBuilder().AddCommandLine(args).Build();
            var exporterConfiguration = configuration.Get<ConversationExporterConfiguration>();

            var additionalOptions = new AdditionalConversationOptions(exporterConfiguration);

            exporter.ExportConversation("chat.txt", "chat_nameFilter.json", additionalOptions);

            var serializedConversation = new StreamReader(new FileStream("chat_nameFilter.json", FileMode.Open)).ReadToEnd();

            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.That(savedConversation.name, Is.EqualTo("My Conversation"));

            var messages = savedConversation.messages.ToList();

            Assert.That(messages[0].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470901)));
            Assert.That(messages[0].senderId, Is.EqualTo("bob"));
            Assert.That(messages[0].content, Is.EqualTo("Hello there!"));

            Assert.That(messages[1].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470906)));
            Assert.That(messages[1].senderId, Is.EqualTo("bob"));
            Assert.That(messages[1].content, Is.EqualTo("I'm good thanks, do you like pie?"));

            Assert.That(messages[2].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470914)));
            Assert.That(messages[2].senderId, Is.EqualTo("bob"));
            Assert.That(messages[2].content, Is.EqualTo("No, just want to know if there's anybody else in the pie society..."));

            Assert.That(messages.Count, Is.EqualTo(3));
        }

        /// <summary>
        /// Test on filtering conversation by word
        /// </summary>
        [Test]
        public void FilterConversationByWord()
        {
            var exporter = new ConversationExporter();

            var args = new string[2];
            args[0] = "--filterByWord"; args[1] = "pie";

            var configuration = new ConfigurationBuilder().AddCommandLine(args).Build();
            var exporterConfiguration = configuration.Get<ConversationExporterConfiguration>();

            var additionalOptions = new AdditionalConversationOptions(exporterConfiguration);

            exporter.ExportConversation("chat.txt", "chat_wordFilter.json", additionalOptions);

            var serializedConversation = new StreamReader(new FileStream("chat_wordFilter.json", FileMode.Open)).ReadToEnd();

            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.That(savedConversation.name, Is.EqualTo("My Conversation"));

            var messages = savedConversation.messages.ToList();

            Assert.That(messages[0].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470906)));
            Assert.That(messages[0].senderId, Is.EqualTo("bob"));
            Assert.That(messages[0].content, Is.EqualTo("I'm good thanks, do you like pie?"));

            Assert.That(messages[1].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470912)));
            Assert.That(messages[1].senderId, Is.EqualTo("angus"));
            Assert.That(messages[1].content, Is.EqualTo("Hell yes! Are we buying some pie?"));

            Assert.That(messages[2].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470914)));
            Assert.That(messages[2].senderId, Is.EqualTo("bob"));
            Assert.That(messages[2].content, Is.EqualTo("No, just want to know if there's anybody else in the pie society..."));

            Assert.That(messages[3].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470915)));
            Assert.That(messages[3].senderId, Is.EqualTo("angus"));
            Assert.That(messages[3].content, Is.EqualTo("YES! I'm the head pie eater there..."));

            Assert.That(messages.Count, Is.EqualTo(4));
        }
    }
}