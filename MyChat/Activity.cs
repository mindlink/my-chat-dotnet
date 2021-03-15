namespace MindLink.Recruitment.MyChat
{
    using System;

    /// <summary>
    /// Represents a chat message.
    /// </summary>
    public sealed class Activity
    {
        /// <summary>
        /// The message sender.
        /// </summary>
        public string sender;

        /// <summary>
        /// The number of messages sent by the sender.
        /// </summary>
        public int count = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="Activity"/> class.
        /// </summary>
        /// <param name="sender">
        /// The ID of the sender.
        /// </param>
        public Activity(string sender, int count)
        {
            this.sender = sender;
            this.count = count;
        }

    }
}
