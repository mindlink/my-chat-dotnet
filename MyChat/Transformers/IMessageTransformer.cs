namespace MindLink.MyChat.Transformers
{
    public interface IMessageTransformer
    {
        Message TransformMessage(Message message);
    }
}