using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using MindLink.MyChat.Domain.Filters;
using MindLink.MyChat.Domain.Transformers;

namespace MindLink.MyChat.Domain
{
    /// <summary>
    ///     Represents the configuration for the exporter.
    /// </summary>
    public sealed class ConversationExporterConfiguration
    {
        /// <summary>
        ///     The input stream.
        /// </summary>
        public Stream InputStream { get; }

        /// <summary>
        ///     The output stream.
        /// </summary>
        public Stream OutputStream { get; }

        public ReadOnlyCollection<IMessageFilter> Filters => this.filters.AsReadOnly();

        private readonly List<IMessageFilter> filters = new List<IMessageFilter>();

        public ReadOnlyCollection<IMessageTransformer> Transformers => this.transformers.AsReadOnly();

        private readonly List<IMessageTransformer> transformers = new List<IMessageTransformer>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConversationExporterConfiguration" /> class.
        /// </summary>
        /// <param name="inputStream">
        ///     The input file path.
        /// </param>
        /// <param name="outputStream">
        ///     The output file path.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when any of the given arguments is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Thrown when any of the given arguments is empty.
        /// </exception>
        public ConversationExporterConfiguration(Stream inputStream, Stream outputStream)
        {
            this.InputStream = inputStream;
            this.OutputStream = outputStream;
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