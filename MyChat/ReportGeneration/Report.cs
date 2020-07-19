namespace MindLink.Recruitment.MyChat
{
    /// <summary>
    /// Stores report data.
    /// </summary>
    public sealed class Report
    {
        /// <summary>
        /// The most active user in a conversation.
        /// </summary>
        public string MostActiveUser { get; set; }

        /// <summary>
        /// User activity ranking ordered decending.
        /// </summary>
        public UserActivityRanking[] UserActivityRanking { get; set; }
    }
}