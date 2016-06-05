using System;

namespace MindLink.Recruitment.MyChat
{
    public class Message
    {
        public string Content { get; }

        public DateTimeOffset Timestamp { get; }

        /// <summary>
        /// Sender name or randomly generated ID if obfuscation is on.
        /// </summary>
        public string Sender { get; }

        public Message(DateTimeOffset timestamp, string sender, string content)
        {
            Content = content;
            Timestamp = timestamp;
            Sender = sender;
        }
    }
}
