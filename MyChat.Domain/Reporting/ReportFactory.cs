namespace MindLink.Recruitment.MyChat.Domain.Reporting
{
    using Conversations;
    using System;

    /// <summary>
    /// Provides factory methods for reports.
    /// </summary>
    public sealed class ReportFactory
    {
        /// <summary>
        /// Creates a report which calculates the most active users in a conversation.
        /// </summary>
        /// <param name="conversation">The conversation to generate the report for.</param>
        /// <returns>The report which displays the most active users in the conversation.</returns>
        public MostActiveUsersReport CreateMostActiveUsersReport(Conversation conversation)
        {
            if (conversation == null)
                throw new ArgumentNullException($"The value of '{nameof(conversation)}' cannot be null.");

            var report = new MostActiveUsersReport();
            foreach (var message in conversation.Messages)
                report.AddUserActivity(message.SenderId);

            return report;
        }
    }
}
