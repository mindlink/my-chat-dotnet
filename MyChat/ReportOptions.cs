namespace MindLink.Recruitment.MyChat
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Abstract class for report classes
    /// </summary>
    public abstract class ReportOptions : ConversationOptions
    {
        public abstract List<Report> GenerateReport(List<Message> messages);
    }


    public class MessageCountPerUserReportOption : ReportOptions
    {
        /// <summary>
        /// Helper method to calculate message count per user
        /// </summary>
        /// <param name="uniqueNameList">
        /// A list of unique names from all sender in the conversation
        /// </param>
        /// <param name="messages">
        /// List of <see cref="Message">
        /// </param>
        /// <returns></returns>
        public Dictionary<string,int> CalculateMessageCountPerUser(IEnumerable<string> uniqueNameList, List<Message> messages)
        {
            try
            {
                Dictionary<string, int> nameMessageCountPairs = new Dictionary<string, int>();

                foreach (var uniqueName in uniqueNameList)
                {
                    nameMessageCountPairs.Add(uniqueName, 0);
                }

                foreach (var message in messages)
                {
                    nameMessageCountPairs[message.senderId]++;
                }

                return nameMessageCountPairs;
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("Something went wrong with the unique names in uniqueNameList");
            }
        }

        /// <summary>
        /// Generate an ordered list of <see cref="Report">
        /// </summary>
        /// <param name="messages">
        /// List of <see cref="Message">
        /// </param>
        /// <returns>
        /// List of <see cref="MessageCountPerUserReport">
        /// </returns>
        public override List<Report> GenerateReport(List<Message> messages)
        {
            try
            {
                var uniqueNameList = messages.Select((message) => message.senderId).Distinct();

                var reportList = new List<Report>();

                Dictionary<string, int> nameMessageCountPairs = CalculateMessageCountPerUser(uniqueNameList, messages);

                var nameMessageCountPairList = nameMessageCountPairs.ToList();

                nameMessageCountPairList.OrderByDescending(p => p.Value);

                foreach (var nameMessageCountPair in nameMessageCountPairList)
                {
                    reportList.Add(new Report(nameMessageCountPair.Key, nameMessageCountPair.Value));
                }
                
                return reportList;
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("Something went wrong with some arguments passed to functions");
            }
        }
    }
}