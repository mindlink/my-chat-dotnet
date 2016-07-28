namespace MindLink.Recruitment.MyChat.UI.Console.Config
{
    using System;

    interface IServiceLocator : IServiceProvider
    {
        T GetService<T>();
    }
}
