using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq; 

namespace MindLink.Recruitment.MyChat
{
    /// <summary>
    /// Stores the functions to edit the exported JSON
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
            if(this.isReportNeeded){ this.AddReport(conversation); }
        }

        private void AddReport(Conversation conversation)
        {
            var report = new List<Activity>();
            foreach(Message message in conversation.messages)
            {
                if (report.Any(record=>record.sender == message.senderId) == false)
                {
                    int count = conversation.messages.Count(count => count.senderId == message.senderId);
                    var record = new Activity(message.senderId, count);
                    report.Add(record);
                }
            }
            conversation.addReport(report.OrderByDescending(record => record.count));
        }
    }
}