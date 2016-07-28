using MindLink.Recruitment.MyChat.Domain.Conversations;

namespace MindLink.Recruitment.MyChat.Domain.Specifications
{
    /// <summary>
    /// A specification which is satisfied by every <see cref="Message"/>
    /// </summary>
    /// <typeparam name="T">The type of each candidate value.</typeparam>
    public sealed class EveryMessageSpecification : ISpecification<Message>
    {
        public bool IsSatisfiedBy(Message candidate)
        {
            return candidate != null;
        }
    }
}
