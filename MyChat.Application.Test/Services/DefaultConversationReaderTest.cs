namespace MindLink.Recruitment.MyChat.Application.Test.Services
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.IO;
    using System.Text;
    using System.Linq;
    using System;
    using Application.Services;
    using Moq;
    using Domain.Specifications;
    using Domain.Conversations;

    [TestClass]
    public class DefaultConversationReaderTest
    {
        Mock<ISpecification<Message>> _alwaysSatisfiedSpec;

        [TestInitialize]
        public void Setup()
        {
            _alwaysSatisfiedSpec = new Mock<ISpecification<Message>>();
            _alwaysSatisfiedSpec.Setup(s => s.IsSatisfiedBy(It.IsAny<Message>()))
                .Returns(true);
        }

        [TestMethod]
        [ExpectedException(typeof(ConversationReaderErrorException), "Invalid message format.")]
        public void It_should_be_able_to_report_errors_in_message_format()
        {
            Encoding encoding = Encoding.ASCII;
            byte[] inputBytes = Encoding.ASCII.GetBytes(ServicesTestResources.invalid_message_format);
            Stream input = new MemoryStream(inputBytes);
            DefaultConversationReader parser = new DefaultConversationReader();
            parser.Read(input, encoding, _alwaysSatisfiedSpec.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ConversationReaderErrorException), "Invalid message timestamp.")]
        public void It_should_be_able_to_report_errors_in_message_timestamp()
        {
            Encoding encoding = Encoding.ASCII;
            byte[] inputBytes = Encoding.ASCII.GetBytes(ServicesTestResources.invalid_message_timestamp);
            Stream input = new MemoryStream(inputBytes);
            DefaultConversationReader parser = new DefaultConversationReader();
            parser.Read(input, encoding, _alwaysSatisfiedSpec.Object);
        }

        [TestMethod]
        public void It_should_be_able_to_parse_valid_input()
        {
            Encoding encoding = Encoding.ASCII;
            byte[] inputBytes = Encoding.ASCII.GetBytes(ServicesTestResources.valid_chat);
            Stream input = new MemoryStream(inputBytes);

            DefaultConversationReader parser = new DefaultConversationReader();
            Conversation conversation = parser.Read(input, encoding, _alwaysSatisfiedSpec.Object);

            Assert.IsNotNull(conversation);
            Assert.AreEqual("name", conversation.Name);
            Assert.AreEqual(1, conversation.Messages.Count());

            var message = conversation.Messages.First();
            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470901")), message.Timestamp);
            Assert.AreEqual("bob", message.SenderId);
            Assert.AreEqual("Hello!", message.Content);
        }
    }
}
