namespace MindLink.Recruitment.MyChat
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents the configuration for the exporter.
    /// </summary>
    public sealed class ConversationExporterConfiguration
    {
        /// <summary>
        /// The input file path.
        /// </summary>
        public string inputFilePath { get; }

        /// <summary>
        /// The output file path.
        /// </summary>
        public string outputFilePath { get; }

        /// <summary>
        /// The user filter.
        /// </summary>
        public string user { get; }

        /// <summary>
        /// The keyword filter.
        /// </summary>
        public string keyword { get; }

        /// <summary>
        /// The list of words to be redacted.
        /// </summary>
        public List<string> blacklist { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConversationExporterConfiguration"/> class.
        /// </summary>
        /// <param name="inputFilePath">
        /// The input file path.
        /// </param>
        /// <param name="outputFilePath">
        /// The output file path.
        /// </param>
        /// <param name="user">
        /// The user for message filtering.
        /// </param>
        /// <param name="keyword">
        /// The keyword for message filtering.
        /// </param>
        /// <param name="blacklist">
        /// The list of words to be replaced with "*redacted*".
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any of the given arguments is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when any of the given arguments is empty.
        /// </exception>
        public ConversationExporterConfiguration(string inputFilePath, string outputFilePath, string user = "", string keyword = "", List<string> blacklist = null)
        {
            this.inputFilePath = inputFilePath;
            this.outputFilePath = outputFilePath;
            this.user = user;
            this.keyword = keyword;
            this.blacklist = blacklist;
        }
    }
}
