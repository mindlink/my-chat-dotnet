namespace MindLink.Recruitment.MyChat.Domain.Specifications
{
    using Conversations;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Specification which is satisfied by messages which contain a keyword.
    /// </summary>
    public sealed class MessageContainingKeywordSpecification : ISpecification<Message>
    {
        private string _keyword;

        /// <summary>
        /// Initializes a <see cref="MessageContainingKeywordSpecification"/>
        /// </summary>
        /// <param name="keyword">The keyword to look for in the content of a message.</param>
        public MessageContainingKeywordSpecification(string keyword)
        {
            _keyword = keyword;
        }

        /// <summary>
        /// Tests whether a message satisfies the specification.
        /// </summary>
        /// <param name="message">The candidate message to test.</param>
        /// <returns></returns>
        public bool IsSatisfiedBy(Message message)
        {
            return (
                new Regex(Regex.Escape(_keyword), RegexOptions.IgnoreCase)
                    .IsMatch(message.Content)
            );
        }
    }
}
