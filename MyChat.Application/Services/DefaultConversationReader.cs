namespace MindLink.Recruitment.MyChat.Application.Services
{
    using Domain.Conversations;
    using Domain.Specifications;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public sealed class DefaultConversationReader : IConversationReader
    {
        public Conversation Read(Stream input, Encoding encoding, ISpecification<Message> messageSpec)
        {
            if (input == null)
                throw new ArgumentNullException($"The value of '{nameof(input)}' cannot be null.");

            if (input.Length == 0)
                throw new ArgumentException($"The '{nameof(input)}' stream must not be empty.");

            if (messageSpec == null)
                throw new ArgumentNullException($"The value of '{nameof(messageSpec)}' cannot be null.");

            try
            {
                StreamReader reader = new StreamReader(input, encoding);
                Conversation conversation = ReadConversation(reader, messageSpec);
                return conversation;
            }
            catch (IOException)
            {
                throw new ConversationReaderErrorException("Something went wrong with the parsing of the input stream.");
            }
        }

        private Conversation ReadConversation(StreamReader reader, ISpecification<Message> messageSpec)
        {
            string name = ReadConversationName(reader);

            IEnumerable<Message> messages = ReadMessages(reader);

            try
            {
                Conversation conversation = new Conversation(name, messages.Where(messageSpec.IsSatisfiedBy));
                return conversation;
            } catch (ArgumentNullException)
            {
                throw new ConversationReaderErrorException("Invalid input.");
            }
        }

        private string ReadConversationName(StreamReader reader)
        {
            string conversationName = reader.ReadLine();
            if (conversationName == null)
                throw new ConversationReaderErrorException("Invalid conversation name.");

            return conversationName;
        }

        private IEnumerable<Message> ReadMessages(StreamReader reader)
        {
            while (!reader.EndOfStream)
                yield return ReadMessage(reader);
        }

        private Message ReadMessage(StreamReader reader)
        {
            try
            {
                string line = reader.ReadLine();
                string[] split = line.Split(new char[] { ' ' }, 3);

                DateTimeOffset timestamp = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(split[0]));
                string senderId = split[1];
                string content = split[2];
                return new Message(timestamp, senderId, content);
            }
            catch (IndexOutOfRangeException)
            {
                throw new ConversationReaderErrorException("Invalid message format.");
            }
            catch (FormatException)
            {
                throw new ConversationReaderErrorException("Invalid message timestamp.");
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new ConversationReaderErrorException("Invalid message timestamp.");
            }
            catch (ArgumentNullException)
            {
                throw new ConversationReaderErrorException("Invalid input.");
            }
        }
    }
}
