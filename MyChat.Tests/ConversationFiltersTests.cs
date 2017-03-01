using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using MyChat.Tools;
using MyChat.Models;

namespace MyChat.Tests
{

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
        /// Tests that the conversation filter method for applying the user filter throws an exception when the user filter is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetUserMessagesFilterArgumentException()
        {
            ConversationFilters filter = new ConversationFilters(null, "", new string[] { "" });
            Conversation conversation = new Conversation();
            conversation.name = "Test";
            conversation.messages = new List<Message>();
            filter.ApplyUserMessageFilter();
        }

        /// <summary>
        /// Tests that the conversation filter method for applying the keyword filter throws an exception when the keyword filter is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetKeywordMessagesFilterFilterArgumentException()
        {
            ConversationFilters filter = new ConversationFilters("", null, new string[] { "" });
            Conversation conversation = new Conversation();
            conversation.name = "Test";
            conversation.messages = new List<Message>();
            filter.ApplyKeywordMessageFilter();
        }

        /// <summary>
        /// Tests that the conversation filter method for applying the hidden words filter throws an exception when the hidden words is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetMessageHiddenWordsFilterFilterArgumentException()
        {
            ConversationFilters filter = new ConversationFilters("", "", null);
            Conversation conversation = new Conversation();
            conversation.name = "Test";
            conversation.messages = new List<Message>();
            filter.ReplaceHiddenWords();
        }

    }
}


