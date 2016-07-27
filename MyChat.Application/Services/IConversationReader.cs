namespace MindLink.Recruitment.MyChat.Application.Services
{
    using Domain.Conversations;
    using Domain.Specifications;
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    /// A <see cref="IConversationReader"/> reads a serialized <see cref="Conversation"/> from
    /// an input stream.
    /// </summary>
    public interface IConversationReader
    {
        /// <summary>
        /// Reads a serialized <see cref="Conversation"/> from an input stream with a specific encoding.
        /// Only messages which satisfy a specification are included in the final result.
        /// </summary>
        /// <param name="input">The input steram.</param>
        /// <param name="encoding">The encoding of the input stream.</param>
        /// <param name="messageSpecification">The specification that must be satisfied by the resulting messages.</param>
        /// <returns>The conversation.</returns>
        /// <exception cref="ArgumentException"
        /// <exception cref="ArgumentNullException"
        /// <exception cref="ConversationReaderErrorException"
        Conversation Read(Stream input, Encoding encoding, ISpecification<Message> messageSpecification);
    }

    /// <summary>
    /// Exception raised by a conversation reader.
    /// </summary>
    public sealed class ConversationReaderErrorException : Exception
    {
        public ConversationReaderErrorException(string message)
            :base(message)
        { }
    }
}
