using MindLink.Recruitment.MyChat.Domain.Conversations;

namespace MindLink.Recruitment.MyChat.Domain.Specifications
{
    /// <summary>
    /// Specification which is satisfied by messages which belong to a particular sender.
    /// </summary>
    public sealed class MessageOfUserSpecification : ISpecification<Message>
    {
        private string _senderId;

        /// <summary>
        /// Initializes a <see cref="MessageOfUserSpecification"/>
        /// </summary>
        /// <param name="senderId">The senderId to look for in a message.</param>
        public MessageOfUserSpecification(string senderId)
        {
            _senderId = senderId;
        }

        /// <summary>
        /// Tests whether a message satisfies the specification.
        /// </summary>
        /// <param name="message">The candidate message to test.</param>
        /// <returns></returns>
        public bool IsSatisfiedBy(Message message)
        {
            return message.SenderId.Equals(_senderId);
        }
    }
}
