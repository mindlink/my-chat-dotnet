namespace MindLink.Recruitment.MyChat
{
    /// <summary>
    /// Represents the configuration for the exporter.
    /// </summary>
    public sealed class ConversationExporterConfiguration
    {
        /// <summary>
        /// The input file path.
        /// </summary>
        public string InputFilePath { get; set; }

        /// <summary>
        /// The output file path.
        /// </summary>
        public string OutputFilePath { get; set; }

        /// <summary>
        /// Filters the results by a user.
        /// Example: --filterByUser bob,angus
        /// </summary>
        public string FilterByUser { get; set; }

        /// <summary>
        /// Filters the results by a keyword.
        /// Example: --filterByKeyword pie,no
        /// </summary>
        public string FilterByKeyword { get; set; }

        /// <summary>
        /// Hides a keyword by replacing it with *redacted*.
        /// Example: --blacklist hello
        /// </summary>
        public string Blacklist { get; set; }

        /// <summary>
        /// Adds a report of message count per user to the output.
        /// Example: --report true
        /// </summary>
        public bool Report { get; set; }
    }
}
