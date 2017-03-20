
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
        /// The filtering option. (Filter By User/Filter By Keyword)
        /// These options cannot be combined together.
        /// </summary>
        public string Filter;

        /// <summary>
        /// The blacklist for bad words removal.
        /// This option cannot be combined with filtering. (see above).
        /// </summary>
        public string[] blacklistedStrings;

        /// <summary>
        /// The option that allows hiding credit card numbers and phone numbers.
        /// Can only be combined with blacklisted words. 
        /// </summary>
        public bool RemoveSensitiveInfo;
        
        /// <summary>
        /// The option that randomizes each senderId.
        /// This option is used independently of other options.
        /// </summary>
        public bool ObfuscateSenders;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConversationExporterConfiguration"/> class.
        /// </summary>
        /// <param name="inputFilePath">
        /// The input file path.
        /// </param>
        /// <param name="outputFilePath">
        /// The output file path.
        /// </param>
        /// <param name="Filter">
        /// The filtering option for the output file's data.
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


        /// <summary>
        /// Initializes a new instance of the <see cref="ConversationExporterConfiguration"/> class.
        /// </summary>
        /// <param name="inputFilePath">
        /// The input file path.
        /// </param>
        /// <param name="outputFilePath">
        /// The output file path.
        /// </param>
        /// <param name="Filter">
        /// The filtering option for the output file's data.
        /// </param>
        public ConversationExporterConfiguration(string inputFilePath, string outputFilePath, string Filter)
        {
            this.inputFilePath = inputFilePath;
            this.outputFilePath = outputFilePath;
            this.Filter = Filter;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConversationExporterConfiguration"/> class.
        /// </summary>
        /// <param name="inputFilePath">
        /// The input file path.
        /// </param>
        /// <param name="outputFilePath">
        /// The output file path.
        /// </param>
        /// <param name="blacklistedStrings">
        /// The filtering option for the output file's bad words removal.
        public ConversationExporterConfiguration(string inputFilePath, string outputFilePath,
            string[] blacklistedStrings)
        {
            this.inputFilePath = inputFilePath;
            this.outputFilePath = outputFilePath;
            this.blacklistedStrings = blacklistedStrings;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConversationExporterConfiguration"/> class.
        /// </summary>
        /// <param name="inputFilePath">
        /// The input file path.
        /// </param>
        /// <param name="outputFilePath">
        /// The output file path.
        /// </param>
        /// <param name="blacklistedStrings">
        /// The filtering option for the output file's bad words removal.
        /// </param>
        /// <param name="removeSensitiveInfo">
        /// The filtering option for additional information removal.
        /// </param>
        public ConversationExporterConfiguration(string inputFilePath, string outputFilePath,
            string[] blacklistedStrings, bool removeSensitiveInfo)
        {
            this.inputFilePath = inputFilePath;
            this.outputFilePath = outputFilePath;
            this.blacklistedStrings = blacklistedStrings;
            this.RemoveSensitiveInfo = removeSensitiveInfo;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ConversationExporterConfiguration"/> class.
        /// </summary>
        /// <param name="inputFilePath">
        /// The input file path.
        /// </param>
        /// <param name="outputFilePath">
        /// The output file path.
        /// </param>
        /// <param name="obfuscateSenders">
        /// The obfuscation option for hiding each sender's Id in export.
        /// </param>
        public ConversationExporterConfiguration(string inputFilePath, string outputFilePath, bool obfuscateSenders)
        {
            this.inputFilePath = inputFilePath;
            this.outputFilePath = outputFilePath;
            this.ObfuscateSenders = obfuscateSenders;
        }
    }
}
