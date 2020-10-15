namespace MindLink.Recruitment.MyChat
{
    using System.Collections.Generic;
    using System;
    using System.IO;
    using System.Security;
    using Newtonsoft.Json;
    using System.Linq; 

    /// <summary>
    /// creates a log to output
    /// </summary>

    public sealed class LogCreator
    {
        public bool isReportNeeded;
        public LogCreator(EditorConfiguration config)
        {
            this.isReportNeeded = config.isReportNeeded;
        }

        public Log CreateLog(Conversation conversation)
        {
            if (this.isReportNeeded) {
                return  new LogWithReport(conversation, this.AddReport(conversation));
            }
            else {
                return new Log(conversation);
            }
        }

        public List<Activity> AddReport(Conversation conversation)
        {
            var activityList = new List<Activity>();
            foreach(Message message in conversation.messages)
            {
                if (activityList.Any(activity=>activity.sender == message.senderId) == false)
                {
                    int count = conversation.messages.Count(count => count.senderId == message.senderId);
                    var activity = new Activity(message.senderId, count);
                    activityList.Add(activity);
                }
            }
            return activityList.OrderByDescending(activity => activity.count).ToList();
        }
    }
}