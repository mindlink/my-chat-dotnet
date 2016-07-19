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
        public string inputFilePath;

        /// <summary>
        /// The output file path.
        /// </summary>
        public string outputFilePath;

        public bool filtersActive;
        public string usernameFilter { get; set; }
        public string keyword { get; set; }
        public string[] wordsBlacklist { get; set; }
        public bool obfuscateUserIDsFlag;
        public Dictionary<string, string> usersMapper;

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
            this.filtersActive = false;
            this.obfuscateUserIDsFlag = false;
            this.inputFilePath = inputFilePath;
            this.outputFilePath = outputFilePath;
            usersMapper = new Dictionary<string, string>();

            
        }
    }
}
