namespace MindLink.Recruitment.MyChat
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// model for the output, to allow report to be optional in children
    /// </summary>
    public class Log
    {
        /// <summary>
        /// The name of the conversation.
        /// </summary>
        public string name;

        /// <summary>
        /// The messages in the conversation.
        /// </summary>
        public IEnumerable<Message> messages;
        public Log(Conversation conversation)
        {
            this.name = conversation.name;
            this.messages = conversation.messages;
        }
    }

    public sealed class LogWithReport : Log
    {
        // JsonProperty(Order) means that the activity is ordered after the Log name and message
        [JsonProperty(Order = 0)]
        public IEnumerable<Activity> activity;
        public LogWithReport(Conversation conversation, List<Activity> report) : base(conversation)
        {
            this.name = conversation.name;
            this.messages = conversation.messages;
            this.activity = report; 
        }

    }
}