using System.IO;
using System.Linq;
using MyChat.Models;
using MyChat.Tools;
using Newtonsoft.Json;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using MyChat.Exporter;

namespace MyChat.Tests
{
<<<<<<< HEAD
=======
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Text;
>>>>>>> origin/master

    /// <summary>
    /// Tests for the <see cref="ConversationExporter"/>.
    /// </summary>
    [TestClass]
    public class ConversationExporterTests
    {

<<<<<<< HEAD
        /// <summary>
        /// Takes currents users Dsktop folder path.
        /// </summary>
        private string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\";

        /// <summary>
=======
        /// <summary>
        /// Takes currents users Dsktop folder path.
        /// </summary>
        private string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\";

        /// <summary>
>>>>>>> origin/master
        /// Tests that exporting the conversation exports conversation and no filters are applied.
        /// </summary>
        [TestMethod]
        public void TestOutputFileData()
        {
<<<<<<< HEAD
            ConversationExporter exporter = new ConversationExporter(path + "chat.txt", path + "chat_test_1.json");
            ConversationFilters filter = new ConversationFilters("", "", new string[] { "" });
            bool test = exporter.ExportConversation(filter);
=======
            ConversationExporterConfiguration configuration = new ConversationExporterConfiguration(path + "chat.txt", path + "chat_test_1.json");
            configuration.SetUserMessagesFilter("");
            configuration.SetKeywordMessagesFilter("");
            configuration.SetMessageHiddenWords(new string[] { "" });
            new ConversationExporter().ExportConversation(configuration);
>>>>>>> origin/master

            var serializedConversation = new StreamReader(new FileStream(path + "chat_test_1.json", FileMode.Open)).ReadToEnd();
            Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.IsTrue(test);

            Assert.AreEqual("My Conversation", savedConversation.name);

            var messages = savedConversation.messages.ToList();

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470901), messages[0].timestamp);
            Assert.AreEqual("bob", messages[0].sender.username);
            Assert.AreEqual("Hello there!", messages[0].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470905), messages[1].timestamp);
            Assert.AreEqual("mike", messages[1].sender.username);
            Assert.AreEqual("how are you?", messages[1].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470906), messages[2].timestamp);
            Assert.AreEqual("bob", messages[2].sender.username);
            Assert.AreEqual("I'm good thanks, do you like pie?", messages[2].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470910), messages[3].timestamp);
            Assert.AreEqual("mike", messages[3].sender.username);
            Assert.AreEqual("no, let me ask Angus...", messages[3].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470912), messages[4].timestamp);
            Assert.AreEqual("angus", messages[4].sender.username);
            Assert.AreEqual("Hell yes! Are we buying some pie?", messages[4].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[5].timestamp);
            Assert.AreEqual("bob", messages[5].sender.username);
            Assert.AreEqual("No, just want to know if there's anybody else in the pie society...", messages[5].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470915), messages[6].timestamp);
            Assert.AreEqual("angus", messages[6].sender.username);
