namespace MindLink.Recruitment.MyChat.Domain.Specifications
{
    /// <summary>
    /// Interface of a specification for values of a specified type.
    /// </summary>
    /// <typeparam name="T">The type of the values.</typeparam>
    public interface ISpecification<T>
    {
        /// <summary>
        /// Returns true if the specification is satisfied by the candidate.
        /// </summary>
        /// <param name="candidate"></param>
        /// <returns></returns>
        bool IsSatisfiedBy(T candidate);
    }
}
