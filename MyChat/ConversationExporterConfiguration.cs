namespace MindLink.Recruitment.MyChat
{
    using global::MyChat;
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
        public string inputFilePath;

        /// <summary>
        /// The output file path.
        /// </summary>
        public string outputFilePath;


        public string user;

        public string keyword;

        public List<string> blacklist;

        public bool hideSensitiveData;


        /// <summary>
        /// Initializes a new instance of the <see cref="ConversationExporterConfiguration"/> class.
        /// </summary>
        /// <param name="inputFilePath">
        /// The input file path.
        /// </param>
        /// <param name="outputFilePath">
        /// The output file path.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any of the given arguments is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when any of the given arguments is empty.
        /// </exception>
        public ConversationExporterConfiguration(string inputFilePath, string outputFilePath)
        {
            this.inputFilePath = inputFilePath;
            this.outputFilePath = outputFilePath;

        }

        public List<FilterType> GetFilterList()
        {
            List<FilterType> list = new List<FilterType>();

            if (user != null ) { list.Add(FilterType.SENDER_ID); }
            if (keyword != null) { list.Add(FilterType.KEYWORD); }
            if (blacklist != null) { list.Add(FilterType.BLACKLIST); }
            if (hideSensitiveData) { list.Add(FilterType.HIDE_SENSITIVE_DATA); }

            return list;
        }




    }
}
