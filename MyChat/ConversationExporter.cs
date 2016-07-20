namespace MyChat
{
    using System;
    using MindLink.Recruitment.MyChat;
    using static MindLink.Recruitment.MyChat.Models.Response;
    using MindLink.Recruitment.MyChat.Helpers;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a conversation exporter that can read a conversation and write it out in JSON.
    /// </summary>
    public sealed class ConversationExporter
    {
        /// <summary>
        /// The application entry point.
        /// </summary>
        /// <param name="args">
        /// The command line arguments.
        /// </param>
        static void Main(string[] args)
        {
            args = new String[2];
            string inputProceedAnswer = string.Empty;
            string inputFilePath = string.Empty;
            string outputFilePath = string.Empty;
            string outputProceedAnswer = string.Empty;

            #region Input filepath
            while (String.IsNullOrEmpty(inputProceedAnswer) || inputProceedAnswer.ToLower() == "n")
            {
                Console.WriteLine("Please provide the full path of the chat text file you want to export: ");
                inputFilePath = Console.ReadLine();

                while (String.IsNullOrEmpty(inputFilePath))
                {
                    Console.WriteLine("Please provide the full path of the chat text file you want to export:");
                    inputFilePath = Console.ReadLine();
                }

                Console.WriteLine("You have selected the following filepath: '{0}'", inputFilePath);
                Console.WriteLine("Are you sure you this is the correct path? [Y/N]");
                inputProceedAnswer = Console.ReadLine();

                while (String.IsNullOrEmpty(inputProceedAnswer) || (inputProceedAnswer.ToLower() != "y" && inputProceedAnswer.ToLower() != "n"))
                {
                    Console.WriteLine("Please type in 'Y' or 'N' to proceed.");
                    inputProceedAnswer = Console.ReadLine();
                }
            }
            args[0] = inputFilePath;
            #endregion

            #region Output filepath
            while (String.IsNullOrEmpty(outputProceedAnswer) || outputProceedAnswer.ToLower() == "n")
            {
                Console.WriteLine("Please provide the full path of the folder you want to export to: ");
                outputFilePath = Console.ReadLine();

                while (String.IsNullOrEmpty(outputFilePath))
                {
                    Console.WriteLine("Please provide the full path of the folder you want to export to: ");
                    outputFilePath = Console.ReadLine();
                }

                Console.WriteLine("You have selected the following filepath: '{0}'", outputFilePath);
                Console.WriteLine("Are you sure you this is the correct path? [Y/N]");
                outputProceedAnswer = Console.ReadLine();

                while (String.IsNullOrEmpty(outputProceedAnswer) || (outputProceedAnswer.ToLower() != "y" && (outputProceedAnswer.ToLower() != "n")))
                {
                    Console.WriteLine("Please type in 'Y' or 'N' to proceed.");
                    outputProceedAnswer = Console.ReadLine();
                }
            }
            args[1] = outputFilePath;
            #endregion

            var conversationExporter = new ConversationExporter();
            ConversationExporterConfiguration configuration = new CommandLineArgumentParser().ParseCommandLineArguments(args);
            ExportResponse eResponse = conversationExporter.ExportConversation(configuration.inputFilePath, configuration.outputFilePath);
            
            if (!eResponse.Successful)
                Console.WriteLine(eResponse.Message);
            Console.ReadLine();
        }

        /// <summary>
        /// Exports the conversation at <paramref name="inputFilePath"/> as JSON to <paramref name="outputFilePath"/>.
        /// </summary>
        /// <param name="inputFilePath">
        /// The input file path.
        /// </param>
        /// <param name="outputFilePath">
        /// The output file path.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when a path is invalid.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown when something bad happens.
        /// </exception>
        public ExportResponse ExportConversation(string inputFilePath, string outputFilePath)
        {
            ConversationResponse conversationResponse = ConversationHelper.ReadConversation(inputFilePath);
            if (conversationResponse.Successful && conversationResponse.Target != null)
            {
                Conversation conversation = conversationResponse.Target;

                Console.WriteLine("Would you like to filter the conversation? [Y/N]");
                string filterProceedAnswer = Console.ReadLine();

                while (String.IsNullOrEmpty(filterProceedAnswer) || (filterProceedAnswer.ToLower() != "y" && (filterProceedAnswer.ToLower() != "n")))
                {
                    Console.WriteLine("Please type in 'Y' or 'N' to proceed.");
                    filterProceedAnswer = Console.ReadLine();
                }
                if (filterProceedAnswer.ToLower() == "y")
                    conversation = ConversationFilterer.AskUserForFilters(conversation);

                List<string> distinctListOfUsers = conversation.Messages.Select(e => e.SenderId).Distinct().ToList();

                foreach (var usr in distinctListOfUsers)
                {
                    conversation.UsersActivity.Add(new Activity() { SenderId = usr, NumberOfMessages = conversation.Messages.Count(s => s.SenderId.Contains(usr)) });
                }
                conversation.UsersActivity.Sort((x, y) => x.NumberOfMessages.CompareTo(y.NumberOfMessages));
                conversation.UsersActivity.Reverse();

                WriteResponse wResponse = ConversationHelper.WriteConversation(conversation, outputFilePath);
                if (wResponse.Successful)
                {
                    Console.WriteLine("Conversation exported from '{0}' to '{1}'", inputFilePath, outputFilePath);
                    return new ExportResponse { Successful = true, Message = wResponse.Message };
                }
                else
                    return new ExportResponse { Successful = false, Message = wResponse.Message };
            }
            else
                return new ExportResponse { Successful = false, Message = conversationResponse.Message };
        }
    }
}
