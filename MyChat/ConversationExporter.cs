namespace MyChat
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Configuration;
    using MindLink.Recruitment.MyChat;
    using MindLink.Recruitment.MyChat.Filters;

    /// <summary>
    /// Represents a conversation exporter that can read a conversation and write it out in JSON.
    /// </summary>
    public sealed class ConversationExporter
    {
        private readonly IEnumerable<IFilter> _filters;

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

            var filters = new List<IFilter>();

            if(!string.IsNullOrEmpty(exporterConfiguration.FilterByUser))
            {
                filters.Add(new UserFilter(exporterConfiguration.FilterByUser.Split(',')));
            }

            if(!string.IsNullOrEmpty(exporterConfiguration.FilterByKeyword))
            {
                filters.Add(new KeywordFilter(exporterConfiguration.FilterByKeyword.Split(',')));
            }

            if(!string.IsNullOrEmpty(exporterConfiguration.Blacklist))
            {
                filters.Add(new Blacklist(exporterConfiguration.Blacklist.Split(',')));
            }

            if (exporterConfiguration.Report)
            {
                filters.Add(new Report());
            }

            var conversationExporter = new ConversationExporter(filters);
            conversationExporter.ExportConversation(exporterConfiguration.InputFilePath, exporterConfiguration.OutputFilePath);
        }

        public ConversationExporter(IEnumerable<IFilter> filters)
        {
            _filters = filters;
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
            var conversationReader = new ConversationReader();
            var conversationWriter = new ConversationWriter();
            
            var conversation = conversationReader.ReadConversation(inputFilePath);

            foreach(var filter in _filters)
            {
                conversation = filter.Filter(conversation);
            }

            conversationWriter.WriteConversation(conversation, outputFilePath);

            Console.WriteLine("Conversation exported from '{0}' to '{1}'", inputFilePath, outputFilePath);
        }
    }
}
