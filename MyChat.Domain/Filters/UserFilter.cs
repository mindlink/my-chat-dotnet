namespace MindLink.MyChat.Domain.Filters
{
    public class UserFilter : IMessageFilter
    {
        private readonly string user;

        public UserFilter(string user)
        {
            this.user = user;
        }

        public bool IncludeMessage(Message message)
        {
            return message.SenderId == this.user;
        }
    }
}
