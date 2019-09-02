namespace MindLink.Recruitment.MyChat
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Linq;
    using MindLink.Recruitment.MyChat;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents an activity reporter that can read a conversation and report message activity data.
    /// </summary>
    public static class ActivityReporter
    {
        public static string[] ActivityReport(Conversation conversation)
        {
            var nameList = new List<string>();
            foreach (var message in conversation.messages)
            {
                nameList.Add(message.userId);
            }
            var frequency = nameList.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
            var frequencyList = frequency.ToList();

            frequencyList.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));
            string[] activityReport = new string[frequencyList.Count()];
            for (int i = 0; i < frequencyList.Count(); i++)
            {
                activityReport[i] = $"UserId: {frequencyList[i].Key}, Messages sent = {frequencyList[i].Value}";
            }
            return activityReport;
        }
    }
}
