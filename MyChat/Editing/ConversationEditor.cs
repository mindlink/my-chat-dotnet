namespace MindLink.Recruitment.MyChat
{
    using System.Collections.Generic; 

    /// <summary>
    /// Stores the filters to edit the exported JSON and generates object to output
    /// </summary>

    public sealed class ConversationEditor
    {
        public IList<Filter> filters;
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
        }
       public void EditConversation(Conversation conversation)
        {
            foreach(Filter filter in this.filters)
            {
                filter.ApplyFilter(conversation);
            }
        }
    }
}