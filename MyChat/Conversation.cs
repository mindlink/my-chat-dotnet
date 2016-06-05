namespace MindLink.Recruitment.MyChat
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// Represents the model of a conversation.
    /// </summary>
    public sealed class Conversation
    {
        public string Name { get; }

        public IEnumerable<Message> Messages { get; }
        
        public string ReportMostActiveUser { get; set; }

        public Dictionary<string, int> UserMessageCount { get; set; }

        public Conversation(string name, IEnumerable<Message> messages, string reportMostActiveUser, Dictionary<string, int> userMessageCount)
        {
            Name = name;
            Messages = messages;
            ReportMostActiveUser = reportMostActiveUser;
            UserMessageCount = userMessageCount;
        }
    }
}
