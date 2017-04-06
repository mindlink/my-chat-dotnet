namespace MindLink.Recruitment.MyChat.Tests
{
    using global::MyChat;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Tests for the <see cref="Encryption"/> and <see cref="UserNameEncryption"/>
    /// </summary>
    [TestClass]
    public class EncryptionTests
    {
        /// <summary>
        /// Tests that encryption and decryption functions properly.
        /// </summary>
        [TestMethod]
        public void EncryptionTest()
        {
            string test1 = Encryption.Encrypt("TestString");
            string test2 = Encryption.Encrypt(@"*\(&^#/%$@");
            string test3 = Encryption.Encrypt("9q3d8h5679");
            string test4 = Encryption.Encrypt("90c5138y5*&%*&%(");
            string test5 = Encryption.Encrypt("οφδσηξ0f49wu8*&%");
            string test6 = Encryption.Encrypt("οφδσηξ0f49wu8*&%");


            // simple string.
            Assert.AreEqual("TestString", Encryption.Decrypt(test1));

            // special characters.
            Assert.AreEqual(@"*\(&^#/%$@", Encryption.Decrypt(test2));

            // alphanumberic.
            Assert.AreEqual("9q3d8h5679", Encryption.Decrypt(test3));

            // mixed/
            Assert.AreEqual("90c5138y5*&%*&%(", Encryption.Decrypt(test4));

            // even more mixed.
            Assert.AreEqual("οφδσηξ0f49wu8*&%", Encryption.Decrypt(test5));

            // tests that same value is encrypted to same so as to be able to identify identical values even when encrypted.
            Assert.AreEqual(test5, test6);
        }

        /// <summary>
        /// Tests that encryption and decryption functions properly within the generated messages.
        /// </summary>
        [TestMethod]
        public void UsernameEncryptionTest()
        {
            // Instanciate ConversationExporter.
            ConversationExporter exporter = new ConversationExporter();

            // Export the conversation in JSON file.
            exporter.ExportConversation("chat.txt", "chat.json", null, null, null, true, false);

            // Read the JSON file generated
            FileStream stream = new FileStream("chat.json", FileMode.Open);

            // using statement to correctly dispose StreamReader
            using (StreamReader reader = new StreamReader(stream))
            {
                string serializedConversation = reader.ReadToEnd();

                // Deserialize JSON data to Conversation object.
                Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

                var messages = savedConversation.Messages.ToList();

                Assert.AreEqual("bob", Encryption.Decrypt(messages[0].SenderId));
                Assert.AreEqual("mike", Encryption.Decrypt(messages[1].SenderId));
                Assert.AreEqual("angus", Encryption.Decrypt(messages[4].SenderId));
            }
        }

    }
}
