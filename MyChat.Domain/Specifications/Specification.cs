namespace MindLink.Recruitment.MyChat.Domain.Specifications
{
    /// <summary>
    /// Provides extension methods for specifications.
    /// </summary>
    public static class Specification
    {
        /// <summary>
        /// Factory method of a composite specification which is satisfied only when
        /// both left and right specifications are satisfied.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public static ISpecification<T> And<T>(this ISpecification<T> left, ISpecification<T> right)
        {
            return new AndSpecification<T>(left, right);
        }

        /// <summary>
        /// Factory method of a composite specification which is satisfied only when
        /// any of left and right specifications are satisfied.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public static ISpecification<T> Or<T>(this ISpecification<T> left, ISpecification<T> right)
        {
            return new OrSpecification<T>(left, right);
        }

        /// <summary>
        /// Factory method of a specification which is reverses the result of an another specification.
        /// </summary>
        /// <returns></returns>
        public static ISpecification<T> Not<T>(this ISpecification<T> other)
        {
            return new NotSpecification<T>(other);
        }
    }
}
