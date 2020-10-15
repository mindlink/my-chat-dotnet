namespace MindLink.Recruitment.MyChat
{
    using System.Collections.Generic;
    using System.Linq; 

    /// <summary>
    /// Stores the filters to edit the exported JSON and generates object to output
    /// </summary>

    public sealed class ConversationEditor
    {
        public IList<Filter> filters;
        public bool isReportNeeded;
        public ConversationEditor(EditorConfiguration config)
        {
            this.filters =  new List<Filter>();
            if (config.filterByUser != null)
            {
                this.filters.Add(new SenderFilter(config.filterByUser));
            }
            if (config.filterByKeyword != null)
            {
                this.filters.Add(new KeywordFilter(config.filterByKeyword));
            }
            if (config.blacklist != null) {
                this.filters.Add(new BlacklistFilter(config.blacklist.Split(',')));
            }
            this.isReportNeeded = config.isReportNeeded;
        }
       public void EditConversation(Conversation conversation)
        {
            foreach(Filter filter in this.filters)
            {
                filter.ApplyFilter(conversation);
            }
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
    }
}