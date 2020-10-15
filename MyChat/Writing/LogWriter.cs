namespace MindLink.Recruitment.MyChat
{
    using System.Collections.Generic;
    using System;
    using System.IO;
    using System.Security;
    using Newtonsoft.Json;
    using System.Linq; 

    /// <summary>
    /// writes logs to output
    /// </summary>

    public sealed class LogWriter
    {
        public bool isReportNeeded;
        public LogWriter(EditorConfiguration config)
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


        private List<Activity> AddReport(Conversation conversation)
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

         public void WriteLogToOutput(Conversation conversation, string outputFilePath)
        {
            var log = CreateLog(conversation);
            try
            {
                var writer = new StreamWriter(new FileStream(outputFilePath, FileMode.Create, FileAccess.ReadWrite));

                var serialized = JsonConvert.SerializeObject(log, Formatting.Indented);

                writer.Write(serialized);

                writer.Flush();

                writer.Close();
            }
            catch (SecurityException)
            {
                throw new ArgumentException("No permission to file.");
            }
            catch (DirectoryNotFoundException)
            {
                throw new ArgumentException("Path invalid.");
            }
            catch (IOException)
            {
                throw new Exception("Something went wrong in the IO.");
            }
        }
    }
}