<<<<<<< HEAD
=======
            Assert.AreEqual("YES! I'm the head pie eater there...", messages[6].content);
        }

        /// <summary>
        /// Tests that exporting the conversation exports conversation and applies the user filter.
        /// </summary>
        [TestMethod]
        public void TestOutputFileDataWithUserFilter()
        {
            ConversationExporterConfiguration configuration = new ConversationExporterConfiguration(path + "chat.txt", path + "chat_test_2.json");
            configuration.SetUserMessagesFilter("bob");
            configuration.SetKeywordMessagesFilter("");
            configuration.SetMessageHiddenWords(new string[] {""});
            new ConversationExporter().ExportConversation(configuration);

            var serializedConversation = new StreamReader(new FileStream(path + "chat_test_2.json", FileMode.Open)).ReadToEnd();
            Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.AreEqual("My Conversation", savedConversation.name);

            var messages = savedConversation.messages.ToList();

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470901), messages[0].timestamp);
            Assert.AreEqual("bob", messages[0].sender.username);
            Assert.AreEqual("Hello there!", messages[0].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470906), messages[1].timestamp);
            Assert.AreEqual("bob", messages[1].sender.username);
            Assert.AreEqual("I'm good thanks, do you like pie?", messages[1].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[2].timestamp);
            Assert.AreEqual("bob", messages[2].sender.username);
            Assert.AreEqual("No, just want to know if there's anybody else in the pie society...", messages[2].content);
        }

        /// <summary>
        /// Tests that exporting the conversation exports conversation and applies the keyword filter.
        /// </summary>
        [TestMethod]
        public void TestOutputFileDataWithKeywordFilter()
        {
            ConversationExporterConfiguration configuration = new ConversationExporterConfiguration(path + "chat.txt", path + "chat_test_3.json");
            configuration.SetUserMessagesFilter("");
            configuration.SetKeywordMessagesFilter("no");
            configuration.SetMessageHiddenWords(new string[] { "" });
            new ConversationExporter().ExportConversation(configuration);

            var serializedConversation = new StreamReader(new FileStream(path + "chat_test_3.json", FileMode.Open)).ReadToEnd();
            Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.AreEqual("My Conversation", savedConversation.name);

            var messages = savedConversation.messages.ToList();

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470910), messages[0].timestamp);
            Assert.AreEqual("mike", messages[0].sender.username);
            Assert.AreEqual("no, let me ask Angus...", messages[0].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[1].timestamp);
            Assert.AreEqual("bob", messages[1].sender.username);
            Assert.AreEqual("No, just want to know if there's anybody else in the pie society...", messages[1].content);
        }

        /// <summary>
        /// Tests that exporting the conversation exports conversation and replaces the hidden words with replacement word.
        /// </summary>
        [TestMethod]
        public void TestOutputFileDataWithHiddenWordsFilter()
        {
            ConversationExporterConfiguration configuration = new ConversationExporterConfiguration(path + "chat.txt", path + "chat_test_4.json");
            configuration.SetUserMessagesFilter("");
            configuration.SetKeywordMessagesFilter("");
            configuration.SetMessageHiddenWords(new string[] { "NO", "ask" });
            new ConversationExporter().ExportConversation(configuration);

            var serializedConversation = new StreamReader(new FileStream(path + "chat_test_4.json", FileMode.Open)).ReadToEnd();
            Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.AreEqual("My Conversation", savedConversation.name);

            var messages = savedConversation.messages.ToList();

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470901), messages[0].timestamp);
            Assert.AreEqual("bob", messages[0].sender.username);
            Assert.AreEqual("Hello there!", messages[0].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470905), messages[1].timestamp);
            Assert.AreEqual("mike", messages[1].sender.username);
            Assert.AreEqual("how are you?", messages[1].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470906), messages[2].timestamp);
            Assert.AreEqual("bob", messages[2].sender.username);
            Assert.AreEqual("I'm good thanks, do you like pie?", messages[2].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470910), messages[3].timestamp);
            Assert.AreEqual("mike", messages[3].sender.username);
            Assert.AreEqual("\\*redacted\\* let me \\*redacted\\* Angus...", messages[3].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470912), messages[4].timestamp);
            Assert.AreEqual("angus", messages[4].sender.username);
            Assert.AreEqual("Hell yes! Are we buying some pie?", messages[4].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[5].timestamp);
            Assert.AreEqual("bob", messages[5].sender.username);
            Assert.AreEqual("\\*redacted\\* just want to know if there's anybody else in the pie society...", messages[5].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470915), messages[6].timestamp);
            Assert.AreEqual("angus", messages[6].sender.username);
>>>>>>> origin/master
            Assert.AreEqual("YES! I'm the head pie eater there...", messages[6].content);
        }

        /// <summary>
