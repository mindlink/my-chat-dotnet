namespace MindLink.Recruitment.MyChat.Domain.Conversations
{
    using Censorship;
    using Obfuscation;
    using System;

    public sealed class Message
    {
        /// <summary>
        /// The message sender.
        /// </summary>
        public string SenderId { get; private set; }

        /// <summary>
        /// The message timestamp.
        /// </summary>
        public DateTimeOffset Timestamp { get; private set; }

        /// <summary>
        /// The message content.
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="timestamp">
        /// The message timestamp.
        /// </param>
        /// <param name="senderId">
        /// The ID of the sender.
        /// </param>
        /// <param name="content">
        /// The message content.
        /// </param>
        public Message(DateTimeOffset timestamp, string senderId, string content)
        {
            SenderId = senderId;
            Timestamp = timestamp;
            Content = content;
        }

        /// <summary>
        /// Returns the obfuscated value of the <see cref="SenderId" based on an obfuscation policy. />
        /// </summary>
        /// <param name="obfuscationPolicy">The <see cref="IObfuscationPolicy"/> to use.</param>
        internal void ObfuscateSender(IObfuscationPolicy obfuscationPolicy)
        {
            if (obfuscationPolicy == null)
                throw new ArgumentNullException($"The value of '{obfuscationPolicy}' cannot be null.");

            SenderId = obfuscationPolicy.Obfuscate(SenderId);
        }

        /// <summary>
        /// Returns the censored value of the <see cref="Content" by applying a collection of censorship policies on the value. />
        /// </summary>
        /// <param name="censorshipPolicies">The collection of <see cref="ICensorshipPolicy"/> to apply on the content.</param>
        internal void CensorContent(ICensorshipPolicy policy)
        {
            if (policy == null)
                throw new ArgumentNullException($"The value of '{policy}' cannot be null.");

            Content = policy.Censor(Content);
        }
    }
}
