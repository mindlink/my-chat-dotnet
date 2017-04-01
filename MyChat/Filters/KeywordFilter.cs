namespace MindLink.MyChat.Filters
{
    public class KeywordFilter : IMessageFilter
    {
        private readonly string keyword;

        public KeywordFilter(string keyword)
        {
            this.keyword = keyword;
        }

        public bool IncludeMessage(string message)
        {
            return message.Contains(this.keyword);
        }
    }
}