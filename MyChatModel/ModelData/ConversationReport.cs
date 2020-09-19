namespace MyChatModel.ModelData
{

    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Conversation Report, contains data from the conversation
    /// 
    /// </summary>
    public sealed class ConversationReport
    {
        /// <summary>
        /// string to contain the most active user
        /// </summary>
        public string mostActiveUser { get; }

        public IList<string> userActivityRanking { get; }

        /// <summary>
        /// Constructor for ConversationReport
        /// </summary>
        public ConversationReport(string mostActive, IList<string> activityRanking) 
        {
            mostActiveUser = mostActive;

            userActivityRanking = activityRanking;
        }


    }
}
