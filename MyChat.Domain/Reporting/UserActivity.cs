namespace MindLink.Recruitment.MyChat.Domain.Reporting
{
    /// <summary>
    /// Represents a chat activity of a user.
    /// </summary>
    public class UserActivity
    {
        public UserActivity(string userId, int numberOfMessages)
        {
            UserId = userId;
            NumberOfMessages = numberOfMessages;
        }

        /// <summary>
        /// The user id 
        /// </summary>
        public string UserId { get; private set; }

        /// <summary>
        /// The number of messages sent by the user.
        /// </summary>
        public int NumberOfMessages { get; private set; }
    }
}
