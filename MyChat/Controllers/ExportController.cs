namespace MindLink.Recruitment.MyChat.Controllers
{
    using MindLink.Recruitment.MyChat.Interfaces.ControllerInterfaces;
    using MyChatModel.ModelData;
    using System;
    using System.Collections.Generic;
    using System.Text;
    public sealed class ExportController : IExportController
    {
        // IReadController variable, called readController
        private IReadController readController;
        // IWriteController variable, called writeController
        private IWriteController writeController;
        // IFilterController variable, called filterController
        private IFilterController filterController;

        /// <summary>
        /// CONSTRUCTOR for ExportController class, takes 2 params
        /// </summary>
        /// <param name="readController"> <see cref="IReadController"> variable </param>
        /// <param name="writeController"> <see cref="IWriteController"> variable </param>
        public ExportController(IReadController readController, IWriteController writeController, IFilterController filterController) 
        {
            // SET the read, write and filter controller variables passed in 
            // to their respective local variables in this class
            this.readController = readController;
            this.writeController = writeController;
            this.filterController = filterController;
        }

        /// <summary>
        /// Helper method to read the conversation from <paramref name="inputFilePath"/>.
        /// </summary>
        /// <param name="inputFilePath">
        /// The input file path.
        /// </param>
        /// <returns>
        /// A <see cref="Conversation"/> model representing the conversation.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the input file could not be found.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown when something else went wrong.
        /// </exception>
        public void ExportConversation(string inputFilePath, string outputFilePath)
        {
            Conversation conversation = readController.ReadConversation(inputFilePath);

            if (filterController.FiltersToApply) 
            {
                conversation = filterController.FilterConversation(conversation);
            }

            writeController.WriteConversation(conversation, outputFilePath);

            Console.WriteLine("Conversation exported from '{0}' to '{1}'", inputFilePath, outputFilePath);
        }
    }
}
