namespace MindLink.Recruitment.MyChat
{
    using System.Collections.Generic;

    /// <summary>
    /// Stores conversation data.
    /// </summary>
    public class Conversation
    {
        /// <summary>
        /// The name of the conversation.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The messages in the conversation.
        /// </summary>
        public IEnumerable<Message> Messages { get; set; }

        /// <summary>
        /// The conversation user activity report.
        /// </summary>
        public Report Report { get; set; }
    }
}