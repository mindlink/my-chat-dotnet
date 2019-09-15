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
            if (inputFilePath == null || outputFilePath == null)
            {
                throw new ArgumentNullException("Argument cannot be null");
            }
            else if (inputFilePath == "" || outputFilePath == "")
            {
                throw new ArgumentException("Argument cannot be empty");
            }
            this.inputFilePath = inputFilePath;
            this.outputFilePath = outputFilePath;
        }
    }
}
