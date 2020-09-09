namespace MindLink.Recruitment.MyChat
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents the model of a conversation.
    /// </summary>
    public sealed class Conversation
    {
        /// <summary>
        /// The name of the conversation.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The messages in the conversation.
        /// </summary>
        public IEnumerable<Message> Messages { get; set; }

    }
}
