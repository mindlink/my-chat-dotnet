using Newtonsoft.Json;
using System.Collections.Generic;

namespace MindLink.Recruitment.MyChat
{
    public class Report
    {
        public string MostActiveUser { get; }

        public Dictionary<string, int> UserMessageCount { get; }

        public Report(string mostActive, Dictionary<string, int> messageCount)
        {
            MostActiveUser = mostActive;
            UserMessageCount = messageCount;
        }

    }
}
