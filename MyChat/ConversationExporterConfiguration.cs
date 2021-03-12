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
        /// Name of user to filter messages with
        /// </summary>
        public string FilterByUser { get; set; }

        /// <summary>
        /// Word to filter messages with
        /// </summary>
        public string FilterByWord { get; set; }
    }
}
