using System;
using System.Linq;
using NUnit.Framework;
using MindLink.Recruitment.MyChat.Filters;
using MindLink.Recruitment.MyChat.Data;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using MyChat;

namespace MindLink.Recruitment.MyChat.Tests
{
    /// <summary>
    /// Tests for the <see cref="ConversationExporter"/>.
    /// </summary>
    [TestFixture]
    public class EndToEndTests
    {
        /// <summary>
        /// Tests that all the filters combined filters the chat correctly.
        /// </summary>
        [Test]
        public void CombinedFiltersFilterChat()
        {
            // Define input and output file path

            var inputFilePath = "chat.txt";
            var outputFilePath = "chat.json";

            // Create instances of conversation reader and writer

            var conversationReader = new ConversationReader();
            var conversationWriter = new ConversationWriter();

            // Read the inputFilePath

            var readConversation = conversationReader.ReadConversation(inputFilePath);

            // Create a list of filters

            var filters = new List<IFilter>();

            // Create all filters to test

            var usersToTest = new string[] { "bob" };
            var keywordsToTest = new string[] { "you" };
            var blacklistedWordsToTest = new string[] { "do" };

            // Populate the list of filters

            filters.Add(new UserFilter(usersToTest));
            filters.Add(new KeywordFilter(keywordsToTest));
            filters.Add(new Blacklist(blacklistedWordsToTest));
            filters.Add(new Report());

            // Create an instance of conversation exporter

            var conversationExporter = new ConversationExporter(filters);
           
            // Write the combined conversation to the output file path

            conversationWriter.WriteConversation(readConversation, outputFilePath);

            // Export the combined conversation to the output file path

            conversationExporter.ExportConversation(inputFilePath, outputFilePath);

            var serializedConversation = new StreamReader(new FileStream(outputFilePath, FileMode.Open)).ReadToEnd();

            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.That(savedConversation.name, Is.EqualTo("My Conversation"));

            var combinedFilteredMessages = savedConversation.messages.ToList();
            var combinedfilteredActivity = savedConversation.activity.ToList();

            Assert.That(combinedFilteredMessages.Count == 1);
            Assert.That(combinedfilteredActivity.Count == 1);

            Assert.That(combinedFilteredMessages[0].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470906)));
            Assert.That(combinedFilteredMessages[0].senderId, Is.EqualTo("bob"));
            Assert.That(combinedFilteredMessages[0].content, Is.EqualTo("I'm good thanks, *redacted* you like pie?"));

            Assert.That(combinedfilteredActivity[0].count == 1);
            Assert.That(combinedfilteredActivity[0].sender == "bob");
        }
    }
}
