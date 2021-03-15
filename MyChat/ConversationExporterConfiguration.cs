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
        /// Filter via username
        /// </summary>
        public string filterByUser { get; set; }

        /// <summary>
        /// Filter via keyword
        /// </summary>
        public string filterByKeyword { get; set; }

        /// <summary>
        /// Blacklist words
        /// </summary>
        public string blacklist { get; set; }

        public bool generateReport { get; set; }
    }
}
