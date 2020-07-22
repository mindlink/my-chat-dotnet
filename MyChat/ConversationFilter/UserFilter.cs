namespace MindLink.Recruitment.MyChat
{
    /// <summary>
    /// Filters <see cref="Message"/> objects by user id.
    /// </summary>
    public sealed class UserFilter : IMessageFilter
    {
        /// <summary>
        /// Stores the user to be filtered for
        /// </summary>
        public string User { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="UserFilter"/> class.
        /// </summary>
        public UserFilter(string user)
        {
            this.User = user;
        }

        /// <summary>
        /// Returns message if it belongs to filtered user.
        /// </summary>
        /// <param name="message">
        /// Message object to be filtered.
        /// </param>
        public Message FilterMessage(Message message)
        {
            if (message.SenderId.Equals(User))
                return message;
            else return null;
        }
    }
}