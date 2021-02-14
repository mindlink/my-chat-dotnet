using System;
namespace MindLink.Recruitment.MyChat
{
    public sealed class Activity
    {
        /// <summary>
        /// The message sender.
        /// </summary>
        public string sender;

        /// <summary>
        /// The number of messages the user has sent.
        /// </summary>
        public int count;

        public Activity(string sender, int count)
        {
            this.sender = sender;
            this.count = count;
        }
    }
}
