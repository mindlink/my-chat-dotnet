namespace MindLink.Recruitment.MyChat
{
    using System;

    /// <summary>
    /// Represents a chat message.
    /// </summary>
    public sealed class Report
    {

        /// <summary>
        /// The message sender.
        /// </summary>
        public string sender;

        /// <summary>
        /// Number of message the user sent
        /// </summary>
        public int count;

        /// <summary>
        /// Initialize new <see cref="Report"/> class
        /// </summary>
        /// <param name="senderId">
        /// Id of sender
        /// </param>
        /// <param name="messageCount">
        /// Number of message sent by user
        /// </param>
        public Report(string senderId, int messageCount)
        {
            this.sender = senderId;
            this.count = messageCount;
        }
    }
}