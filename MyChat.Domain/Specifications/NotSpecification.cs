namespace MindLink.Recruitment.MyChat.Domain.Specifications
{
    using System;

    /// <summary>
    /// A <see cref="NotSpecification{T}"/> specification reverses the result of an another specification.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NotSpecification<T> : ISpecification<T>
    {
        private readonly ISpecification<T> _other;

        /// <summary>
        /// Initializes a <see cref="NotSpecification{T}"/>
        /// </summary>
        /// <param name="other">The other specification to consolidate.</param>
        public NotSpecification(ISpecification<T> other)
        {
            if (other == null)
                throw new ArgumentNullException($"The value of '{nameof(other)}' cannot be null.");

            _other = other;
        }

        /// <summary>
        /// Checks whether the specification is satisfied by a candidate.
        /// </summary>
        /// <param name="candidate">The candidate.</param>
        /// <returns>True if the specification is satisfied by the candidate.</returns>
        public bool IsSatisfiedBy(T candidate)
        {
            return !_other.IsSatisfiedBy(candidate);
        }
    }
}
