namespace MindLink.Recruitment.MyChat.Application.Handlers
{
    public interface ICommandHandler<in TCommand, out TResult>
    {
        TResult Handle(TCommand command);
    }
}
