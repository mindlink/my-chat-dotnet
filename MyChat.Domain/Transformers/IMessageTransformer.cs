namespace MindLink.MyChat.Domain.Transformers
{
    public interface IMessageTransformer
    {
        Message TransformMessage(Message message);
    }
}