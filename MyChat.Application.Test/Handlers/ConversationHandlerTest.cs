namespace MindLink.Recruitment.MyChat.Application.Test.Handlers
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;
    using System.IO;
    using System.Text;
    using System.Collections.Generic;
    using System;
    using Application.Services;
    using Application.Handlers;
    using Commands;
    using Results;
    using Application.Data;

    [TestClass]
    public class ConversationHandlerTest
    {
        private IConversationReader _conversationReader;
        private ConversationHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _conversationReader = new DefaultConversationReader();
            _handler = new ConversationHandler(_conversationReader);
        }

        [TestMethod]
        public void It_should_report_an_error_when_no_input_is_specified()
        {
            ExportConversationCommand request = new ExportConversationCommand { };
            ExportConversationResult result = _handler.Handle(request);

            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.Errors.Any(s => s.Equals("The Input Stream is required.")));
        }

        [TestMethod]
        public void It_should_report_an_error_when_an_empty_input_is_specified()
        {
            var request = new ExportConversationCommand
            {
                Input = new byte[] { }
            };

            ExportConversationResult result = _handler.Handle(request);

            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.Errors.Any(s => s.Equals("The input must not be empty.")));
        }

        [TestMethod]
        public void It_should_return_the_whole_conversation_when_given_default_options()
        {
            Encoding encoding = Encoding.ASCII;
            byte[] inputBytes = Encoding.ASCII.GetBytes(HandlersTestResources.chat);

            var request = new ExportConversationCommand
            {
                Input = inputBytes
            };

            ExportConversationResult result = _handler.Handle(request);

            var expected = new ConversationDTO
            {
                Name = "My Conversation",
                Messages = new List<MessageDTO>
                {
                    new MessageDTO { SenderId = "bob" , Timestamp = CreateDateTimeOffsetFromString("1448470901"), Content = "Hello there!" },
                    new MessageDTO { SenderId = "mike", Timestamp = CreateDateTimeOffsetFromString("1448470905"), Content = "how are you?" },
                    new MessageDTO { SenderId = "bob" , Timestamp = CreateDateTimeOffsetFromString("1448470906"), Content = "I'm good thanks, here is my credit card: 4111111111111" },
                    new MessageDTO { SenderId = "mike", Timestamp = CreateDateTimeOffsetFromString("1448470910"), Content = "Cool!" },
                    new MessageDTO { SenderId = "mike", Timestamp = CreateDateTimeOffsetFromString("1448470911"), Content = "Give me a call at: +35722123456" },
                }
            };

            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(EqualityComparer<ConversationDTO>.Default.Equals(expected, result.Conversation));
        }

        [TestMethod]
        public void It_should_only_return_messages_of_a_specific_user()
        {
            Encoding encoding = Encoding.ASCII;
            byte[] inputBytes = Encoding.ASCII.GetBytes(HandlersTestResources.chat);

            var request = new ExportConversationCommand
            {
                Input = inputBytes,
                UserIdFilter = "bob"
            };

            ExportConversationResult result = _handler.Handle(request);

            var expected = new ConversationDTO
            {
                Name = "My Conversation",
                Messages = new List<MessageDTO>
                {
                    new MessageDTO { SenderId = "bob" , Timestamp = CreateDateTimeOffsetFromString("1448470901"), Content = "Hello there!" },
                    new MessageDTO { SenderId = "bob" , Timestamp = CreateDateTimeOffsetFromString("1448470906"), Content = "I'm good thanks, here is my credit card: 4111111111111" },
                }
            };

            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(EqualityComparer<ConversationDTO>.Default.Equals(expected, result.Conversation));
        }

        [TestMethod]
        public void It_should_only_return_messages_which_contain_a_specific_keyword()
        {
            Encoding encoding = Encoding.ASCII;
            byte[] inputBytes = Encoding.ASCII.GetBytes(HandlersTestResources.chat);

            var request = new ExportConversationCommand
            {
                Input = inputBytes,
                ContentKeywordFilter = "there"
            };

            ExportConversationResult result = _handler.Handle(request);

            var expected = new ConversationDTO
            {
                Name = "My Conversation",
                Messages = new List<MessageDTO>
                {
                    new MessageDTO { SenderId = "bob" , Timestamp = CreateDateTimeOffsetFromString("1448470901"), Content = "Hello there!" },
                }
            };

            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(EqualityComparer<ConversationDTO>.Default.Equals(expected, result.Conversation));
        }

        [TestMethod]
        public void It_should_censor_messages_based_on_specified_keywords()
        {
            Encoding encoding = Encoding.ASCII;
            byte[] inputBytes = Encoding.ASCII.GetBytes(HandlersTestResources.chat);

            var request = new ExportConversationCommand
            {
                Input = inputBytes,
                KeywordsToCensor = new List<string> { "cool", "call" }
            };

            ExportConversationResult result = _handler.Handle(request);

            var expected = new ConversationDTO
            {
                Name = "My Conversation",
                Messages = new List<MessageDTO>
                {
                    new MessageDTO { SenderId = "bob" , Timestamp = CreateDateTimeOffsetFromString("1448470901"), Content = "Hello there!" },
                    new MessageDTO { SenderId = "mike", Timestamp = CreateDateTimeOffsetFromString("1448470905"), Content = "how are you?" },
                    new MessageDTO { SenderId = "bob" , Timestamp = CreateDateTimeOffsetFromString("1448470906"), Content = "I'm good thanks, here is my credit card: 4111111111111" },
                    new MessageDTO { SenderId = "mike", Timestamp = CreateDateTimeOffsetFromString("1448470910"), Content = "*redacted*!" },
                    new MessageDTO { SenderId = "mike", Timestamp = CreateDateTimeOffsetFromString("1448470911"), Content = "Give me a *redacted* at: +35722123456" },
                }
            };

            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(EqualityComparer<ConversationDTO>.Default.Equals(expected, result.Conversation));
        }

        [TestMethod]
        public void It_should_censor_credit_card_numbers()
        {
            Encoding encoding = Encoding.ASCII;
            byte[] inputBytes = Encoding.ASCII.GetBytes(HandlersTestResources.chat);

            var request = new ExportConversationCommand
            {
                Input = inputBytes,
                CensorCreditCardNumbers = true
            };

            ExportConversationResult result = _handler.Handle(request);

            var expected = new ConversationDTO
            {
                Name = "My Conversation",
                Messages = new List<MessageDTO>
                {
                    new MessageDTO { SenderId = "bob" , Timestamp = CreateDateTimeOffsetFromString("1448470901"), Content = "Hello there!" },
                    new MessageDTO { SenderId = "mike", Timestamp = CreateDateTimeOffsetFromString("1448470905"), Content = "how are you?" },
                    new MessageDTO { SenderId = "bob" , Timestamp = CreateDateTimeOffsetFromString("1448470906"), Content = "I'm good thanks, here is my credit card: *redacted*" },
                    new MessageDTO { SenderId = "mike", Timestamp = CreateDateTimeOffsetFromString("1448470910"), Content = "Cool!" },
                    new MessageDTO { SenderId = "mike", Timestamp = CreateDateTimeOffsetFromString("1448470911"), Content = "Give me a call at: +35722123456" },
                }
            };

            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(EqualityComparer<ConversationDTO>.Default.Equals(expected, result.Conversation));
        }

        [TestMethod]
        public void It_should_censor_telephone_numbers()
        {
            Encoding encoding = Encoding.ASCII;
            byte[] inputBytes = Encoding.ASCII.GetBytes(HandlersTestResources.chat);

            var request = new ExportConversationCommand
            {
                Input = inputBytes,
                CensorTelephoneNumbers = true
            };

            ExportConversationResult result = _handler.Handle(request);

            var expected = new ConversationDTO
            {
                Name = "My Conversation",
                Messages = new List<MessageDTO>
                {
                    new MessageDTO { SenderId = "bob" , Timestamp = CreateDateTimeOffsetFromString("1448470901"), Content = "Hello there!" },
                    new MessageDTO { SenderId = "mike", Timestamp = CreateDateTimeOffsetFromString("1448470905"), Content = "how are you?" },
                    new MessageDTO { SenderId = "bob" , Timestamp = CreateDateTimeOffsetFromString("1448470906"), Content = "I'm good thanks, here is my credit card: 4111111111111" },
                    new MessageDTO { SenderId = "mike", Timestamp = CreateDateTimeOffsetFromString("1448470910"), Content = "Cool!" },
                    new MessageDTO { SenderId = "mike", Timestamp = CreateDateTimeOffsetFromString("1448470911"), Content = "Give me a call at: *redacted*" },
                }
            };

            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(EqualityComparer<ConversationDTO>.Default.Equals(expected, result.Conversation));
        }

        [TestMethod]
        public void It_should_obfuscate_users()
        {
            Encoding encoding = Encoding.ASCII;
            byte[] inputBytes = Encoding.ASCII.GetBytes(HandlersTestResources.chat);
            Stream input = new MemoryStream(inputBytes);

            var request = new ExportConversationCommand
            {
                Input = inputBytes,
                ObfuscateUserId = true
            };

            ExportConversationResult result = _handler.Handle(request);

            var expected = new ConversationDTO
            {
                Name = "My Conversation",
                Messages = new List<MessageDTO>
                {
                    new MessageDTO { SenderId = "i9mY"    , Timestamp = CreateDateTimeOffsetFromString("1448470901"), Content = "Hello there!" },
                    new MessageDTO { SenderId = "==QZrlWb", Timestamp = CreateDateTimeOffsetFromString("1448470905"), Content = "how are you?" },
                    new MessageDTO { SenderId = "i9mY"    , Timestamp = CreateDateTimeOffsetFromString("1448470906"), Content = "I'm good thanks, here is my credit card: 4111111111111" },
                    new MessageDTO { SenderId = "==QZrlWb", Timestamp = CreateDateTimeOffsetFromString("1448470910"), Content = "Cool!" },
                    new MessageDTO { SenderId = "==QZrlWb", Timestamp = CreateDateTimeOffsetFromString("1448470911"), Content = "Give me a call at: +35722123456" },
                }
            };

            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(EqualityComparer<ConversationDTO>.Default.Equals(expected, result.Conversation));
        }

        [TestMethod]
        public void It_should_generate_most_active_users_report()
        {
            Encoding encoding = Encoding.ASCII;
            byte[] inputBytes = Encoding.ASCII.GetBytes(HandlersTestResources.chat);

            var request = new ExportConversationCommand
            {
                Input = inputBytes,
                GenerateMostActiveUsersReport = true
            };

            ExportConversationResult result = _handler.Handle(request);

            var expected = new ConversationDTO
            {
                Name = "My Conversation",
                Messages = new List<MessageDTO>
                {
                    new MessageDTO { SenderId = "bob" , Timestamp = CreateDateTimeOffsetFromString("1448470901"), Content = "Hello there!" },
                    new MessageDTO { SenderId = "mike", Timestamp = CreateDateTimeOffsetFromString("1448470905"), Content = "how are you?" },
                    new MessageDTO { SenderId = "bob" , Timestamp = CreateDateTimeOffsetFromString("1448470906"), Content = "I'm good thanks, here is my credit card: 4111111111111" },
                    new MessageDTO { SenderId = "mike", Timestamp = CreateDateTimeOffsetFromString("1448470910"), Content = "Cool!" },
                    new MessageDTO { SenderId = "mike", Timestamp = CreateDateTimeOffsetFromString("1448470911"), Content = "Give me a call at: +35722123456" },
                },
                MostActiveUsers = new List<UserActivityDTO>
                {
                    new UserActivityDTO { UserId = "mike", NumberOfMessages = 3 },
                    new UserActivityDTO { UserId = "bob", NumberOfMessages = 2 }
                }
            };

            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(EqualityComparer<ConversationDTO>.Default.Equals(expected, result.Conversation));
        }

        [TestMethod]
        public void It_should_generate_most_active_users_based_on_filtered_messages()
        {
            Encoding encoding = Encoding.ASCII;
            byte[] inputBytes = Encoding.ASCII.GetBytes(HandlersTestResources.chat);
            Stream input = new MemoryStream(inputBytes);

            var request = new ExportConversationCommand
            {
                Input = inputBytes,
                UserIdFilter = "bob",
                GenerateMostActiveUsersReport = true
            };

            ExportConversationResult result = _handler.Handle(request);

            var expected = new ConversationDTO
            {
                Name = "My Conversation",
                Messages = new List<MessageDTO>
                {
                    new MessageDTO { SenderId = "bob" , Timestamp = CreateDateTimeOffsetFromString("1448470901"), Content = "Hello there!" },
                    new MessageDTO { SenderId = "bob" , Timestamp = CreateDateTimeOffsetFromString("1448470906"), Content = "I'm good thanks, here is my credit card: 4111111111111" },
                },
                MostActiveUsers = new List<UserActivityDTO>
                {
                    new UserActivityDTO { UserId = "bob", NumberOfMessages = 2 }
                }
            };

            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(EqualityComparer<ConversationDTO>.Default.Equals(expected, result.Conversation));
        }

        [TestMethod]
        public void It_should_obfuscate_the_user_id_in_most_active_users()
        {
            Encoding encoding = Encoding.ASCII;
            byte[] inputBytes = Encoding.ASCII.GetBytes(HandlersTestResources.chat);
            Stream input = new MemoryStream(inputBytes);

            var request = new ExportConversationCommand
            {
                Input = inputBytes,
                ObfuscateUserId = true,
                GenerateMostActiveUsersReport = true
            };

            ExportConversationResult result = _handler.Handle(request);

            var expected = new ConversationDTO
            {
                Name = "My Conversation",
                Messages = new List<MessageDTO>
                {
                    new MessageDTO { SenderId = "i9mY"    , Timestamp = CreateDateTimeOffsetFromString("1448470901"), Content = "Hello there!" },
                    new MessageDTO { SenderId = "==QZrlWb", Timestamp = CreateDateTimeOffsetFromString("1448470905"), Content = "how are you?" },
                    new MessageDTO { SenderId = "i9mY"    , Timestamp = CreateDateTimeOffsetFromString("1448470906"), Content = "I'm good thanks, here is my credit card: 4111111111111" },
                    new MessageDTO { SenderId = "==QZrlWb", Timestamp = CreateDateTimeOffsetFromString("1448470910"), Content = "Cool!" },
                    new MessageDTO { SenderId = "==QZrlWb", Timestamp = CreateDateTimeOffsetFromString("1448470911"), Content = "Give me a call at: +35722123456" },
                },
                MostActiveUsers = new List<UserActivityDTO>
                {
                    new UserActivityDTO { UserId = "==QZrlWb", NumberOfMessages = 3 },
                    new UserActivityDTO { UserId = "i9mY", NumberOfMessages = 2 }
                }
            };

            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(EqualityComparer<ConversationDTO>.Default.Equals(expected, result.Conversation));
        }

        private DateTimeOffset CreateDateTimeOffsetFromString(string unixSeconds)
        {
            var seconds = Convert.ToInt64(unixSeconds);
            return DateTimeOffset.FromUnixTimeSeconds(seconds);
        }
    }
}
