namespace MyChat
{
    using System;
    using Microsoft.Extensions.Configuration;
    using MindLink.Recruitment.MyChat;

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
            // We use Microsoft.Extensions.Configuration.CommandLine and Configuration.Binder to read command line arguments.
            var configuration = new ConfigurationBuilder().AddCommandLine(args).Build();
            var exporterConfiguration = configuration.Get<ConversationExporterConfiguration>();

            var conversationExporter = new ConversationExporter();
            conversationExporter.ExportConversation(exporterConfiguration.InputFilePath, exporterConfiguration.OutputFilePath);
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
        public void ExportConversation(string inputFilePath, string outputFilePath)
        {
            ConversationReader conversationReader = new ConversationReader();
            ConversationWriter conversationWriter = new ConversationWriter();

            Conversation conversation = conversationReader.ReadConversation(inputFilePath);

            conversationWriter.WriteConversation(conversation, outputFilePath);

            Console.WriteLine("Conversation exported from '{0}' to '{1}'", inputFilePath, outputFilePath);
        }
    }
}
