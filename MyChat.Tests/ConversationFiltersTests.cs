using System.IO;
using System.Linq;
using MyChat;
using Newtonsoft.Json;

namespace MindLink.Recruitment.MyChat.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Generic;

    /// <summary>
    /// Tests for the <see cref="ConversationFilters"/>.
    /// </summary>
    [TestClass]
    public class ConversationFiltersTests
    {

        /// <summary>
        /// Takes currents users Dsktop folder path.
        /// </summary>
        private string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\";

        /// <summary>
        /// Tests that the conversation filter throws an exception when the <see cref="ConversationExporterConfiguration"/> is null with the specified message.
        /// </summary>
        [TestMethod]
        public void ApplyFiltersArgumentException()
        {
            try
            {
                ConversationFilters filter = new ConversationFilters();
                Conversation filteredConversation = filter.ApplyFilters(new Conversation("Test", new List<Message>()), null);
            }
            catch (NullReferenceException e)
            {
                Assert.AreEqual("Something went wrong while applying the conversation filters.", e.Message);
            }
        }

        /// <summary>
        /// Tests that the conversation filter method for applying the user filter throws an exception when the user filter is null with the specified message.
        /// </summary>
        [TestMethod]
        public void SetUserMessagesFilterArgumentException()
        {
            try
            {
                ConversationExporterConfiguration configuration = new ConversationExporterConfiguration(path + "chat.txt", path + "chat_output.json");
                configuration.SetUserMessagesFilter(null);
                configuration.SetKeywordMessagesFilter("");
                configuration.SetMessageHiddenWords(new string[] { "" });
                ConversationFilters filter = new ConversationFilters();
                Conversation filteredConversation = filter.ApplyFilters(new Conversation("Test", new List<Message>()), configuration);
            }
            catch (NullReferenceException e)
            {
                Assert.AreEqual("Something went wrong while applying the conversation filters.", e.Message);
            }
        }

        /// <summary>
        /// Tests that the conversation filter method for applying the keyword filter throws an exception when the keyword filter is null with the specified message..
        /// </summary>
        [TestMethod]
        public void SetKeywordMessagesFilterFilterArgumentException()
        {
            try
            {
                ConversationExporterConfiguration configuration = new ConversationExporterConfiguration(path + "chat.txt", path + "chat_output.json");
                configuration.SetUserMessagesFilter("");
                configuration.SetKeywordMessagesFilter(null);
                configuration.SetMessageHiddenWords(new string[] { "" });
                ConversationFilters filter = new ConversationFilters();
                Conversation filteredConversation = filter.ApplyFilters(new Conversation("Test", new List<Message>()), null);
            }
            catch (NullReferenceException e)
            {
                Assert.AreEqual("Something went wrong while applying the conversation filters.", e.Message);
            }
        }

        /// <summary>
        /// Tests that the conversation filter method for applying the hidden words filter throws an exception when the hidden words is null with the specified message..
        /// </summary>
        [TestMethod]
        public void SetMessageHiddenWordsFilterFilterArgumentException()
        {
            try
            {
                ConversationExporterConfiguration configuration = new ConversationExporterConfiguration(path + "chat.txt", path + "chat_output.json");
                configuration.SetUserMessagesFilter("");
                configuration.SetKeywordMessagesFilter("");
                configuration.SetMessageHiddenWords(null);
                ConversationFilters filter = new ConversationFilters();
                Conversation filteredConversation = filter.ApplyFilters(new Conversation("Test", new List<Message>()), null);
            }
            catch (NullReferenceException e)
            {
                Assert.AreEqual("Something went wrong while applying the conversation filters.", e.Message);
            }
        }

    }
}


