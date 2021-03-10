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
        /// The user by which to filter messages.
        /// </summary>
        public string FilterByUser { get; set; }

        /// <summary>
        /// The keyword by which to filter messages.
        /// </summary>
        public string FilterByKeyword { get; set; }

        /// <summary>
        /// The list of blacklisted words.
        /// </summary>
        public string BlackList { get; set; }

        public bool Report { get; set; }


    }
}
