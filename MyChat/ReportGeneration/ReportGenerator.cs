namespace MindLink.Recruitment.MyChat
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Responsible for producing user activity reports for <see cref="Conversation"/> objects.
    /// </summary>
    public class ReportGenerator : IReportGenerator
    {
        private IDictionary<string, int> userActivity;
        private IList<UserActivityRanking> userActivityRanking;
        private Report report;

        /// <summary>
        /// Initialises a new instance of the <see cref="Report"/> class.
        /// </summary>
        public ReportGenerator()
        {
            userActivity = new Dictionary<string, int>();
            userActivityRanking = new List<UserActivityRanking>();
            report = new Report();
        }

        /// <summary>
        /// Generates a report for the passsed <see cref="Conversation"/> object.
        /// </summary>
        /// <param name="conversation">
        /// The conversation object that the report will be written to.
        /// </param>
        /// <returns></returns>
        public IDictionary<string, int> Generate(Conversation conversation)
        {
            IList<string> topUsers = new List<string>();
            string tiedTopUser = "Tied: ";

            foreach (Message message in conversation.Messages)
            {
                if (userActivity.ContainsKey(message.SenderId))
                    userActivity[message.SenderId] += 1;
                else userActivity.Add(message.SenderId, 1);
            }

            userActivity = userActivity.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

            foreach (string user in userActivity.Keys)
            {
                UserActivityRanking userRank = new UserActivityRanking();
                userRank.User = user;
                userRank.MessageCount = userActivity[user];
                userActivityRanking.Add(userRank);
            }

            report.UserActivityRanking = userActivityRanking.ToArray();
            report.MostActiveUser = userActivity.First().Key;        

            for (int i = 0; i < report.UserActivityRanking.Count(); i++)
            {
                if (report.UserActivityRanking[i].MessageCount == report.UserActivityRanking[i+1].MessageCount)
                {
                    topUsers.Add(report.UserActivityRanking[i].User);
                    topUsers.Add(report.UserActivityRanking[i + 1].User);
                }
                else
                {
                    break;
                }
            }

            if (topUsers.Count == 0)
            {
                report.MostActiveUser = userActivity.First().Key;
            }
            else
            {
                foreach (string user in topUsers)
                {
                    if (topUsers.Last() == user)
                        tiedTopUser += user;
                    else tiedTopUser += user + ", ";
                }
            }
            report.MostActiveUser = tiedTopUser;

            conversation.Report = report;

            return userActivity;
        }
    }
}
