using System.Collections.Generic;
using System.Linq;

namespace MindLink.MyChat
{
    public sealed class ConversationStatsItem
    {
        public string SenderId { get; set; }
        public int MessagesSent { get; set; }
    }

    /// <summary>
    ///     Represents the model of a conversation.
    /// </summary>
    public sealed class Conversation
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Conversation" /> class.
        /// </summary>
        /// <param name="name">
        ///     The name of the conversation.
        /// </param>
        /// <param name="messages">
        ///     The messages in the conversation.
        /// </param>
        public Conversation(string name, IEnumerable<Message> messages)
        {
            this.Name = name;
            this.Messages = messages;
        }

        /// <summary>
        ///     The name of the conversation.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     The messages in the conversation.
        /// </summary>
        public IEnumerable<Message> Messages { get; }

        /// <summary>
        ///     Conversation stats.
        /// </summary>
        public IEnumerable<ConversationStatsItem> Stats
        {
            get
            {
                return this.Messages.GroupBy(m => m.SenderId)
                    .Select(g => new ConversationStatsItem {SenderId = g.Key, MessagesSent = g.Count()})
                    .OrderByDescending(i => i.MessagesSent);
            }
            // ReSharper disable once ValueParameterNotUsed
            // Ignore on deserialization
            set { }
        }
    }
}