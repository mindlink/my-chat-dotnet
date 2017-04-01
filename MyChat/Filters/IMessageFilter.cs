namespace MindLink.MyChat.Filters
{
    public interface IMessageFilter
    {
        bool IncludeMessage(string message);
    }
}