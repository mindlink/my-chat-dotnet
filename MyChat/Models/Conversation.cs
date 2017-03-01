using System;
using System.Collections.Generic;

namespace MyChat.Models
{

    /// <summary>
    /// The Conversation model.
    /// </summary>
    public sealed class Conversation
    {
        /// <summary>
        /// Name property of Conversation model.
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Messages property of Conversation model.
        /// </summary>
        public IEnumerable<Message> messages { get; set; }

    }
}
