using System;

namespace MyChat.Models
{ 

    /// <summary>
    /// The Message model.
    /// </summary>
    public sealed class Message
    {
        /// <summary>
        /// Content property of Message model.
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// Timestamp property of Message model.
        /// </summary>
        public DateTimeOffset timestamp { get; set; }

        /// <summary>
        /// Sender property of Message model.
        /// </summary>
        public User sender { get; set; }

    }
}
