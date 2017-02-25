using System.IO;
using System.Linq;
using MyChat;
using Newtonsoft.Json;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MindLink.Recruitment.MyChat.Tests
{
    
    using System;
    using NUnit.Framework;

    /// <summary>
    /// Tests for the <see cref="ConversationExporter"/>.
    /// </summary>
    [TestFixture]
    public class ConversationExporterTests
    {



        /// <summary>
        /// Tests that exporting the conversation exports conversation.
        /// </summary>
        [TestCase(@"chat.txt", @"chat0.json", null, "", "", "test0")]
        [TestCase(@"chat.txt", @"chat1.json", "bob",  "", "", "test1")]
        [TestCase(@"chat.txt", @"chat2.json", "bob", "no", "", "test2")]
        [TestCase(@"chat.txt", @"chat3.json", "bob", "no", "want", "test3")]
        //[TestCase(@"chat_wrong.txt", @"chat3.json", "", "", "", "ExceptionFile", ExpectedException = typeof(FileNotFoundException), ExpectedMessage = "File not found")]

        public void ExportingConversationExportsConversation(string input, string output, string user = "", string keyword = "", string blacklist = "", string test = "test0")
        {
            ConversationExporter exporter = new ConversationExporter();
            ProgramArguments args = new ProgramArguments() { inputFile = AppDomain.CurrentDomain.BaseDirectory + input, outFile = AppDomain.CurrentDomain.BaseDirectory + output, user = user, keyboard = keyword, blacklist = blacklist };
            ConversationExporterConfiguration conf = new ConversationExporterConfiguration(args);
            exporter.ExportConversation(conf);
            
            var serializedConversation = new StreamReader(new FileStream(AppDomain.CurrentDomain.BaseDirectory + output, FileMode.Open)).ReadToEnd();
            Output savedConversation = JsonConvert.DeserializeObject<Output>(serializedConversation);
            Assert.AreEqual("My Conversation", savedConversation.conversation.name);

            var messages = savedConversation.conversation.messages.ToList();
            switch (test)
            {
                case "test1":
                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470901), messages[0].timestamp);
                Assert.AreEqual("bob", messages[0].senderId);
                Assert.AreEqual("Hello there!", messages[0].content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470906), messages[1].timestamp);
                Assert.AreEqual("bob", messages[1].senderId);
                Assert.AreEqual("I'm good thanks, do you like pie?", messages[1].content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[2].timestamp);
                Assert.AreEqual("bob", messages[2].senderId);
                Assert.AreEqual("No, just want to know if there's anybody else in the pie society...", messages[2].content);
                    break;
                case "test2":
                   
                    Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[0].timestamp);
                    Assert.AreEqual("bob", messages[0].senderId);
                    Assert.AreEqual("No, just want to know if there's anybody else in the pie society...", messages[0].content);
                    break;
                case "test3":

                    Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[0].timestamp);
                    Assert.AreEqual("bob", messages[0].senderId);
                    Assert.AreEqual("No, just *redacted* to know if there's anybody else in the pie society...".ToLower(), messages[0].content);
                    break;
                case "ExceptionFile":
                    
                    break;
                default:
           
                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470901), messages[0].timestamp);
                Assert.AreEqual("bob", messages[0].senderId);
                Assert.AreEqual("Hello there!", messages[0].content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470905), messages[1].timestamp);
                Assert.AreEqual("mike", messages[1].senderId);
                Assert.AreEqual("how are you?", messages[1].content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470906), messages[2].timestamp);
                Assert.AreEqual("bob", messages[2].senderId);
                Assert.AreEqual("I'm good thanks, do you like pie?", messages[2].content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470910), messages[3].timestamp);
                Assert.AreEqual("mike", messages[3].senderId);
                Assert.AreEqual("no, let me ask Angus...", messages[3].content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470912), messages[4].timestamp);
                Assert.AreEqual("angus", messages[4].senderId);
                Assert.AreEqual("Hell yes! Are we buying some pie?", messages[4].content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[5].timestamp);
                Assert.AreEqual("bob", messages[5].senderId);
                Assert.AreEqual("No, just want to know if there's anybody else in the pie society...", messages[5].content);

                Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470915), messages[6].timestamp);
                Assert.AreEqual("angus", messages[6].senderId);
                Assert.AreEqual("YES! I'm the head pie eater there...", messages[6].content);
                    break;
            }
        }
    }
}
