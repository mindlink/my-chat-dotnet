namespace MindLink.Recruitment.MyChat
{
    /// <summary>
    /// Represents the User Activity of a Conversation.
    /// </summary>    
    public sealed class UserActivity
    {
        /// <summary>
        /// The username of user in conversation.
        /// </summary>
        private string userName;

        /// <summary>
        /// The number of messages in conversation.
        /// </summary>
        private int messagesNo;

        public string UserName { get => userName; set => userName = value; }
        public int MessagesNo { get => messagesNo; set => messagesNo = value; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserActivity"/> class.
        /// </summary>
        /// <param name="userName">
        /// The user's username.
        /// </param>
        /// <param name="messageNumber">
        /// The number of messages for the given user within the conversation.
        /// </param>
        public UserActivity(string userName, int messageNumber)
        {
            this.UserName = userName;
            this.MessagesNo = messageNumber;
        }
    }
}