<<<<<<< HEAD
        /// Tests that exporting the conversation exports conversation and applies the user filter.
        /// </summary>
        [TestMethod]
        public void TestOutputFileDataWithUserFilter()
        {
            ConversationExporter exporter = new ConversationExporter(path + "chat.txt", path + "chat_test_2.json");
            ConversationFilters filter = new ConversationFilters("bob", "", new string[] { "" });
            bool test = exporter.ExportConversation(filter);

            var serializedConversation = new StreamReader(new FileStream(path + "chat_test_2.json", FileMode.Open)).ReadToEnd();
            Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.IsTrue(test);

            Assert.AreEqual("My Conversation", savedConversation.name);

            var messages = savedConversation.messages.ToList();

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470901), messages[0].timestamp);
            Assert.AreEqual("bob", messages[0].sender.username);
            Assert.AreEqual("Hello there!", messages[0].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470906), messages[1].timestamp);
            Assert.AreEqual("bob", messages[1].sender.username);
            Assert.AreEqual("I'm good thanks, do you like pie?", messages[1].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[2].timestamp);
            Assert.AreEqual("bob", messages[2].sender.username);
            Assert.AreEqual("No, just want to know if there's anybody else in the pie society...", messages[2].content);
        }

        /// <summary>
        /// Tests that exporting the conversation exports conversation and applies the keyword filter.
        /// </summary>
        [TestMethod]
        public void TestOutputFileDataWithKeywordFilter()
        {
            ConversationExporter exporter = new ConversationExporter(path + "chat.txt", path + "chat_test_3.json");
            ConversationFilters filter = new ConversationFilters("", "no", new string[] { "" });
            bool test = exporter.ExportConversation(filter);

            var serializedConversation = new StreamReader(new FileStream(path + "chat_test_3.json", FileMode.Open)).ReadToEnd();
            Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.IsTrue(test);

            Assert.AreEqual("My Conversation", savedConversation.name);

            var messages = savedConversation.messages.ToList();

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470910), messages[0].timestamp);
            Assert.AreEqual("mike", messages[0].sender.username);
            Assert.AreEqual("no, let me ask Angus...", messages[0].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[1].timestamp);
            Assert.AreEqual("bob", messages[1].sender.username);
            Assert.AreEqual("No, just want to know if there's anybody else in the pie society...", messages[1].content);
        }

        /// <summary>
        /// Tests that exporting the conversation exports conversation and replaces the hidden words with replacement word.
        /// </summary>
        [TestMethod]
        public void TestOutputFileDataWithHiddenWordsFilter()
        {
            ConversationExporter exporter = new ConversationExporter(path + "chat.txt", path + "chat_test_4.json");
            ConversationFilters filter = new ConversationFilters("", "", new string[] { "NO","ask" });
            bool test = exporter.ExportConversation(filter);

            var serializedConversation = new StreamReader(new FileStream(path + "chat_test_4.json", FileMode.Open)).ReadToEnd();
            Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.IsTrue(test);

            Assert.AreEqual("My Conversation", savedConversation.name);

            var messages = savedConversation.messages.ToList();

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470901), messages[0].timestamp);
            Assert.AreEqual("bob", messages[0].sender.username);
            Assert.AreEqual("Hello there!", messages[0].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470905), messages[1].timestamp);
            Assert.AreEqual("mike", messages[1].sender.username);
            Assert.AreEqual("how are you?", messages[1].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470906), messages[2].timestamp);
            Assert.AreEqual("bob", messages[2].sender.username);
            Assert.AreEqual("I'm good thanks, do you like pie?", messages[2].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470910), messages[3].timestamp);
            Assert.AreEqual("mike", messages[3].sender.username);
            Assert.AreEqual("\\*redacted\\* let me \\*redacted\\* Angus...", messages[3].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470912), messages[4].timestamp);
            Assert.AreEqual("angus", messages[4].sender.username);
            Assert.AreEqual("Hell yes! Are we buying some pie?", messages[4].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[5].timestamp);
            Assert.AreEqual("bob", messages[5].sender.username);
            Assert.AreEqual("\\*redacted\\* just want to know if there's anybody else in the pie society...", messages[5].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470915), messages[6].timestamp);
            Assert.AreEqual("angus", messages[6].sender.username);
            Assert.AreEqual("YES! I'm the head pie eater there...", messages[6].content);
        }

        /// <summary>
        /// Tests that reading the conversation throws an exception when the directory is wrong, with the specified message.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ReadConversationExceptionDirectoryNotFound()
        {

            ConversationExporter exporter = new ConversationExporter(path + "\\vvv\\" + "chat.txt", path + "chat_output.json");
            ConversationFilters filter = new ConversationFilters("", "", new string[] { "" });
            exporter.ReadConversation();

            bool test = exporter.ExportConversation(filter);
            Assert.IsFalse(test);
=======
        /// Tests that reading the conversation throws an exception when the directory is wrong, with the specified message.
        /// </summary>
        [TestMethod]
        public void ReadConversationExceptionDirectoryNotFound()
        {
            try
            {
                new ConversationExporter().ReadConversation(path + "\\vvv\\" + "chat.txt");
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual("Directory path invalid.", e.Message);
            }
>>>>>>> origin/master
        }

        /// <summary>
        /// Tests that writing the conversation throws an exception when the directory is wrong, with the specified message.
        /// </summary>
        [TestMethod]
<<<<<<< HEAD
        [ExpectedException(typeof(ArgumentException))]
        public void WriteConversationExceptionDirectoryNotFound()
        {
            ConversationExporter exporter = new ConversationExporter(path + "chat.txt", path + "\\vvv\\" + "chat_output.json");
            ConversationFilters filter = new ConversationFilters("", "", new string[] { "" });
            exporter.WriteConversation(new Conversation());

            bool test = exporter.ExportConversation(filter);
            Assert.IsFalse(test);
=======
        public void WriteConversationExceptionDirectoryNotFound()
        {
            try
            {
                ConversationExporterConfiguration configuration = new ConversationExporterConfiguration(path + "chat.txt", path + "\\vvv\\" + "chat_output.json");
                new ConversationExporter().ExportConversation(configuration);
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual("Directory path invalid.", e.Message);
            }
>>>>>>> origin/master
        }


        /// <summary>
        /// Tests that reading the conversation throws an exception when the file name is empty, with the specified message.
        /// </summary>
        [TestMethod]
<<<<<<< HEAD
        [ExpectedException(typeof(ArgumentException))]
        public void ReadConversationExceptionFileNameEmpty()
        {
            ConversationExporter exporter = new ConversationExporter(path + "", path + "chat_output.json");
            ConversationFilters filter = new ConversationFilters("", "", new string[] { "" });
            exporter.ReadConversation();

            bool test = exporter.ExportConversation(filter);
            Assert.IsFalse(test);
=======
        public void ReadConversationExceptionFileNameEmpty()
        {
            try
            {
                new ConversationExporter().ReadConversation(path + "");
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual("Directory path invalid.", e.Message);
            }
>>>>>>> origin/master
        }

        /// <summary>
        /// Tests that writing the conversation throws an exception when the directory is wrong, with the specified message.
        /// </summary>
        [TestMethod]
<<<<<<< HEAD
        [ExpectedException(typeof(ArgumentException))]
        public void WriteConversationExceptionFileNameEmpty()
        {
            ConversationExporter exporter = new ConversationExporter(path + "chat.txt", path + "");
            ConversationFilters filter = new ConversationFilters("", "", new string[] { "" });
            exporter.WriteConversation(new Conversation());

            bool test = exporter.ExportConversation(filter);
            Assert.IsFalse(test);
=======
        public void WriteConversationExceptionFileNameEmpty()
        {
            try
            {
                ConversationExporterConfiguration configuration = new ConversationExporterConfiguration(path + "chat.txt", path + "");
                new ConversationExporter().ExportConversation(configuration);
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual("Directory path invalid.", e.Message);
            }
>>>>>>> origin/master
        }

        /// <summary>
        /// Tests that reading the conversation throws an exception when input file does not exist, with the specified message.
        /// </summary>
        [TestMethod]
<<<<<<< HEAD
        [ExpectedException(typeof(ArgumentException))]
        public void ReadConversationExceptionFileNotFound()
        {

            ConversationExporter exporter = new ConversationExporter(path + "chatVVV.txt", path + "chat_output.json");
            ConversationFilters filter = new ConversationFilters("", "", new string[] { "" });
            exporter.ReadConversation();

            bool test = exporter.ExportConversation(filter);
            Assert.IsFalse(test);
=======
        public void ReadConversationExceptionFileNotFound()
        {
            try
            {
                new ConversationExporter().ReadConversation(path + "chatVVV.txt");
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual("The file was not found.", e.Message);
            }
>>>>>>> origin/master
        }

        /// <summary>
        /// Tests that reading the conversation throws an exception when no read permission is given to the input file, with the specified message.
        /// </summary>
        [TestMethod]
<<<<<<< HEAD
        [ExpectedException(typeof(ArgumentException))]
        public void ReadConversationExceptionFilePermission()
        {
            StreamReader reader = new StreamReader(new FileStream(path + "chat.txt", FileMode.Open, FileAccess.Read), Encoding.ASCII);

            StreamWriter writer = new StreamWriter(new FileStream(path + "chat_test_5.txt", FileMode.Create, FileAccess.Write));
            writer.Write(reader.ReadToEnd());
            writer.Flush();
            writer.Close();

            ConversationExporter exporter = new ConversationExporter(path + "chat_text_5.txt", path + "chat_output.json");
            ConversationFilters filter = new ConversationFilters("", "", new string[] { "" });
            exporter.ReadConversation();

            bool test = exporter.ExportConversation(filter);
            Assert.IsFalse(test);
=======
        public void ReadConversationExceptionFilePermission()
        {
            try
            {
                StreamReader reader = new StreamReader(new FileStream(path + "chat.txt", FileMode.Open, FileAccess.Read), Encoding.ASCII);

                StreamWriter writer = new StreamWriter(new FileStream(path + "chat_test_5.txt", FileMode.Create, FileAccess.Write));
                writer.Write(reader.ReadToEnd());
                writer.Flush();
                writer.Close();

                new ConversationExporter().ReadConversation(path + "chat_test_5.txt");
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual("No permission to file.", e.Message);
            }
>>>>>>> origin/master
        }

        /// <summary>
        /// Tests that reading the conversation throws an exception when the timestamp format is wrong, with the specified message.
        /// </summary>
        [TestMethod]
<<<<<<< HEAD
        [ExpectedException(typeof(ArgumentException))]
        public void ReadConversationExceptionTimestampFormat()
        {
            string testConversation = "My Test Conversation\nVVV bob Hello there!";
            StreamWriter writer = new StreamWriter(new FileStream(path + "chat_test_6.txt", FileMode.Create, FileAccess.Write));
            writer.Write(testConversation);
            writer.Flush();
            writer.Close();

            ConversationExporter exporter = new ConversationExporter(path + "chat_test_6.txt", path + "chat_output.json");
            ConversationFilters filter = new ConversationFilters("", "", new string[] { "" });
            exporter.ReadConversation();

            bool test = exporter.ExportConversation(filter);
            Assert.IsFalse(test);
=======
        public void ReadConversationExceptionTimestampFormat()
        {
            try
            {
                string testConversation = "My Test Conversation\nVVV bob Hello there!";
                StreamWriter writer = new StreamWriter(new FileStream(path + "chat_test_6.txt", FileMode.Create, FileAccess.Write));
                writer.Write(testConversation);
                writer.Flush();
                writer.Close();

                new ConversationExporter().ReadConversation(path + "chat_test_6.txt");
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual("The format of the timestamp is wrong.", e.Message);
            }
>>>>>>> origin/master
        }

        /// <summary>
        /// Tests that reading the conversation throws an exception when the message contains less arguments, with the specified message.
        /// </summary>
        [TestMethod]
<<<<<<< HEAD
        [ExpectedException(typeof(ArgumentException))]
        public void ReadConversationExceptionMessageLessArguments()
        {
            string testConversation = "My Test Conversation\n1448470901";
            StreamWriter writer = new StreamWriter(new FileStream(path + "chat_test_7.txt", FileMode.Create, FileAccess.Write));
            writer.Write(testConversation);
            writer.Flush();
            writer.Close();

            ConversationExporter exporter = new ConversationExporter(path + "chat_test_7.txt", path + "chat_output.json");
            ConversationFilters filter = new ConversationFilters("", "", new string[] { "" });
            exporter.ReadConversation();

            bool test = exporter.ExportConversation(filter);
            Assert.IsFalse(test);
        }

        /// <summary>
        /// Tests that reading an empty file will return null and application will terminate.
        /// </summary>
        [TestMethod]
        public void ReadConversationEmptyFile()
        {
            string testConversation = "";
            StreamWriter writer = new StreamWriter(new FileStream(path + "chat_test_8.txt", FileMode.Create, FileAccess.Write));
            writer.Write(testConversation);
            writer.Flush();
            writer.Close();

            ConversationExporter exporter = new ConversationExporter(path + "chat_test_8.txt", path + "chat_output.json");
            ConversationFilters filter = new ConversationFilters("", "", new string[] { "" });

            bool test = exporter.ExportConversation(filter);
            Assert.IsFalse(test);
=======
        public void ReadConversationExceptionMessageLessArguments()
        {
            try
            {
                string testConversation = "My Test Conversation\n1448470901 ";
                StreamWriter writer = new StreamWriter(new FileStream(path + "chat_test_7.txt", FileMode.Create, FileAccess.Write));
                writer.Write(testConversation);
                writer.Flush();
                writer.Close();

                new ConversationExporter().ReadConversation(path + "chat_test_7.txt");
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual("Something went wrong while reading a message.", e.Message);
            }
>>>>>>> origin/master
        }

    }
}


