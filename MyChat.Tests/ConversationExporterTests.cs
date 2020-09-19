namespace MindLink.Recruitment.MyChat.Tests
{
    using MindLink.Recruitment.MyChat.Controllers;
    using MindLink.Recruitment.MyChat.Interfaces.ControllerInterfaces;
    using MyChatModel.ModelData;
    using System;
    using System.IO;
    using System.Linq;
    using Newtonsoft.Json;
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
        [Test]
        public void ExportingConversationExportsConversation()
        {
           
            // INITIALISE the ExportController for testing
            IExportController exporter = new ExportController(new ReadController(), new WriteController(), new FilterController(), new ReportController());

            exporter.ExportConversation("chat.txt", "chat.json");

            var serializedConversation = new StreamReader(new FileStream("chat.json", FileMode.Open)).ReadToEnd();

            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.That(savedConversation.Name, Is.EqualTo("My Conversation"));

            var messages = savedConversation.Messages.ToList();

            Assert.That(messages[0].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470901)));
            Assert.That(messages[0].SenderId, Is.EqualTo("bob"));
            Assert.That(messages[0].Content, Is.EqualTo("Hello there!"));

            Assert.That(messages[1].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470905)));
            Assert.That(messages[1].SenderId, Is.EqualTo("mike"));
            Assert.That(messages[1].Content, Is.EqualTo("how are you?"));

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
    }
}
