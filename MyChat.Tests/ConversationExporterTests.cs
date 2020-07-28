using System.IO;
using System.Linq;
using MyChat;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MindLink.Recruitment.MyChat.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Text;

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

            exporter.ExportConversation("chat.txt", "export.txt");

            var serializedConversation = new StreamReader(new FileStream("export.txt", FileMode.Open)).ReadToEnd();

            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.That(savedConversation.Name, Is.EqualTo("My Conversation"));

            var messages = savedConversation.Messagez.ToList();

            Assert.That(messages[0].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470901)));
            Assert.That(messages[0].SenderId, Is.EqualTo("bob"));
            Assert.That(messages[0].Content, Is.EqualTo("Hello there!"));

            Assert.That(messages[0].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470905)));
            Assert.That(messages[0].SenderId, Is.EqualTo("mike"));
            Assert.That(messages[0].Content, Is.EqualTo("how are you?"));

            Assert.That(messages[2].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470906)));
            Assert.That(messages[2].SenderId, Is.EqualTo("bob"));
            Assert.That(messages[2].Content, Is.EqualTo("I'm good thanks, do you like pie?"));

            Assert.That(messages[3].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470910)));
            Assert.That(messages[3].SenderId, Is.EqualTo("mike"));
            Assert.That(messages[3].Content, Is.EqualTo("no, let me ask Angus..."));

            Assert.That(messages[4].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470912)));
            Assert.That(messages[4].SenderId, Is.EqualTo("angus"));
            Assert.That(messages[4].Content, Is.EqualTo("Hell yes! Are we buying some pie?"));

            Assert.That(messages[5].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470914)));
            Assert.That(messages[5].SenderId, Is.EqualTo("bob"));
            Assert.That(messages[5].Content, Is.EqualTo("No, just want to know if there's anybody else in the pie society..."));

            Assert.That(messages[6].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470915)));
            Assert.That(messages[6].SenderId, Is.EqualTo("angus"));
            Assert.That(messages[6].Content, Is.EqualTo("YES! I'm the head pie eater there..."));
        }

        /// <summary>
        /// Tests that finding the user works.
        /// </summary>
        [Test]
        public void FindUser()
        {
            var serializedConversation = new StreamReader(new FileStream("export1.txt", FileMode.Open)).ReadToEnd();

            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);
            var user = "mike";
           
            foreach (var value in savedConversation.Messagez)
            {
                //if the sender is the same as teh command line argument,convert to json,write the result to file - values are multiplied because the previous method Read is not  showing everything(to be redacted)
                if (value.SenderId == user)
                {
                    var result = value.Content;

                }
                
                Assert.That(value.Content, Is.EqualTo("how are you?"));

                Assert.AreNotEqual(value.Content, "how ");

                //assert exceptions -add tests

            }
        }
        /// <summary>
        /// Tests that the command-line word s in the conversation.
        /// </summary>

        [Test]
        public void SearchWord()
        {
            var serializedConversation = new StreamReader(new FileStream("export2.txt", FileMode.Open)).ReadToEnd();

            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);
            string randomWord = "pie";
            string wordNotThere = "lol";
            var messageList = from x in savedConversation.Messagez
                              where x.Content.Contains(randomWord)
                              select x;
            // StringBuilder sb = new StringBuilder();
            foreach (Message x in messageList)
            {
                if (x.Content.Contains(randomWord))
                {
                    Assert.That(x.Content, Is.EqualTo("you like pie?"));
                    Assert.That(x.Content, Is.EqualTo("buying some pie?"));
                }
                if (x.Content.Contains(wordNotThere))
                {
                    Assert.AreNotEqual(x.Content, "how are you");
                }
            }
        }

        /// <summary>
        /// Tests that if a word is in blacklist and test if a word is redacted.
        /// </summary>

        [Test]
        public void RedactWord()
        {
            var serializedConversation = new StreamReader(new FileStream("export3.txt", FileMode.Open)).ReadToEnd();

            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            //create a list that holds the blacklist words
            List<string> blacklist = new List<string>();
            // read the blacklist file
            string[] lines = System.IO.File.ReadAllLines("blacklist.txt");
            //add words to the list
            foreach (string line in lines)
            {
                blacklist.Add(line);
                //test all lines are read correctly and added to file
                Assert.That(line[0], Is.EqualTo("pie"));
                Assert.That(line[1], Is.EqualTo("fuck"));
                //test that random word is not there
                Assert.AreNotEqual(line[1], "shit");

            }

            //loop through blacklist,check if it contains bad word
            foreach (string blacklistWord in blacklist)
            {

                var listRedacted = from j in savedConversation.Messagez
                                   where j.Content.Contains(blacklistWord)
                                   select j;
                Assert.That(listRedacted, Is.EqualTo("pie"));

                StringBuilder sb2 = new StringBuilder();
                foreach (Message toBeRedacted in listRedacted)
                {
                    sb2.Append("UserName: " + toBeRedacted.SenderId + "Message: " + toBeRedacted.Content);


                }
                //test list
                List<string> testList = new List<string>();
                string[] redactedLines = System.IO.File.ReadAllLines("redactedConversation.txt");
                //test if there are redacted words
                foreach (string l in redactedLines)
                {
                    testList.Add(l);
                    Assert.That(l[0], Is.EqualTo("UserName: bobMessage: you like *redacted*?"));

                }

            }
        }
}
}
