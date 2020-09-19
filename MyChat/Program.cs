namespace MindLink.Recruitment.MyChat
{
    using MindLink.Recruitment.MyChat.Controllers;
    using MindLink.Recruitment.MyChat.Interfaces.ControllerInterfaces;
    using System;
    using System.Collections.Generic;
    using System.Text;
    public sealed class Program
    {

        /// <summary>
        /// The application entry point.
        /// </summary>
        /// <param name="args">
        /// The command line arguments.
        /// </param>
        static void Main(string[] args)
        {
            // INITIALISE an IReadController, IWriteController, IFilterController, IReportController as their respective
            // concrete implementations, ReadController, WriteController, FilterController and ReportController
            IReadController readController = new ReadController();
            IWriteController writeController = new WriteController();
            IFilterController filterController = new FilterController();
            IReportController reportController = new ReportController();
            // INITIALISE an IExportController as ExportController, passing in the readController and WriteController
            IExportController conversationExporter = new ExportController(readController, writeController, filterController, reportController);

            ConversationExporterConfiguration configuration = new CommandLineArgumentParser().ParseCommandLineArguments(args, filterController);

            conversationExporter.ExportConversation(configuration.inputFilePath, configuration.outputFilePath);
        }
    }
}
