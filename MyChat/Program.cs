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
            // INITIALISE an IReadController and IWriteController as their respective
            // concrete implementations, ReadController and WriteController
            IReadController readController = new ReadController();
            IWriteController writeController = new WriteController();
            // INITIALISE an IExportController as ExportController, passing in the readController and WriteController
            IExportController conversationExporter = new ExportController(readController, writeController);

            ConversationExporterConfiguration configuration = new CommandLineArgumentParser().ParseCommandLineArguments(args);

            conversationExporter.ExportConversation(configuration.inputFilePath, configuration.outputFilePath);
        }
    }
}
