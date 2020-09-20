namespace MindLink.Recruitment.MyChat.Tests.UnitTests.Controllers
{
    using MindLink.Recruitment.MyChat.Controllers;
    using MindLink.Recruitment.MyChat.Interfaces.ControllerInterfaces;
    using MyChatModel.ModelData;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    [TestFixture]
    public class ReadControllerTests
    {
        // IReadController to be tested
        private IReadController readController;
        // Conversation which the readController will attempt to fill
        private Conversation conversation;

        /// <summary>
        /// Test to see if the conversation is read correctly from the .txt file
        /// </summary>
        [Test]
        public void ReadControllerReadsFile() 
        {
            // INITIALISE the readController to be tested
            readController = new ReadController();
            // SET the conversation to the return value of the ReadController 
            conversation = readController.ReadConversation("chat.txt");
            // INITIALISE a list of messages to read
            IList<Message> msgs = conversation.Messages.ToList();

            // Tests to see if the file is read as intended 

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
        /// Tests whether the ReadController throws exceptions where expected 
        /// </summary>
        [Test]
        public void ReadControllerThrowsFileNotFoundException() 
        {
            // INITIALISE the readController to be tested
            readController = new ReadController();

            Assert.That(() => (conversation = readController.ReadConversation("NoFile.txt")), 
                Throws.Exception.
                TypeOf<ArgumentException>().With.
                InnerException.TypeOf<FileNotFoundException>());

            var exception = Assert.Throws<ArgumentException>(() => readController.ReadConversation("NoFile.txt"));

            Assert.That(exception.Message, Is.EqualTo("The file was not found."));
        }

    }
}
