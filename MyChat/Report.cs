using System.Collections.Generic; 

namespace MindLink.Recruitment.MyChat
{
    /// <summary>
    /// Represents the model of a report.
    /// </summary>
    public sealed class Report
    {
        public IEnumerable<Activity> report;
        public Report(IEnumerable<Activity> activity)
        {
            this.report = activity;
        }
    }
}