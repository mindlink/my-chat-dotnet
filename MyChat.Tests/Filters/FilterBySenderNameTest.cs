using System.Linq;
using NUnit.Framework;

namespace MindLink.Recruitment.MyChat.Tests
{
    using System;
    using System.Collections.Generic;
    using MindLink.Recruitment.MyChat;

    /// <summary>
    /// Tests for the <see cref="filter by sender"/>.
    /// </summary>
    [TestFixture]
    public class FilerBySenderNameTests
    {
        [Test]
        public void FilterTestSenderID()
        {
            var messages = new List<Message>();
            var messageOne = new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470901")), "stan", "i tested string");
            messages.Add(messageOne);
            var messageTwo = new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470901")), "bob", "i tested string");
            messages.Add(messageTwo);   

            var conversation = new Conversation("test", messages);
            var senderfilter = new SenderFilter("bob");

            senderfilter.ApplyFilter(conversation);

            var filteredMessages = conversation.messages.ToList();
            Assert.That(filteredMessages[0].senderId, Is.EqualTo("bob"));
        }

    }
}
