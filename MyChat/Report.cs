namespace MindLink.Recruitment.MyChat
{
    /// <summary>
    /// Represents a report of a conversation.
    /// </summary>
    public sealed class Report
    {
        /// <summary>
        /// The message sender.
        /// </summary>
        public string senderId;

        /// <summary>
        ///The number of messages sent by user.
        /// </summary>
        public int messageCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="Report"/> class.
        /// </summary>
        /// <param name="senderId">
        /// The ID of the sender.
        /// </param>
        /// <param name="messageCount">
        /// The message count.
        /// </param>
        public Report(string senderId, int messageCount)
        {
            this.senderId = senderId;
            this.messageCount = messageCount;
        }
    }
}
