namespace MindLink.MyChat.Filters
{
    public interface IMessageFilter
    {
        bool IncludeMessage(Message message);
    }
}