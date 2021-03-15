namespace MyChat
{
    using System;
    using System.Collections.Generic;
    using MindLink.Recruitment.MyChat;

    public static class ConversationModifier
    {
        /// <summary>
        /// Helper function to check whether given conversation entry is within given filters.
        /// </summary>
        /// <param name="senderId">
        /// String username of the message sender.
        /// </param>
        /// <param name="content">
        /// String contents of the message.
        /// </param>
        /// <param name="filterByUser">
        /// SenderId by which to filter the conversation.
        /// </param>
        /// <param name="filterByKeyword">
        /// Keyword by which to filter the conversation.
        /// </param>
        /// <returns>
        /// Returns true iff message passes all filters, false if not. 
        /// </returns>
        public static bool IsInFilters(DateTimeOffset timestamp, string senderId, string content, ConversationExporterParameters exporterParameters)
        {

            if (exporterParameters.FilterByUser != null && senderId != exporterParameters.FilterByUser)
            {
                return false;
            }
            if (exporterParameters.FilterByKeyword != null && !content.Contains(exporterParameters.FilterByKeyword))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Helper function which applies redaction on the <paramref name="content"/> using the <paramref name="blacklist"/>
        /// </summary>
        /// <param name="content">
        /// Message string on which to apply redactions.
        /// </param>
        /// <param name="blacklist">
        /// Array of strings to be replaced.
        /// </param>
        /// <returns>
        /// String <paramref name="content"/> with blacklisted words replaced with "\\*redacted\\*"
        /// </returns>
        public static string ApplyBlacklist(string content, string[] blacklist)
        {
            if (blacklist != null)
            {
                foreach (string redaction in blacklist)
                {
                    content = content.Replace(redaction, @"\*redacted\*", true, null);
                }
            }
            return content;
        }

        /// <summary>
        /// Helper function to generate a report of activity as a list.
        /// </summary>
        /// <param name="messageCount">
        /// Dictionary holding a tally of number of messages sent by each sender.
        /// </param>
        /// <param name="report">
        /// Boolean marker for whether to include the report after the messages. 
        /// </param>
        /// <returns>
        /// A list of message counts for each sender sorted in descending order.
        /// </returns>
        public static List<Activity> GenerateReport(List<Message> messages, ConversationExporterParameters exporterParameters)
        {
            var activity = new List<Activity>();

            if (exporterParameters.Report)
            {
                var messageCount = new Dictionary<string, int>();

                foreach (Message message in messages)
                {
                    if (messageCount.ContainsKey(message.senderId))
                    {
                        messageCount[message.senderId]++;
                    }
                    else
                    {
                        messageCount[message.senderId] = 1;
                    }
                }

                foreach (KeyValuePair<string, int> entry in messageCount)
                {
                    activity.Add(new Activity(entry.Key, entry.Value));
                }
                activity.Sort((x, y) => -x.count.CompareTo(y.count));
            }
            else
            {
                activity = null;
            }
            return activity;
        }

        public static Message ApplyMessageModifiers(DateTimeOffset timestamp, string senderId, string content, ConversationExporterParameters exporterParameters)
        {
            if (IsInFilters(timestamp, senderId, content, exporterParameters))
            {
                content = ApplyBlacklist(content, exporterParameters.Blacklist);
                return new Message(timestamp, senderId, content);
            }
            else
            {
                return null;
            }
        }
    }
}
