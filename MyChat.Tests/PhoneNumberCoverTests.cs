using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace MindLink.Recruitment.MyChat.Tests
{
    [TestFixture]
    public class PhoneNumberCoverTests
    {
        [Test]
        public void Continuous9DigitNumberIsHidden()
        {
            Conversation conversation = new Conversation("Test Conversation", new List<Message>() { new Message(DateTimeOffset.Now, "Bob", "My phone number is 123456789") });
            PhoneNumberCover cover = new PhoneNumberCover(conversation);
            cover.Hide();

            Assert.That(cover.Conversation.Messages[0].Content, Is.EqualTo("My phone number is *redacted*"));

        }

        [Test]
        public void A10DigitLongNumberWithSeparatorsIsHidden()
        {
            Conversation conversation = new Conversation("Test Conversation", new List<Message>() { new Message(DateTimeOffset.Now, "Bob", "My phone number is 1 2345-67890") });
            PhoneNumberCover cover = new PhoneNumberCover(conversation);
            cover.Hide();

            Assert.That(cover.Conversation.Messages[0].Content, Is.EqualTo("My phone number is *redacted*"));

        }


    }
}
