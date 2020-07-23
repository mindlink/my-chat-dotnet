namespace MindLink.Recruitment.MyChat
{
    using MindLink.Recruitment.MyChat.CommandLineParsing;
    using MindLink.Recruitment.MyChat.ConversationReaders;
    using MindLink.Recruitment.MyChat.ConversationWriters;
    using MindLink.Recruitment.MyChat.ConversationFilters;
    using MindLink.Recruitment.MyChat.ReportGeneration;

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