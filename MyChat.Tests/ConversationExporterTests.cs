using System.IO;
using System.Linq;
using MyChat;
using Newtonsoft.Json;

namespace MindLink.Recruitment.MyChat.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for the <see cref="ConversationExporter"/>.
    /// These tests cover most of the functionality including filters, blacklists, encryption and hidden numbers.
    /// </summary>
    [TestClass]
    public class ConversationExporterTests
    {
        /// <summary>
        /// Tests that exporting the conversation exports conversation.
        /// </summary>
        [TestMethod]
        public void ExportingConversationExportsConversation()
        {
            ConversationExporter exporter = new ConversationExporter();

            exporter.ExportConversation("chat.txt", "chat.json", null, null, null, false, false);

            FileStream stream = new FileStream("chat.json", FileMode.Open);

            // using statement to correctly dispose StreamReader
            using (StreamReader reader = new StreamReader(stream))
            {
                string serializedConversation = reader.ReadToEnd();

                Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

                Assert.AreEqual("My Conversation", savedConversation.Name);

                var messages = savedConversation.Messages.ToList();

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470901), messages[0].Timestamp);
                Assert.AreEqual("bob", messages[0].SenderId);
                Assert.AreEqual("Hello there!", messages[0].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470905), messages[1].Timestamp);
                Assert.AreEqual("mike", messages[1].SenderId);
                Assert.AreEqual("how are you?", messages[1].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470906), messages[2].Timestamp);
                Assert.AreEqual("bob", messages[2].SenderId);
                Assert.AreEqual("I'm good thanks, do you like pie?", messages[2].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470910), messages[3].Timestamp);
                Assert.AreEqual("mike", messages[3].SenderId);
                Assert.AreEqual("no, let me ask Angus...", messages[3].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470912), messages[4].Timestamp);
                Assert.AreEqual("angus", messages[4].SenderId);
                Assert.AreEqual("Hell yes! Are we buying some pie?", messages[4].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[5].Timestamp);
                Assert.AreEqual("bob", messages[5].SenderId);
                Assert.AreEqual("No, just want to know if there's anybody else in the pie society...", messages[5].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470915), messages[6].Timestamp);
                Assert.AreEqual("angus", messages[6].SenderId);
                Assert.AreEqual("YES! I'm the head pie eater there...", messages[6].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470925), messages[7].Timestamp);
                Assert.AreEqual("bob", messages[7].SenderId);
                Assert.AreEqual("Call me on 281-342-2452 to give me your credit card details.", messages[7].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470927), messages[8].Timestamp);
                Assert.AreEqual("angus", messages[8].SenderId);
                Assert.AreEqual("Its 5105105105105100", messages[8].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470929), messages[9].Timestamp);
                Assert.AreEqual("mike", messages[9].SenderId);
                Assert.AreEqual("Mine is 4012888888881881", messages[9].Content);

                var userActivity = savedConversation.UserActivity.ToList();

                Assert.AreEqual("bob", userActivity[0].UserName);
                Assert.AreEqual(4, userActivity[0].MessagesNo);

                Assert.AreEqual("mike", userActivity[1].UserName);
                Assert.AreEqual(3, userActivity[1].MessagesNo);

                Assert.AreEqual("angus", userActivity[2].UserName);
                Assert.AreEqual(3, userActivity[2].MessagesNo);
            }
        }

        /// <summary>
        /// Tests that exporting the conversation exports conversation using the username filter.
        /// </summary>
        [TestMethod]
        public void ExportingConversationExportsConversationWithUsernameFilter()
        {
            ConversationExporter exporter = new ConversationExporter();

            exporter.ExportConversation("chat.txt", "bob's chat.json", "bob", null, null, false, false);

            FileStream stream = new FileStream("bob's chat.json", FileMode.Open);
            // using statement to correctly dispose StreamReader
            using (StreamReader reader = new StreamReader(stream))
            {
                string serializedConversation = reader.ReadToEnd();

                Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

                Assert.AreEqual("My Conversation", savedConversation.Name);

                var messages = savedConversation.Messages.ToList();

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470901), messages[0].Timestamp);
                Assert.AreEqual("bob", messages[0].SenderId);
                Assert.AreEqual("Hello there!", messages[0].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470906), messages[1].Timestamp);
                Assert.AreEqual("bob", messages[1].SenderId);
                Assert.AreEqual("I'm good thanks, do you like pie?", messages[1].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[2].Timestamp);
                Assert.AreEqual("bob", messages[2].SenderId);
                Assert.AreEqual("No, just want to know if there's anybody else in the pie society...", messages[2].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470925), messages[3].Timestamp);
                Assert.AreEqual("bob", messages[3].SenderId);
                Assert.AreEqual("Call me on 281-342-2452 to give me your credit card details.", messages[3].Content);

                var userActivity = savedConversation.UserActivity.ToList();

                Assert.AreEqual("bob", userActivity[0].UserName);
                Assert.AreEqual(4, userActivity[0].MessagesNo);
            }
        }

        /// <summary>
        /// Tests that exporting the conversation exports conversation using the keyword filter.
        /// </summary>
        [TestMethod]
        public void ExportingConversationExportsConversationWithKeywordFilter()
        {
            ConversationExporter exporter = new ConversationExporter();

            exporter.ExportConversation("chat.txt", "pie chat.json", null, "pie", null, false, false);

            FileStream stream = new FileStream("pie chat.json", FileMode.Open);

            // using statement to correctly dispose StreamReader
            using (StreamReader reader = new StreamReader(stream))
            {
                string serializedConversation = reader.ReadToEnd();

                Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

                Assert.AreEqual("My Conversation", savedConversation.Name);

                var messages = savedConversation.Messages.ToList();

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470906), messages[0].Timestamp);
                Assert.AreEqual("bob", messages[0].SenderId);
                Assert.AreEqual("I'm good thanks, do you like pie?", messages[0].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470912), messages[1].Timestamp);
                Assert.AreEqual("angus", messages[1].SenderId);
                Assert.AreEqual("Hell yes! Are we buying some pie?", messages[1].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[2].Timestamp);
                Assert.AreEqual("bob", messages[2].SenderId);
                Assert.AreEqual("No, just want to know if there's anybody else in the pie society...", messages[2].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470915), messages[3].Timestamp);
                Assert.AreEqual("angus", messages[3].SenderId);
                Assert.AreEqual("YES! I'm the head pie eater there...", messages[3].Content);

                var userActivity = savedConversation.UserActivity.ToList();

                Assert.AreEqual("bob", userActivity[0].UserName);
                Assert.AreEqual(2, userActivity[0].MessagesNo);

                Assert.AreEqual("angus", userActivity[1].UserName);
                Assert.AreEqual(2, userActivity[1].MessagesNo);
            }
        }

        /// <summary>
        /// Tests that exporting the conversation exports conversation using the both username AND keyword filter.
        /// </summary>
        [TestMethod]
        public void ExportingConversationExportsConversationWithUsernameAndKeywordFilter()
        {
            ConversationExporter exporter = new ConversationExporter();

            exporter.ExportConversation("chat.txt", "bob's pie chat.json", "bob", "pie", null, false, false);

            FileStream stream = new FileStream("bob's pie chat.json", FileMode.Open);
            // using statement to correctly dispose StreamReader
            using (StreamReader reader = new StreamReader(stream))
            {
                string serializedConversation = reader.ReadToEnd();

                Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

                Assert.AreEqual("My Conversation", savedConversation.Name);

                var messages = savedConversation.Messages.ToList();

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470906), messages[0].Timestamp);
                Assert.AreEqual("bob", messages[0].SenderId);
                Assert.AreEqual("I'm good thanks, do you like pie?", messages[0].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[1].Timestamp);
                Assert.AreEqual("bob", messages[1].SenderId);
                Assert.AreEqual("No, just want to know if there's anybody else in the pie society...", messages[1].Content);

                var userActivity = savedConversation.UserActivity.ToList();

                Assert.AreEqual("bob", userActivity[0].UserName);
                Assert.AreEqual(2, userActivity[0].MessagesNo);
            }
        }

        /// <summary>
        /// Tests that exporting the conversation exports conversation using the blacklist filter.
        /// </summary>
        [TestMethod]
        public void ExportingConversationExportsConversationWithBlacklistFilter()
        {
            ConversationExporter exporter = new ConversationExporter();

            exporter.ExportConversation("chat.txt", "blackist chat.json", null, null, "hell", false, false);

            FileStream stream = new FileStream("blackist chat.json", FileMode.Open);
            // using statement to correctly dispose StreamReader
            using (StreamReader reader = new StreamReader(stream))
            {
                string serializedConversation = reader.ReadToEnd();

                Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

                Assert.AreEqual("My Conversation", savedConversation.Name);

                var messages = savedConversation.Messages.ToList();

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470901), messages[0].Timestamp);
                Assert.AreEqual("bob", messages[0].SenderId);
                Assert.AreEqual("Hello there!", messages[0].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470905), messages[1].Timestamp);
                Assert.AreEqual("mike", messages[1].SenderId);
                Assert.AreEqual("how are you?", messages[1].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470906), messages[2].Timestamp);
                Assert.AreEqual("bob", messages[2].SenderId);
                Assert.AreEqual("I'm good thanks, do you like pie?", messages[2].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470910), messages[3].Timestamp);
                Assert.AreEqual("mike", messages[3].SenderId);
                Assert.AreEqual("no, let me ask Angus...", messages[3].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470912), messages[4].Timestamp);
                Assert.AreEqual("angus", messages[4].SenderId);
                // This is where the blacklist takes place.
                Assert.AreEqual("**** yes! Are we buying some pie?", messages[4].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[5].Timestamp);
                Assert.AreEqual("bob", messages[5].SenderId);
                Assert.AreEqual("No, just want to know if there's anybody else in the pie society...", messages[5].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470915), messages[6].Timestamp);
                Assert.AreEqual("angus", messages[6].SenderId);
                Assert.AreEqual("YES! I'm the head pie eater there...", messages[6].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470925), messages[7].Timestamp);
                Assert.AreEqual("bob", messages[7].SenderId);
                Assert.AreEqual("Call me on 281-342-2452 to give me your credit card details.", messages[7].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470927), messages[8].Timestamp);
                Assert.AreEqual("angus", messages[8].SenderId);
                Assert.AreEqual("Its 5105105105105100", messages[8].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470929), messages[9].Timestamp);
                Assert.AreEqual("mike", messages[9].SenderId);
                Assert.AreEqual("Mine is 4012888888881881", messages[9].Content);

                var userActivity = savedConversation.UserActivity.ToList();

                Assert.AreEqual("bob", userActivity[0].UserName);
                Assert.AreEqual(4, userActivity[0].MessagesNo);

                Assert.AreEqual("mike", userActivity[1].UserName);
                Assert.AreEqual(3, userActivity[1].MessagesNo);

                Assert.AreEqual("angus", userActivity[2].UserName);
                Assert.AreEqual(3, userActivity[2].MessagesNo);

            }
        }

        /// <summary>
        /// Tests that exporting the conversation exports conversation using the multiple words for blacklist filter, comma separated.
        /// </summary>
        [TestMethod]
        public void ExportingConversationExportsConversationWithMultipleBlacklistFilter()
        {
            ConversationExporter exporter = new ConversationExporter();

            exporter.ExportConversation("chat.txt", "blackist chat.json", null, null, "hell, pie", false, false);

            FileStream stream = new FileStream("blackist chat.json", FileMode.Open);
            // using statement to correctly dispose StreamReader
            using (StreamReader reader = new StreamReader(stream))
            {
                string serializedConversation = reader.ReadToEnd();

                Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

                Assert.AreEqual("My Conversation", savedConversation.Name);

                var messages = savedConversation.Messages.ToList();

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470901), messages[0].Timestamp);
                Assert.AreEqual("bob", messages[0].SenderId);
                Assert.AreEqual("Hello there!", messages[0].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470905), messages[1].Timestamp);
                Assert.AreEqual("mike", messages[1].SenderId);
                Assert.AreEqual("how are you?", messages[1].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470906), messages[2].Timestamp);
                Assert.AreEqual("bob", messages[2].SenderId);
                Assert.AreEqual("I'm good thanks, do you like ***?", messages[2].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470910), messages[3].Timestamp);
                Assert.AreEqual("mike", messages[3].SenderId);
                Assert.AreEqual("no, let me ask Angus...", messages[3].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470912), messages[4].Timestamp);
                Assert.AreEqual("angus", messages[4].SenderId);
                // This is where the blacklist takes place.
                Assert.AreEqual("**** yes! Are we buying some ***?", messages[4].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[5].Timestamp);
                Assert.AreEqual("bob", messages[5].SenderId);
                Assert.AreEqual("No, just want to know if there's anybody else in the *** society...", messages[5].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470915), messages[6].Timestamp);
                Assert.AreEqual("angus", messages[6].SenderId);
                Assert.AreEqual("YES! I'm the head *** eater there...", messages[6].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470925), messages[7].Timestamp);
                Assert.AreEqual("bob", messages[7].SenderId);
                Assert.AreEqual("Call me on 281-342-2452 to give me your credit card details.", messages[7].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470927), messages[8].Timestamp);
                Assert.AreEqual("angus", messages[8].SenderId);
                Assert.AreEqual("Its 5105105105105100", messages[8].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470929), messages[9].Timestamp);
                Assert.AreEqual("mike", messages[9].SenderId);
                Assert.AreEqual("Mine is 4012888888881881", messages[9].Content);

                var userActivity = savedConversation.UserActivity.ToList();

                Assert.AreEqual("bob", userActivity[0].UserName);
                Assert.AreEqual(4, userActivity[0].MessagesNo);

                Assert.AreEqual("mike", userActivity[1].UserName);
                Assert.AreEqual(3, userActivity[1].MessagesNo);

                Assert.AreEqual("angus", userActivity[2].UserName);
                Assert.AreEqual(3, userActivity[2].MessagesNo);
            }
        }

        /// <summary>
        /// Tests that exporting the conversation exports conversation using the username AND blacklist filter.
        /// </summary>
        [TestMethod]
        public void ExportingConversationExportsConversationWithUsernameAndBlacklistFilter()
        {
            ConversationExporter exporter = new ConversationExporter();

            exporter.ExportConversation("chat.txt", "username and blackist chat.json", "angus", null, "hell", false, false);

            FileStream stream = new FileStream("username and blackist chat.json", FileMode.Open);
            // using statement to correctly dispose StreamReader
            using (StreamReader reader = new StreamReader(stream))
            {
                string serializedConversation = reader.ReadToEnd();

                Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

                Assert.AreEqual("My Conversation", savedConversation.Name);

                var messages = savedConversation.Messages.ToList();

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470912), messages[0].Timestamp);
                Assert.AreEqual("angus", messages[0].SenderId);
                // This is where the blacklist takes place.
                Assert.AreEqual("**** yes! Are we buying some pie?", messages[0].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470915), messages[1].Timestamp);
                Assert.AreEqual("angus", messages[1].SenderId);
                Assert.AreEqual("YES! I'm the head pie eater there...", messages[1].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470927), messages[2].Timestamp);
                Assert.AreEqual("angus", messages[2].SenderId);
                Assert.AreEqual("Its 5105105105105100", messages[2].Content);

                var userActivity = savedConversation.UserActivity.ToList();

                Assert.AreEqual("angus", userActivity[0].UserName);
                Assert.AreEqual(3, userActivity[0].MessagesNo);
            }
        }

        /// <summary>
        /// Tests that exporting the conversation exports conversation using the keywoord AND blacklist filter.
        /// </summary>
        [TestMethod]
        public void ExportingConversationExportsConversationWithKeywordAndBlackListFilter()
        {
            ConversationExporter exporter = new ConversationExporter();

            exporter.ExportConversation("chat.txt", "keyword and blackist chat.json", null, "pie", "hell", false, false);

            FileStream stream = new FileStream("keyword and blackist chat.json", FileMode.Open);
            // using statement to correctly dispose StreamReader
            using (StreamReader reader = new StreamReader(stream))
            {
                string serializedConversation = reader.ReadToEnd();

                Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

                Assert.AreEqual("My Conversation", savedConversation.Name);

                var messages = savedConversation.Messages.ToList();

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470906), messages[0].Timestamp);
                Assert.AreEqual("bob", messages[0].SenderId);
                Assert.AreEqual("I'm good thanks, do you like pie?", messages[0].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470912), messages[1].Timestamp);
                Assert.AreEqual("angus", messages[1].SenderId);
                Assert.AreEqual("**** yes! Are we buying some pie?", messages[1].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[2].Timestamp);
                Assert.AreEqual("bob", messages[2].SenderId);
                Assert.AreEqual("No, just want to know if there's anybody else in the pie society...", messages[2].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470915), messages[3].Timestamp);
                Assert.AreEqual("angus", messages[3].SenderId);
                Assert.AreEqual("YES! I'm the head pie eater there...", messages[3].Content);

                var userActivity = savedConversation.UserActivity.ToList();

                Assert.AreEqual("bob", userActivity[0].UserName);
                Assert.AreEqual(2, userActivity[0].MessagesNo);

                Assert.AreEqual("angus", userActivity[1].UserName);
                Assert.AreEqual(2, userActivity[1].MessagesNo);
            }
        }

        /// <summary>
        /// Tests that exporting the conversation exports conversation using the keywoord AND blacklist filter.
        /// </summary>
        [TestMethod]
        public void ExportingConversationExportsConversationWithUsernameKeywordAndBlacklistFilter()
        {
            ConversationExporter exporter = new ConversationExporter();

            exporter.ExportConversation("chat.txt", "username, keyword and blackist chat.json", "angus", "pie", "hell", false, false);

            FileStream stream = new FileStream("username, keyword and blackist chat.json", FileMode.Open);
            // using statement to correctly dispose StreamReader
            using (StreamReader reader = new StreamReader(stream))
            {
                string serializedConversation = reader.ReadToEnd();

                Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

                Assert.AreEqual("My Conversation", savedConversation.Name);

                var messages = savedConversation.Messages.ToList();

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470912), messages[0].Timestamp);
                Assert.AreEqual("angus", messages[0].SenderId);
                Assert.AreEqual("**** yes! Are we buying some pie?", messages[0].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470915), messages[1].Timestamp);
                Assert.AreEqual("angus", messages[1].SenderId);
                Assert.AreEqual("YES! I'm the head pie eater there...", messages[1].Content);

                var userActivity = savedConversation.UserActivity.ToList();

                Assert.AreEqual("angus", userActivity[0].UserName);
                Assert.AreEqual(2, userActivity[0].MessagesNo);
            }
        }

        /// <summary>
        /// Tests that exporting the conversation exports conversation with usernames encrypted.
        /// </summary>
        [TestMethod]
        public void ExportingConversationExportsConversationWithEncryptedUsernames()
        {
            ConversationExporter exporter = new ConversationExporter();

            exporter.ExportConversation("chat.txt", "encrypted usernames chat.json", null, null, null, true, false);

            FileStream stream = new FileStream("encrypted usernames chat.json", FileMode.Open);
            // using statement to correctly dispose StreamReader
            using (StreamReader reader = new StreamReader(stream))
            {
                string serializedConversation = reader.ReadToEnd();

                Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

                Assert.AreEqual("My Conversation", savedConversation.Name);

                var messages = savedConversation.Messages.ToList();

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470901), messages[0].Timestamp);
                Assert.AreEqual("bob", Encryption.Decrypt(messages[0].SenderId));
                Assert.AreEqual("Hello there!", messages[0].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470905), messages[1].Timestamp);
                Assert.AreEqual("mike", Encryption.Decrypt(messages[1].SenderId));
                Assert.AreEqual("how are you?", messages[1].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470906), messages[2].Timestamp);
                Assert.AreEqual("bob", Encryption.Decrypt(messages[2].SenderId));
                Assert.AreEqual("I'm good thanks, do you like pie?", messages[2].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470910), messages[3].Timestamp);
                Assert.AreEqual("mike", Encryption.Decrypt(messages[3].SenderId));
                Assert.AreEqual("no, let me ask Angus...", messages[3].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470912), messages[4].Timestamp);
                Assert.AreEqual("angus", Encryption.Decrypt(messages[4].SenderId));
                Assert.AreEqual("Hell yes! Are we buying some pie?", messages[4].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[5].Timestamp);
                Assert.AreEqual("bob", Encryption.Decrypt(messages[5].SenderId));
                Assert.AreEqual("No, just want to know if there's anybody else in the pie society...", messages[5].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470915), messages[6].Timestamp);
                Assert.AreEqual("angus", Encryption.Decrypt(messages[6].SenderId));
                Assert.AreEqual("YES! I'm the head pie eater there...", messages[6].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470925), messages[7].Timestamp);
                Assert.AreEqual("bob", Encryption.Decrypt(messages[7].SenderId));
                Assert.AreEqual("Call me on 281-342-2452 to give me your credit card details.", messages[7].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470927), messages[8].Timestamp);
                Assert.AreEqual("angus", Encryption.Decrypt(messages[8].SenderId));
                Assert.AreEqual("Its 5105105105105100", messages[8].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470929), messages[9].Timestamp);
                Assert.AreEqual("mike", Encryption.Decrypt(messages[9].SenderId));
                Assert.AreEqual("Mine is 4012888888881881", messages[9].Content);

                var userActivity = savedConversation.UserActivity.ToList();

                Assert.AreEqual("bob", Encryption.Decrypt(userActivity[0].UserName));
                Assert.AreEqual(4, userActivity[0].MessagesNo);

                Assert.AreEqual("mike", Encryption.Decrypt(userActivity[1].UserName));
                Assert.AreEqual(3, userActivity[1].MessagesNo);

                Assert.AreEqual("angus", Encryption.Decrypt(userActivity[2].UserName));
                Assert.AreEqual(3, userActivity[2].MessagesNo);
            }
        }

        /// <summary>
        /// Tests that exporting the conversation exports conversation with hidden credit card and phone numbers.
        /// </summary>
        [TestMethod]
        public void ExportingConversationExportsConversationWithHiddenNumbers()
        {
            ConversationExporter exporter = new ConversationExporter();

            exporter.ExportConversation("chat.txt", "hidden numbers chat.json", null, null, null, false, true);

            FileStream stream = new FileStream("hidden numbers chat.json", FileMode.Open);
            // using statement to correctly dispose StreamReader
            using (StreamReader reader = new StreamReader(stream))
            {
                string serializedConversation = reader.ReadToEnd();

                Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

                Assert.AreEqual("My Conversation", savedConversation.Name);

                var messages = savedConversation.Messages.ToList();

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470901), messages[0].Timestamp);
                Assert.AreEqual("bob", messages[0].SenderId);
                Assert.AreEqual("Hello there!", messages[0].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470905), messages[1].Timestamp);
                Assert.AreEqual("mike", messages[1].SenderId);
                Assert.AreEqual("how are you?", messages[1].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470906), messages[2].Timestamp);
                Assert.AreEqual("bob", messages[2].SenderId);
                Assert.AreEqual("I'm good thanks, do you like pie?", messages[2].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470910), messages[3].Timestamp);
                Assert.AreEqual("mike", messages[3].SenderId);
                Assert.AreEqual("no, let me ask Angus...", messages[3].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470912), messages[4].Timestamp);
                Assert.AreEqual("angus", messages[4].SenderId);
                Assert.AreEqual("Hell yes! Are we buying some pie?", messages[4].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[5].Timestamp);
                Assert.AreEqual("bob", messages[5].SenderId);
                Assert.AreEqual("No, just want to know if there's anybody else in the pie society...", messages[5].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470915), messages[6].Timestamp);
                Assert.AreEqual("angus", messages[6].SenderId);
                Assert.AreEqual("YES! I'm the head pie eater there...", messages[6].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470925), messages[7].Timestamp);
                Assert.AreEqual("bob", messages[7].SenderId);
                Assert.AreEqual("Call me on ************ to give me your credit card details.", messages[7].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470927), messages[8].Timestamp);
                Assert.AreEqual("angus", messages[8].SenderId);
                Assert.AreEqual("Its ************5100", messages[8].Content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470929), messages[9].Timestamp);
                Assert.AreEqual("mike", messages[9].SenderId);
                Assert.AreEqual("Mine is ************1881", messages[9].Content);

                var userActivity = savedConversation.UserActivity.ToList();

                Assert.AreEqual("bob", userActivity[0].UserName);
                Assert.AreEqual(4, userActivity[0].MessagesNo);

                Assert.AreEqual("mike", userActivity[1].UserName);
                Assert.AreEqual(3, userActivity[1].MessagesNo);

                Assert.AreEqual("angus", userActivity[2].UserName);
                Assert.AreEqual(3, userActivity[2].MessagesNo);
            }
        }

    }
}
