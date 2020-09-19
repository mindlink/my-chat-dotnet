using System.IO;
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
            
            var emptyConfig = new ConversationExporterConfiguration();

            var reader = exporter.GetStreamReader("chat.txt", FileMode.Open,
                FileAccess.Read, Encoding.ASCII);

            var writer = exporter.GetStreamWriter("output.json", FileMode.Create, FileAccess.ReadWrite);

            exporter.WriteConversation(writer, exporter.ExtractConversation(reader, emptyConfig), "chat.json");

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
        //
        // // [Test]
        // // public void NonExistentDirectoryToStreamReaderThrowsError()
        // // {
        // //TODO: fill test.
        // // }
        //
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
        
            Assert.That(cp.UsernameFound(message, "david"), Is.EqualTo(true));
        }
        
        [Test]
        public void ShorterNameContainedWithinLongerNameIsNotFound()
        {
            // If someone searches for "davide" and we find the name is "david"
            // that should fail. 
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "davide", "a message"};
        
            var message = cp.ArrayToMessage(line);
        
            Assert.That(cp.UsernameFound(message, "david"), Is.EqualTo(false));
        }
        
        [Test]
        public void NameNotInSenderReturnsFalse()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "a message"};
        
            var message = cp.ArrayToMessage(line);
        
            Assert.That(cp.KeywordInMessage(message, "notInSender"), Is.EqualTo(false));
        }
        
        [Test]
        public void KeywordInMessageReturnsTrue()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "this is a message"};
        
            var message = cp.ArrayToMessage(line);
        
            Assert.That(cp.KeywordInMessage(message, "message"), Is.EqualTo(true));
        }
        
        [Test]
        public void KeywordNotInMessageReturnsFalse()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "this is a message"};
        
            var message = cp.ArrayToMessage(line);
        
            Assert.That(cp.KeywordInMessage(message, "nonexistent"), Is.EqualTo(false));
        }
        
        [Test]
        public void ShortKeywordNotFoundInLongerWordReturns()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "shorter word is found within"};
        
            var message = cp.ArrayToMessage(line);
        
            Assert.That(cp.KeywordInMessage(message, "with"), Is.EqualTo(false));
        }
        
        [Test]
        public void CapitalKeywordFindsLowercaseKeyword()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "Shorter word is found within"};
        
            var message = cp.ArrayToMessage(line);
        
            Assert.That(cp.KeywordInMessage(message, "shorter"), Is.EqualTo(true));
        }
        
        [Test]
        public void KeywordWithinAnotherWordNotFound()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "hereisalongword found within"};
        
            var message = cp.ArrayToMessage(line);
        
            Assert.That(cp.KeywordInMessage(message, "hereisalongwordplusextra"), Is.EqualTo(false));
        }
        
        [Test]
        public void MessageContainingPartOfKeywordIsFalse()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "thisisalongstring"};
        
            var message = cp.ArrayToMessage(line);
        
            Assert.That(cp.KeywordInMessage(message, "string"), Is.EqualTo(false));
        }
        
        [Test]
        public void BannedWordRemovedFromMessage()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "banned word is found"};
            var message = cp.ArrayToMessage(line);
            var bt = "word";
            Assert.That(cp.SanitiseMessage(message, "word").content, Is.EqualTo("banned *redacted* is found"));
        }
        
        [Test]
        public void NonExistentBannedTermNotRemovedFromMessage()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "banned term is not found"};
            var message = cp.ArrayToMessage(line);
            var bt = "word";
            Assert.That(cp.SanitiseMessage(message, "other").content, Is.EqualTo("banned term is not found"));
        }
        
        [Test]
        public void BannedWordContainedWithinALongerWordIsNotRedacted()
        {    
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "banned word contained within is not changed"};
            var message = cp.ArrayToMessage(line);
            var bt = "with";
            Assert.That(cp.SanitiseMessage(message, "with").content, Is.EqualTo("banned word contained within is not changed"));
        }
        
        [Test]
        public void CasingOfWordsNotImportantForBlacklistedTerms()
        {    
        ConversationExporter cp = new ConversationExporter();
        string[] line = {"1234", "david", "blacklisted word contained within is not changed"};
        var message = cp.ArrayToMessage(line);
        var bl = "with";
        Assert.That(cp.SanitiseMessage(message, "Word").content, Is.EqualTo("blacklisted *redacted* contained within is not changed"));
        }
    }

}
