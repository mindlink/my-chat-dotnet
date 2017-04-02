namespace MindLink.Recruitment.MyChat
{
    using System;

    /// <summary>
    /// Represents the configuration for the exporter.
    /// </summary>
    public class ConversationExporterConfiguration
    {
        /// <summary>
        /// The input file path.
        /// </summary>
        string inputFilePath;

        /// <summary>
        /// The output file path.
        /// </summary>
        string outputFilePath;

        /// <summary>
        /// Sets  or returns the input file path
        /// </summary>
        public string InputFilePath
        {                   
            get { return this.inputFilePath; }
            set { this.inputFilePath = value; }
        }   

        /// <summary>
        /// Sets or returns the output file path
        /// </summary>
        public string OutputFilePath { 
            get { return this.outputFilePath;}
            set { this.outputFilePath = value;}
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
        /// <exception cref="ArgumentNullException">
        /// Thrown when any of the given arguments is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when any of the given arguments is empty.
        /// </exception>
        public ConversationExporterConfiguration(string inputFilePath, string outputFilePath)
        {
            InputFilePath = inputFilePath;
            OutputFilePath = outputFilePath;
        }
    }
}
