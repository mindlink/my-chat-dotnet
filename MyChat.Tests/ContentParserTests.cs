namespace MindLink.Recruitment.MyChat.Tests
{
    using global::MyChat;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Tests for the <see cref="ContentParser"/>.
    /// </summary>
    [TestClass]
    public class ContentParserTests
    {
        /// <summary>
        /// Tests that the message content can be parsed correctly given its structure.
        /// </summary>
        [TestMethod]
        public void ContentParserSimpleTest()
        {
            // set a simple string with the given message format.
            string simpleString = "[TIMESTAMP] [USERNAME] this is used for testing";

            // split with whitespace
            string[] testData = simpleString.Split(' ');

            // parse the splitted string.
            string result = ContentParser.ParseConversationContent(testData);

            // check if string is as expected.
            Assert.AreEqual("this is used for testing", result);
        }

        /// <summary>
        /// Tests that the message content containing special characters can be parsed correctly.
        /// </summary>
        [TestMethod]
        public void ContentParserSpecialCharactersTest()
        {
            // set a complex string containing special characters.
            string simpleString = @"[TIMESTAMP] [USERNAME] & this < is % used / for \ testing@";

            // split with whitespace
            string[] testData = simpleString.Split(' ');

            // parse the splitted string.
            string result = ContentParser.ParseConversationContent(testData);

            // check if string is as expected.
            Assert.AreEqual(@"& this < is % used / for \ testing@", result);
        }

        /// <summary>
        /// Tests that the message content containing blacklist words can be parsed correctly and blacklisted words are masked with '*'.
        /// </summary>
        [TestMethod]
        public void ContentParserBlackListTest()
        {
            // set a simple string containing the word to blacklist.
            string simpleString = @"[TIMESTAMP] [USERNAME] one word are blacklisted here.";

            // split with whitespace
            string[] testData = simpleString.Split(' ');

            // set the blacklist word to "word".
            string result = ContentParser.ParseConversationContent(testData, "word");

            // check if string is as expected.
            Assert.AreEqual(@"one **** are blacklisted here.", result);
        }

        /// <summary>
        /// Tests that the message content containing multiple blacklist words can be parsed correctly and blacklisted words are masked with '*'.
        /// </summary>
        [TestMethod]
        public void ContentParserMultipleBlackListTest()
        {
            // set a simple string containing the word to blacklist.
            string simpleString = @"[TIMESTAMP] [USERNAME] more than one word is blacklisted here.";

            // split with whitespace
            string[] testData = simpleString.Split(' ');

            // set the blacklist words to "more" and "word".
            string result = ContentParser.ParseConversationContent(testData, "more, word");

            Assert.AreEqual(@"**** than one **** is blacklisted here.", result);
        }

        /// <summary>
        /// Tests that the message content containing sensitive numbers can be parsed correctly those numbers are masked with '*'.
        /// </summary>
        [TestMethod]
        public void HiddenNumbersTest()
        {
            // Instanciate ConversationExporter.
            ConversationExporter exporter = new ConversationExporter();

            // Export the conversation in JSON file.
            exporter.ExportConversation("chat.txt", "chat.json", null, null, null, false, true);

            // Read the JSON file generated
            FileStream stream = new FileStream("chat.json", FileMode.Open);

            // using statement to correctly dispose StreamReader
            using (StreamReader reader = new StreamReader(stream))
            {
                string serializedConversation = reader.ReadToEnd();

                // Deserialize JSON data to Conversation object.
                Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

                var messages = savedConversation.Messages.ToList();

                Assert.AreEqual("Call me on ************ to give me your credit card details.", messages[7].Content);
                Assert.AreEqual("Its ************5100", messages[8].Content);
                Assert.AreEqual("Mine is ************1881", messages[9].Content);
            }
        }
    }
}
