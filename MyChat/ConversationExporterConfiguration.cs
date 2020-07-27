namespace MindLink.Recruitment.MyChat
{
    using System;

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

        /// <summary>
        /// The user that needs to be found file path.
        /// </summary>
        public string user;

        /// <summary>
        /// The word that needs to be found file path.
        /// </summary>
        public string word;

        /// <summary>
        /// The blacklist file path.
        /// </summary>
        public string blacklistPath;
        public string redactedConversationPath;

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
        public ConversationExporterConfiguration(string inputFilePath, string outputFilePath,string user, string word, string blacklistPath,string redactedConversationPath)
        {
            this.inputFilePath = inputFilePath;
            this.outputFilePath = outputFilePath;
            this.user = user;
            this.word = word;
            this.blacklistPath = blacklistPath;
            this.redactedConversationPath = redactedConversationPath;
        }
    }
}
