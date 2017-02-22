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

        /// <summary>
        /// Include Keyboard.
        /// </summary>
        public string keyboard = "";

        /// <summary>
        /// Search by User.
        /// </summary>
        public string user = "";

        /// <summary>
        /// Blacklisted word.
        /// </summary>
        public string blacklist = "";

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
        public ConversationExporterConfiguration(ProgramArguments args)
        {
            this.inputFilePath = args.inputFile;
            this.outputFilePath = args.outFile;
            this.user = args.user;
            this.keyboard = args.keyboard;
            this.blacklist = args.blacklist;
        }
    }
}
