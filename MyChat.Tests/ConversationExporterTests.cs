using System.IO;
using System.Linq;
using System.Text;
using MyChat;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MindLink.Recruitment.MyChat.Tests
{
    using System;

    [TestFixture]
    public class ConversationExporterTests
    {
        [Test]
        public void TitleOfExportedConversationIsCorrect()
        {
            var exporter = new ConversationExporter();

            var reader = exporter.GetStreamReader("chat.txt", FileMode.Open,
                FileAccess.Read, Encoding.ASCII);

            var writer = exporter.GetStreamWriter("output.json", FileMode.Create, FileAccess.ReadWrite);

            exporter.WriteConversation(writer, exporter.ExtractConversation(reader), "chat.json");

            var serializedConversation = new StreamReader(new FileStream("chat.json", FileMode.Open)).ReadToEnd();

            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.That(savedConversation.Name, Is.EqualTo("My Conversation"));
        }

        [Test]
        public void CorrectArgsReturnsCorrectStreamReader()
        {
            var cp = new ConversationExporter();
            var got = cp.GetStreamReader("chat.txt", FileMode.Create, FileAccess.ReadWrite, Encoding.ASCII);
            Assert.That(got, Is.TypeOf<StreamReader>());
        }

        // [Test]
        // public void NonExistentDirectoryToStreamReaderThrowsError()
        // {
        //TODO: fill test.
        // }

        [Test]
        public void CorrectArgsReturnsCorrectStreamWriter()
        {
            var cp = new ConversationExporter();
            var got = cp.GetStreamWriter("output.something", FileMode.Create, FileAccess.ReadWrite);
            Assert.That(got, Is.TypeOf<StreamWriter>());
        }

        [Test]
        public void ArrayToMessageTakesArrayAndReturnsMessageObject()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "a message"};
            var got = cp.ArrayToMessage(line);
            Assert.That(got, Is.TypeOf<Message>());

        }

        [Test]
        public void SenderIDCorrectAfterConversionIntoMessage()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "a message"};
            var got = cp.ArrayToMessage(line).senderId;
            var want = "david";
            Assert.That(got, Is.EqualTo(want));
        }

        [Test]
        public void MessageContentCorrectAfterConversion()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "a message"};
            var got = cp.ArrayToMessage(line).content;
            var want = "a message";
            Assert.That(got, Is.EqualTo(want));
        }

        [Test]
        public void TimestampCorrectWhenCreatingNewMessage()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1448470901", "david", "a message"};
            var got = cp.ArrayToMessage(line).timestamp;
            DateTimeOffset want = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470901"));
            Assert.That(got, Is.EqualTo(want));
        }

        [Test]
        public void StringToUnixTimeStampCorrrectlyFormatsTimeStamp()
        {
            ConversationExporter cp = new ConversationExporter();
            DateTimeOffset got = cp.StringToUnixTimeStamp("1448470901");
            DateTimeOffset want = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470901"));
            Assert.That(got, Is.EqualTo(want));
        }

        [Test]
        public void NameInSenderReturnsTrue()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "a message"};

            var message = cp.ArrayToMessage(line);

            Assert.That(cp.StringEqual(message.senderId, "david"), Is.EqualTo(true));
        }

        [Test]
        public void ShorterQueryContainedWithinLongerNameIsFalse()
        {
            // Assumption: a shorter query contained within a longer
            // name will fail. 
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "davide", "a message"};

            var message = cp.ArrayToMessage(line);

            Assert.That(cp.StringEqual(message.senderId, "david"), Is.EqualTo(false));
        }

        [Test]
        public void SimilarNameNotCaught()
        {    // If someone searches for "davide" and we find the name is "david"
            // that should fail.
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "a message"};

            var message = cp.ArrayToMessage(line);

            Assert.That(cp.StringEqual(message.senderId, "davide"), Is.EqualTo(false));
        }
        
        [Test]
        public void NameNotInSenderReturnsFalse()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "a message"};

            var message = cp.ArrayToMessage(line);

            Assert.That(cp.StringEqual("notInSender", message.senderId), Is.EqualTo(false));
        }
    }

}
