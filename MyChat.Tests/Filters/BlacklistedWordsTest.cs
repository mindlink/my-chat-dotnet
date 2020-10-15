using System.Linq;
using NUnit.Framework;

namespace MindLink.Recruitment.MyChat.Tests
{
    using System;
    using System.Collections.Generic;
    using MindLink.Recruitment.MyChat;

    [TestFixture]
    public class BlackListedWordsTests
    {
        [Test]
        public void RedactWordTest()
        {
            var messages = new List<Message>();
            var messageOne = new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470901")), "stan", "i tested stringone");
            messages.Add(messageOne);
            var messageTwo = new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470901")), "bob", "i tested stringtwo");
            messages.Add(messageTwo);

            var conversation = new Conversation("test", messages);
            string[] args = { "tested", "stringtwo" };
            var BlacklistFilter = new BlacklistFilter(args);

            BlacklistFilter.ApplyFilter(conversation);

            var filteredMessages = conversation.messages.ToList();
            Assert.That(filteredMessages[0].content, Is.EqualTo("i *redacted* stringone"));
            Assert.That(filteredMessages[1].content, Is.EqualTo("i *redacted* *redacted*"));
        }

        [Test]
        public void OnlyRedactCompleteWordsRedactBlacklistedWords()
        {   
            string[] args = { "i" };
            var blacklistFilter = new BlacklistFilter(args);
            var message = new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470901")), "name", "i tested string");
            
            blacklistFilter.ApplyRegexRedaction(message);
            Assert.That(message.content, Is.EqualTo("*redacted* tested string"));
        }

        [Test]
        public void RedactCompleteWordsNextToPunctuationRedactBlacklistedWords()
        {
            string[] args = { "string" };
            var blacklistFilter = new BlacklistFilter(args);
            var message = new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470901")), "name", "tested string!");
            
            blacklistFilter.ApplyRegexRedaction(message);
            Assert.That(message.content, Is.EqualTo("tested *redacted*!"));
        }
    }
}
