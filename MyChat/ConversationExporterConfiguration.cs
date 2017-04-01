using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MindLink.MyChat.Filters;
using MindLink.MyChat.Transformers;

namespace MindLink.MyChat
{
    /// <summary>
    ///     Represents the configuration for the exporter.
    /// </summary>
    public sealed class ConversationExporterConfiguration
    {
        /// <summary>
        ///     The input file path.
        /// </summary>
        public string InputFilePath { get; }

        /// <summary>
        ///     The output file path.
        /// </summary>
        public string OutputFilePath { get; }

        public ReadOnlyCollection<IMessageFilter> Filters => this.filters.AsReadOnly();

        private readonly List<IMessageFilter> filters = new List<IMessageFilter>();

        public ReadOnlyCollection<IMessageTransformer> Transformers => this.transformers.AsReadOnly();

        private readonly List<IMessageTransformer> transformers = new List<IMessageTransformer>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConversationExporterConfiguration" /> class.
        /// </summary>
        /// <param name="inputFilePath">
        ///     The input file path.
        /// </param>
        /// <param name="outputFilePath">
        ///     The output file path.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when any of the given arguments is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Thrown when any of the given arguments is empty.
        /// </exception>
        public ConversationExporterConfiguration(string inputFilePath, string outputFilePath)
        {
            this.InputFilePath = inputFilePath;
            this.OutputFilePath = outputFilePath;
        }

        public void AddFilter(IMessageFilter filter)
        {
            this.filters.Add(filter);
        }

        public void AddTransformer(IMessageTransformer transformer)
        {
            this.transformers.Add(transformer);
        }
    }
}