﻿using System.IO;
using System.Linq;
using MyChat;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MindLink.Recruitment.MyChat.Tests
{
    using Microsoft.Extensions.Configuration;
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

            var args = new string[2];
            args[0] = "--inputFilePath";
            args[1] = "./MyChat/chat.txt";

            var configuration = new ConfigurationBuilder().AddCommandLine(args).Build();
            var exporterConfiguration = configuration.Get<ConversationExporterConfiguration>();

            var additionalOptions = new AdditionalConversationOptions(exporterConfiguration);

            exporter.ExportConversation("chat.txt", "chat.json",additionalOptions);

            var serializedConversation = new StreamReader(new FileStream("chat.json", FileMode.Open)).ReadToEnd();

            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.That(savedConversation.name, Is.EqualTo("My Conversation"));

            var messages = savedConversation.messages.ToList();

            Assert.That(messages[0].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470901)));
            Assert.That(messages[0].senderId, Is.EqualTo("bob"));
            Assert.That(messages[0].content, Is.EqualTo("Hello there!"));

            Assert.That(messages[1].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470905)));
            Assert.That(messages[1].senderId, Is.EqualTo("mike"));
            Assert.That(messages[1].content, Is.EqualTo("how are you?"));

            Assert.That(messages[2].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470906)));
            Assert.That(messages[2].senderId, Is.EqualTo("bob"));
            Assert.That(messages[2].content, Is.EqualTo("I'm good thanks, do you like pie?"));

            Assert.That(messages[3].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470910)));
            Assert.That(messages[3].senderId, Is.EqualTo("mike"));
            Assert.That(messages[3].content, Is.EqualTo("no, let me ask Angus..."));

            Assert.That(messages[4].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470912)));
            Assert.That(messages[4].senderId, Is.EqualTo("angus"));
            Assert.That(messages[4].content, Is.EqualTo("Hell yes! Are we buying some pie?"));

            Assert.That(messages[5].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470914)));
            Assert.That(messages[5].senderId, Is.EqualTo("bob"));
            Assert.That(messages[5].content, Is.EqualTo("No, just want to know if there's anybody else in the pie society..."));

            Assert.That(messages[6].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470915)));
            Assert.That(messages[6].senderId, Is.EqualTo("angus"));
            Assert.That(messages[6].content, Is.EqualTo("YES! I'm the head pie eater there..."));
        }
    }
}
