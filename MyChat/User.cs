namespace MindLink.Recruitment.MyChat
{
    using System;

    /// <summary>
    /// Represents a conversation user
    /// </summary>
    public sealed class User
    {

        /// <summary>
        /// The message sender.
        /// </summary>
        public string username;

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="username">
        /// The message sender.
        /// </param>
        public User(string username)
        {
            this.username = username;
        }
    }
}
