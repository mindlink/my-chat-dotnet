namespace MindLink.Recruitment.MyChat
{
    /// <summary>
    /// Startup objects, defines project dependencies.
    /// </summary>
    class Program
    {
        /// <summary>
        /// The application entry point, inject relevant modules.
        /// </summary>
        /// <param name="args">
        /// The command line arguments.
        /// </param>
        static void Main(string[] args)
        {
            IReportGenerator reportGenerator = new ReportGenerator();

            IConversationReader reader = new ConversationReader();
            IConversationWriter writer = new ConversationWriter();
            IConversationFilter filter = new ConversationFilter();
            ICommandLineParser cmdParser = new CommandLineParser();

            ExportController controller = new ExportController(reader, writer, filter, cmdParser, reportGenerator);

            controller.Export(args);
        }
    }
}