using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace MindLink.Recruitment.MyChat.Tests
{
    [TestFixture]
    public class CreditCardCoverTests
    {
        [Test]
        public void ContinuousCreditCardIsHidden()
        {
            Conversation conversation = new Conversation("Test Conversation", new List<Message>() { new Message(DateTimeOffset.Now, "Bob", "My card is 1234567891123456") });
            CreditCardCover cover = new CreditCardCover(conversation);

            cover.Hide();

            Assert.That(cover.Conversation.Messages[0].Content, Is.EqualTo("My card is *redacted*"));
        }

        [Test]
        public void SpacedCreditCardIsHidden()
        {
            Conversation conversation = new Conversation("Test Conversation", new List<Message>() { new Message(DateTimeOffset.Now, "Bob", "My card is 1234 5678 9112 3456") });
            CreditCardCover cover = new CreditCardCover(conversation);

            cover.Hide();

            Assert.That(cover.Conversation.Messages[0].Content, Is.EqualTo("My card is *redacted*"));
        }
    }
}
