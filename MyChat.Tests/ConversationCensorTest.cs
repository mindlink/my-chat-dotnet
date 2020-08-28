namespace MindLink.Recruitment.MyChat.Tests
{
    using NUnit.Framework;
    using MindLink.Recruitment.MyChat;
    using System.IO;
    using System.Linq;
    using Newtonsoft.Json;
    using System;

    /// <summary>
    /// Tests for the <see cref="ConversationCensor"/>.
    /// </summary>
    [TestFixture]
    class ConversationCensorTest
    {
        /// <summary>
        /// Tests the censor methods in conversation strings.
        /// </summary>
        [Test]
        public void ConvsersationStringFilters()
        {
            Assert.That(ConversationCensor.CensorPhoneNumber("+447000023000"), Is.EqualTo("*redacted*"));
            Assert.That(ConversationCensor.CensorPhoneNumber("07005902300"), Is.EqualTo("*redacted*"));
            Assert.That(ConversationCensor.CensorCardNumber("4672 9234 7432 9912"), Is.EqualTo("*redacted*"));
            Assert.That(ConversationCensor.CensorCardNumber("4511-8212-5321-1984"), Is.EqualTo("*redacted*"));
            Assert.That(ConversationCensor.CensorCardNumber("5800323211902332"), Is.EqualTo("*redacted*"));
        }

        [Test]
        public void ConversationBlacklistedWord()
        {
            var exporter = new ConversationExporter();
            var conversationCensor = new ConversationCensor();

            exporter.ExportConversation("chat_censor.txt", "chat_censor.json", "Recipient", "no", "no", "no", conversationCensor.BlacklistWord);

            var serializedConversation = new StreamReader(new FileStream("chat_censor.json", FileMode.Open)).ReadToEnd();

            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.That(savedConversation.name, Is.EqualTo("My Conversation"));

            var messages = savedConversation.messages.ToList();

            Assert.That(messages[0].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470905)));
            Assert.That(messages[0].senderId, Is.EqualTo("jimmy"));
            Assert.That(messages[0].content, Is.EqualTo("Hey Bimmy how's it going?"));

            Assert.That(messages[1].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470907)));
            Assert.That(messages[1].senderId, Is.EqualTo("bimmy"));
            Assert.That(messages[1].content, Is.EqualTo("Not too bad Jimmy, you?"));

            Assert.That(messages[2].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470908)));
            Assert.That(messages[2].senderId, Is.EqualTo("jimmy"));
            Assert.That(messages[2].content, Is.EqualTo("I'm cool thanks, just checking if you still want those Bill Burr tickets?"));

            Assert.That(messages[3].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470910)));
            Assert.That(messages[3].senderId, Is.EqualTo("bimmy"));
            Assert.That(messages[3].content, Is.EqualTo("Sure, I mean is it still on? I thought covid-19 shut everything down?"));

            Assert.That(messages[4].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470911)));
            Assert.That(messages[4].senderId, Is.EqualTo("jimmy"));
            Assert.That(messages[4].content, Is.EqualTo("Don't worry bout that, I've got the hook up... just send me your credit card number and phone number"));

            Assert.That(messages[5].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470913)));
            Assert.That(messages[5].senderId, Is.EqualTo("bimmy"));
            Assert.That(messages[5].content, Is.EqualTo("umm... is that safe?"));

            Assert.That(messages[6].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470913)));
            Assert.That(messages[6].senderId, Is.EqualTo("jimmy"));
            Assert.That(messages[6].content, Is.EqualTo("Don't worry bout it!"));

            Assert.That(messages[7].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470914)));
            Assert.That(messages[7].senderId, Is.EqualTo("bimmy"));
            Assert.That(messages[7].content, Is.EqualTo("Ok... my credit card number is 1234567890123456"));

            Assert.That(messages[8].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470914)));
            Assert.That(messages[8].senderId, Is.EqualTo("bimmy"));
            Assert.That(messages[8].content, Is.EqualTo("that's 1234 5678 9012 3456 or 1234-5678-9012-3456"));

            Assert.That(messages[9].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470914)));
            Assert.That(messages[9].senderId, Is.EqualTo("bimmy"));
            Assert.That(messages[9].content, Is.EqualTo("and my mobile is 07002002000"));

            Assert.That(messages[10].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470915)));
            Assert.That(messages[10].senderId, Is.EqualTo("jimmy"));
            Assert.That(messages[10].content, Is.EqualTo("+447002002000, cool thanks..."));

            Assert.That(messages[11].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470916)));
            Assert.That(messages[11].senderId, Is.EqualTo("bimmy"));
            Assert.That(messages[11].content, Is.EqualTo("So when can I get my tickets?"));

            Assert.That(messages[12].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470916)));
            Assert.That(messages[12].senderId, Is.EqualTo("chatbot"));
            Assert.That(messages[12].content, Is.EqualTo("*redacted* could not be found"));

            Assert.That(messages[13].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470917)));
            Assert.That(messages[13].senderId, Is.EqualTo("bimmy"));
            Assert.That(messages[13].content, Is.EqualTo("Jimmy?"));

            Assert.That(messages[14].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470917)));
            Assert.That(messages[14].senderId, Is.EqualTo("chatbot"));
            Assert.That(messages[14].content, Is.EqualTo("*redacted* could not be found"));

            Assert.That(messages[15].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470917)));
            Assert.That(messages[15].senderId, Is.EqualTo("bimmy"));
            Assert.That(messages[15].content, Is.EqualTo("JIMMY?!!!!!"));

            Assert.That(messages[14].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470917)));
            Assert.That(messages[14].senderId, Is.EqualTo("chatbot"));
            Assert.That(messages[14].content, Is.EqualTo("*redacted* could not be found"));
        }
    }
}
