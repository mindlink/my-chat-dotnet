namespace MindLink.Recruitment.MyChat.Domain.Test.Conversations
{
    using Censorship;
    using Common.Extensions;
    using Domain.Conversations;
    using Domain.Obfuscation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Linq;

    [TestClass]
    public class ConversationTest
    {
        [TestMethod]
        public void It_should_censor_the_content_of_every_message_based_on_a_policy()
        {
            var censorshipPolicy = new Mock<ICensorshipPolicy>();
            censorshipPolicy.Setup(p => p.Censor(It.IsAny<string>())).Returns<string>(s => "*");

            var messages = new Message[]
            {
                new Message(new DateTimeOffset(new DateTime(2000, 1, 1), TimeSpan.Zero), "senderId", "content1"),
                new Message(new DateTimeOffset(new DateTime(2000, 1, 1), TimeSpan.Zero), "senderId", "content2"),
            };

            var conversation = new Conversation("Conversation name", messages);
            conversation.CensorMessages(censorshipPolicy.Object);

            foreach (var message in conversation.Messages)
                Assert.AreEqual("*", message.Content);
        }

        [TestMethod]
        public void It_should_obfuscate_the_sender_id_of_every_message_based_on_a_policy()
        {
            var obfuscationPolicy = new Mock<IObfuscationPolicy>();
            obfuscationPolicy.Setup(p => p.Obfuscate(It.IsAny<string>())).Returns<string>(s => s.Reverse());

            var messages = new Message[]
            {
                new Message(new DateTimeOffset(new DateTime(2000, 1, 1), TimeSpan.Zero), "senderId", "content1"),
                new Message(new DateTimeOffset(new DateTime(2000, 1, 1), TimeSpan.Zero), "senderId", "content2"),
            };

            var conversation = new Conversation("Conversation name", messages);
            conversation.ObfuscateSenders(obfuscationPolicy.Object);

            foreach (var message in conversation.Messages)
                Assert.AreEqual(message.Content.Reverse().Reverse(), message.Content);
        }
    }
}
