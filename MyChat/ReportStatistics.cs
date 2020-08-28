namespace MindLink.Recruitment.MyChat
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents the model of a report statistics.
    /// </summary>
    public sealed class ReportStatistics
    {
        /// <summary>
        /// The report statistics.
        /// </summary>
        public string report;

        /// <summary>
        /// The messages in the conversation.
        /// </summary>
        public IEnumerable<Report> statistics;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportStatistics"/> class.
        /// </summary>
        /// <param name="statistics">
        /// The statistics of the conversation.
        /// </param>
        public ReportStatistics(IEnumerable<Report> statistics)
        {
            report = "This is a list of the most active users in the conversation in order of most messages sent";
            this.statistics = statistics;
        }
    }
}
