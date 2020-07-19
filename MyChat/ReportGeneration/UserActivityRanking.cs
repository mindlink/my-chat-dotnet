namespace MindLink.Recruitment.MyChat
{
    /// <summary>
    /// Stores user activity data.
    /// </summary>
    public sealed class UserActivityRanking
    {
        /// <summary>
        /// User username.
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// User conversation message count.
        /// </summary>
        public int MessageCount { get; set; }
    }
}