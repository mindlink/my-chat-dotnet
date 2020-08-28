namespace MindLink.Recruitment.MyChat
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Create a report from messages in conversation.
    /// </summary>
    public sealed class ConversationReport
    {
        /// <summary>
        /// Method to create report from <paramref name="messages"/>.
        /// </summary>
        /// <param name="messages">
        /// The input file path.
        /// </param>
        /// <returns>
        /// A <see cref="Report"/> model representing the report.
        /// </returns>
        public List<Report> CreateReport(IEnumerable<Message> messages)
        {
            var report = new List<Report>();

            var mes = messages
                .GroupBy(u => u.senderId)
                .Select(g => new {
                    SenderID = g.Key,
                    Count = g.Select(u => u.content).Count()
                });

            foreach (var m in mes.OrderByDescending(i => i.Count)) report.Add(new Report(m.SenderID, m.Count));

            return report;
        }
    }
}
