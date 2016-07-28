namespace MindLink.Recruitment.MyChat.Domain.Specifications
{
    using System;

    /// <summary>
    /// An <see cref="AndSpecification{T}"/> is a composite specification of two other specifications
    /// which is satisfied when any of them is satisfied.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OrSpecification<T> : ISpecification<T>
    {
        private readonly ISpecification<T> _left;
        private readonly ISpecification<T> _right;

        /// <summary>
        /// Initializes an <see cref="OrSpecification{T}"/>
        /// </summary>
        /// <param name="left">The first specification.</param>
        /// <param name="right">The second specification.</param>
        public OrSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            if (left == null)
                throw new ArgumentNullException($"The value of '{nameof(left)}' cannot be null.");

            if (right == null)
                throw new ArgumentNullException($"The value of '{nameof(right)}' cannot be null.");

            _left = left;
            _right = right;
        }

        /// <summary>
        /// Checks whether the specification is satisfied by a candidate.
        /// </summary>
        /// <param name="candidate">The candidate.</param>
        /// <returns>True if the specification is satisfied by the candidate.</returns>
        public bool IsSatisfiedBy(T candidate)
        {
            return _left.IsSatisfiedBy(candidate) || _right.IsSatisfiedBy(candidate);
        }
    }
}
