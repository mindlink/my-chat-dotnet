namespace MindLink.Recruitment.MyChat.ReportGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MindLink.Recruitment.MyChat.ConversationData;

    /// <summary>
    /// Responsible for producing user activity reports for <see cref="Conversation"/> objects.
    /// </summary>
    public sealed class ReportGenerator : IReportGenerator
    {
        private IDictionary<string, int> userActivity;
        private IList<UserActivityRanking> userActivityRanking;
        private Report report;
        private IList<string> topUsers;
        private string tiedTopUser;

        /// <summary>
        /// Initialises a new instance of the <see cref="Report"/> class.
        /// </summary>
        public ReportGenerator()
        {
            userActivity = new Dictionary<string, int>();
            userActivityRanking = new List<UserActivityRanking>();
            report = new Report();
            topUsers = new List<string>();
            tiedTopUser = "Tied: ";
        }

        /// <summary>
        /// Generates a report for the passsed <see cref="Conversation"/> object.
        /// </summary>
        /// <param name="conversation">
        /// The conversation object that the report will be written to.
        /// </param>
        /// <exception cref="NullReferenceException">
        /// Thrown when the conversation argument hass Messages are null.
        /// </exception>
        public IDictionary<string, int> Generate(Conversation conversation)
        {
            try
            {
                foreach (Message message in conversation.Messages)
                {
                    if (userActivity.ContainsKey(message.SenderId))
                        userActivity[message.SenderId] += 1;
                    else userActivity.Add(message.SenderId, 1);
                }
            }
            catch (NullReferenceException e)
            {
                throw new ArgumentException("Conversation has no messages. Messages is null.", e);
            }           

            userActivity = userActivity.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

            foreach (string user in userActivity.Keys)
            {
                UserActivityRanking userRank = new UserActivityRanking();
                userRank.User = user;
                userRank.MessageCount = userActivity[user];
                userActivityRanking.Add(userRank);
            }

            if (conversation.Messages.Count() > 0)
                CalculateTopUser();

            conversation.Report = report;

            return userActivity;
        }

        /// <summary>
        /// Calculates the top user based on message activity.
        /// </summary>
        private void CalculateTopUser()
        {
            report.UserActivityRanking = userActivityRanking.ToArray();

            if (report.UserActivityRanking.Count() > 1)
            {
                for (int i = 0; i < report.UserActivityRanking.Count(); i++)
                {
                    if (i < report.UserActivityRanking.Count() - 1)
                    {
                        if (report.UserActivityRanking[i].MessageCount == report.UserActivityRanking[i + 1].MessageCount)
                        {
                            topUsers.Add(report.UserActivityRanking[i].User);
                            topUsers.Add(report.UserActivityRanking[i + 1].User);
                        }
                        else
                        {
                            break;
                        }
                    }
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

                report.MostActiveUser = tiedTopUser;
            }
        }
    }
}
