namespace MindLink.Recruitment.MyChat.Domain.Test.Conversations
{
    using Censorship;
    using Domain.Conversations;
    using Domain.Obfuscation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Linq;

    [TestClass]
    public class MessageTest
    {
        [TestMethod]
        public void It_should_obfuscate_senderId_based_on_a_policy()
        {
            var message = new Message(new DateTime(2000, 1, 1), "senderId", "content");
            Assert.AreEqual("senderId", message.SenderId);

            var obfuscationPolicy = new Mock<IObfuscationPolicy>();
            obfuscationPolicy.Setup(p => p.Obfuscate(It.IsAny<string>())).Returns<string>(s => string.Join("", s.Reverse()));

            message.ObfuscateSender(obfuscationPolicy.Object);
            Assert.AreEqual("dIrednes", message.SenderId);
        }

        [TestMethod]
        public void It_should_sensor_content_based_on_a_policy()
        {
            var message = new Message(new DateTime(2000, 1, 1), "senderId", "sensitive content");
            Assert.AreEqual("sensitive content", message.Content);

            var censorshipPolicy = new Mock<ICensorshipPolicy>();
            censorshipPolicy.Setup(p => p.Censor(It.IsAny<string>())).Returns<string>(s => s.Replace("sensitive", "****"));

            message.CensorContent(censorshipPolicy.Object);
            Assert.AreEqual("**** content", message.Content);
        }
    }
}
