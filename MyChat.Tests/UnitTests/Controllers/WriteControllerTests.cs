namespace MindLink.Recruitment.MyChat.Tests.UnitTests.Controllers
{
    using MindLink.Recruitment.MyChat.Controllers;
    using MindLink.Recruitment.MyChat.Interfaces.ControllerInterfaces;
    using MyChatModel.ModelData;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    [TestFixture]
    public class WriteControllerTests
    {

        // IwriteController to be tested
        private IWriteController writeController;
        // Conversation which the writeController will attempt to fill
        private Conversation conversation;

        /// <summary>
        /// Tests whether the WriteController performs as expected
        /// </summary>
        [Test]
        public void WriteControllerWritesFile() 
        {

            // INITIALISE the writeController to be tested
            writeController = new WriteController();
            MakeConverSation();
            // SET the conversation to the return value of the writeController 
            writeController.WriteConversation(conversation, "outputTest.json");
            // INITIALISE a list of messages to read
            IList<Message> msgs = conversation.Messages.ToList();

            var serializedConversation = new StreamReader(new FileStream("outputTest.json", FileMode.Open)).ReadToEnd();

            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.That(msgs[0].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470901)));
            Assert.That(msgs[0].SenderId, Is.EqualTo("bob"));
            Assert.That(msgs[0].Content, Is.EqualTo("Hello there!"));

            Assert.That(msgs[1].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470905)));
            Assert.That(msgs[1].SenderId, Is.EqualTo("mike"));
            Assert.That(msgs[1].Content, Is.EqualTo("how are you?"));

            Assert.That(msgs[2].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470906)));
            Assert.That(msgs[2].SenderId, Is.EqualTo("bob"));
            Assert.That(msgs[2].Content, Is.EqualTo("I'm good thanks, do you like pie?"));

            Assert.That(msgs[3].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470910)));
            Assert.That(msgs[3].SenderId, Is.EqualTo("mike"));
            Assert.That(msgs[3].Content, Is.EqualTo("no, let me ask Angus..."));

            Assert.That(msgs[4].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470912)));
            Assert.That(msgs[4].SenderId, Is.EqualTo("angus"));
            Assert.That(msgs[4].Content, Is.EqualTo("Hell yes! Are we buying some pie?"));

            Assert.That(msgs[5].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470914)));
            Assert.That(msgs[5].SenderId, Is.EqualTo("bob"));
            Assert.That(msgs[5].Content, Is.EqualTo("No, just want to know if there's anybody else in the pie society..."));

            Assert.That(msgs[6].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470915)));
            Assert.That(msgs[6].SenderId, Is.EqualTo("angus"));
            Assert.That(msgs[6].Content, Is.EqualTo("YES! I'm the head pie eater there..."));

        }

        /// <summary>
        /// Tests whether the WriteController throws an exception when it can't find the directory to
        /// save a file to
        /// </summary>
        [Test]
        public void WriteControllerThrowsDirectoryNotFoundException() 
        {
            // INITIALISE the writeController to be tested
            writeController = new WriteController();
            MakeConverSation();


            Assert.That(() => writeController.WriteConversation(conversation, "NonExistentDirectory/outputTest.json"),
                Throws.Exception.TypeOf<ArgumentException>().With.InnerException.TypeOf<DirectoryNotFoundException>());

            var exception = Assert.Throws<ArgumentException>(() => writeController.WriteConversation(conversation, "NonExistentDirectory/outputTest.json"));

            Assert.That(exception.Message, Is.EqualTo("Path NonExistentDirectory/outputTest.json is invalid"));
        }

        private void MakeConverSation()
        {
            // IList of type Message called msgs
            IList<Message> messages = new List<Message>();

            messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(1448470901)), "bob", "Hello there!"));
            messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(1448470905)), "mike", "how are you?"));
            messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(1448470906)), "bob", "I'm good thanks, do you like pie?"));
            messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(1448470910)), "mike", "no, let me ask Angus..."));
            messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(1448470912)), "angus", "Hell yes! Are we buying some pie?"));
            messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(1448470914)), "bob", "No, just want to know if there's anybody else in the pie society..."));
            messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(1448470915)), "angus", "YES! I'm the head pie eater there..."));

            conversation = new Conversation("Test conversation", messages);

        }
    }
}
