namespace MindLink.Recruitment.MyChat.Domain.Censorship
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a censorship policy which can censor a value by threading
    /// it through a series of other censorship policies.
    /// </summary>
    public sealed class PipelineCensorshipPolicy : ICensorshipPolicy
    {
        private readonly List<ICensorshipPolicy> _policies;

        public PipelineCensorshipPolicy()
        {
            _policies = new List<ICensorshipPolicy>();
        }

        /// <summary>
        /// The collection of policies registered to the pipeline.
        /// </summary>
        public IEnumerable<ICensorshipPolicy> Policies
        {
            get
            {
                return _policies.AsReadOnly();
            }
        }

        /// <summary>
        /// Adds a policy at the end of the pipeline.
        /// </summary>
        /// <param name="policy">The policy to add.</param>
        /// <returns>The <see cref="PipelineCensorshipPolicy"/> in order to support method call chaining.</returns>
        public PipelineCensorshipPolicy AddPolicy(ICensorshipPolicy policy)
        {
            if (policy == null)
                throw new ArgumentNullException($"The value of '{nameof(policy)}' cannot be null.");

            _policies.Add(policy);

            return this;
        }

        /// <summary>
        /// Censors a value by threading it to the policies registered in the pipeline.
        /// </summary>
        /// <param name="value">The value to censor.</param>
        /// <returns>The censored value.</returns>
        public string Censor(string value)
        {
            return Policies.Aggregate(value, (currValue, nextPolicy) => nextPolicy.Censor(currValue));
        }
    }
}
