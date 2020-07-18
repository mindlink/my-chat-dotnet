﻿namespace MindLink.Recruitment.MyChat
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

            IConversationReader reader = new ConversationReader(reportGenerator);
            IConversationWriter writer = new ConversationWriter();
            IConversationFilter filter = new ConversationFilter();
            ICommandLineParser cmdParser = new CommandLineParser();

            ExportController controller = new ExportController(reader, writer, filter, cmdParser);

            if (args.Length == 0)
            {
                args = new string[] { "chat.txt", "chat.json", "-hcc" };
            }
            controller.Export(args);
        }
    }
}