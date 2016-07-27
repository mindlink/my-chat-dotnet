namespace MindLink.Recruitment.MyChat.UI.Console
{
    using Application.Commands;
    using Application.Handlers;
    using Application.Results;
    using Application.Services;
    using Config;
    using Controllers;
    using Options;
    using Serialization;

    class Program
    {
        public static IServiceLocator ServiceLocator { get; private set; }

        static Program()
        {
            SetupServiceLocator();
        }

        static void Main(string[] args)
        {
            new OptionsParser(
                onSuccess: Run,
                onError: HandleOptionsError,
                onHelp: HandleOptionsHelp
            ).Parse(args);
        }

        private static void Run(Options.Options options)
        {
            var controller = ServiceLocator.GetService<IConversationController>();
            controller.Export(options);
        }

        private static void HandleOptionsError(string error)
        {
            System.Console.WriteLine(error);
        }

        private static void HandleOptionsHelp(string help)
        {
            System.Console.WriteLine(help);
        }

        private static void SetupServiceLocator()
        {
            var container = new SimpleInjector.Container();
            container.Register<IConversationReader, DefaultConversationReader>(SimpleInjector.Lifestyle.Singleton);
            container.Register<ISerializer, JsonSerializer>(SimpleInjector.Lifestyle.Singleton);
            container.Register<ICommandHandler<ExportConversationCommand, ExportConversationResult>, ConversationHandler>(SimpleInjector.Lifestyle.Transient);
            container.Register<IConversationController, ConversationController>();

            ServiceLocator = new SimpleInjectorServiceLocator(container);
        }
    }
}
