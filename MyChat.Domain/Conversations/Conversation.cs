namespace MindLink.Recruitment.MyChat.Domain.Conversations
{
    using Censorship;
    using Obfuscation;
    using System.Collections.Generic;

    public sealed class Conversation
    {
        private readonly List<Message> _messages;

        /// <summary>
        /// The name of the conversation
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The messages in the conversation.
        /// </summary>
        public IEnumerable<Message> Messages
        {
            get { return _messages; }
        }

        /// <summary>
        /// Initializes a conversation with the specified name.
        /// </summary>
        /// <param name="name">The name of the conversation</param>
        public Conversation(string name)
            : this(name, null)
        { }

        /// <summary>
        /// Initializes a conversation with the specified name and the list of messages.
        /// </summary>
        /// <param name="name">The name of the conversation</param>
        /// <param name="messages">The messages belonging to the conversation</param>
        public Conversation(string name, IEnumerable<Message> messages)
        {
            Name = name;

            _messages = new List<Message>();
            if (messages != null)
                _messages.AddRange(messages);
        }

        /// <summary>
        /// Censors the content of each message in the conversation based on a censorship policy.
        /// </summary>
        /// <param name="censorshipPolicy">The censorship policy to apply to each message.</param>
        public void CensorMessages(ICensorshipPolicy censorshipPolicy)
        {
            foreach (var message in Messages)
                message.CensorContent(censorshipPolicy);
        }

        /// <summary>
        /// Obfuscates the sender of each message in the conversation based on an obfuscation policy.
        /// </summary>
        /// <param name="obfuscationPolicy">The obfuscation policy to apply to each message.</param>
        public void ObfuscateSenders(IObfuscationPolicy obfuscationPolicy)
        {
            foreach (var message in Messages)
                message.ObfuscateSender(obfuscationPolicy);
        }
    }
}
