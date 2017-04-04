namespace MindLink.MyChat.Domain.Filters
{
    public interface IMessageFilter
    {
        bool IncludeMessage(Message message);
    }
}