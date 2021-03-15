using MyChat;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;

namespace MindLink.Recruitment.MyChat.Tests
{
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

        /// <summary>
        /// Test on blacklisting words in conversation
        /// </summary>
        [Test]
        public void BlacklistWordsInConversation()
        {
            var exporter = new ConversationExporter();

            var args = new string[2];
            args[0] = "--blacklist"; args[1] = "pie,society";

            var configuration = new ConfigurationBuilder().AddCommandLine(args).Build();
            var exporterConfiguration = configuration.Get<ConversationExporterConfiguration>();

            var additionalOptions = new AdditionalConversationOptions(exporterConfiguration);

            exporter.ExportConversation("chat.txt", "chat_wordBlacklist.json", additionalOptions);

            var serializedConversation = new StreamReader(new FileStream("chat_wordBlacklist.json", FileMode.Open)).ReadToEnd();

            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.That(savedConversation.name, Is.EqualTo("My Conversation"));

            var messages = savedConversation.messages.ToList();

            Assert.That(messages[2].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470906)));
            Assert.That(messages[2].senderId, Is.EqualTo("bob"));
            Assert.That(messages[2].content, Is.EqualTo("I'm good thanks, do you like *redacted*?"));

            Assert.That(messages[4].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470912)));
            Assert.That(messages[4].senderId, Is.EqualTo("angus"));
            Assert.That(messages[4].content, Is.EqualTo("Hell yes! Are we buying some *redacted*?"));

            Assert.That(messages[5].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470914)));
            Assert.That(messages[5].senderId, Is.EqualTo("bob"));
            Assert.That(messages[5].content, Is.EqualTo("No, just want to know if there's anybody else in the *redacted* *redacted*..."));

            Assert.That(messages[6].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470915)));
            Assert.That(messages[6].senderId, Is.EqualTo("angus"));
            Assert.That(messages[6].content, Is.EqualTo("YES! I'm the head *redacted* eater there..."));
        }

        /// <summary>
        /// Test report option for conversation
        /// </summary>
        [Test]
        public void ReportOnConversation()
        {
            var exporter = new ConversationExporter();

            var args = new string[3];
            args[0] = "--inputFilePath";
            args[1] = "./MyChat/chat.txt";
            args[2] = "--report";

            var configuration = new ConfigurationBuilder().AddCommandLine(args).Build();
            var exporterConfiguration = configuration.Get<ConversationExporterConfiguration>();

            exporterConfiguration.Report = true;

            var additionalOptions = new AdditionalConversationOptions(exporterConfiguration);

            exporter.ExportConversation("chat.txt", "chat_report.json", additionalOptions);

            var serializedConversation = new StreamReader(new FileStream("chat_report.json", FileMode.Open)).ReadToEnd();

            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            var activities = savedConversation.activity.ToList();

            Assert.That(activities[0].sender, Is.EqualTo("bob"));
            Assert.That(activities[0].count, Is.EqualTo(3));

            Assert.That(activities[1].sender, Is.EqualTo("mike"));
            Assert.That(activities[1].count, Is.EqualTo(2));

            Assert.That(activities[2].sender, Is.EqualTo("angus"));
            Assert.That(activities[2].count, Is.EqualTo(2));
        }
    }
}