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
        private string inputFilePath;

        /// <summary>
        /// The output file path.
        /// </summary>
        private string outputFilePath;

        /// <summary>
        /// The name of specific user to be filtered.
        /// </summary>
        private string userName;

        /// <summary>
        /// The keyword to filter messages.
        /// </summary>
        private string keyword;

        /// <summary>
        /// The keyword to filter messages.
        /// </summary>
        private string blacklist;

        /// <summary>
        /// Boolean flag indicating whether to encrypt usernames or not.
        /// </summary>
        private bool encryptUsernames;

        /// <summary>
        /// Boolean flag indicating whether to hide numbers like credit card or phone numbers.
        /// </summary>
        private bool hideNumbers;

        public string InputFilePath { get => inputFilePath; set => inputFilePath = value; }
        public string OutputFilePath { get => outputFilePath; set => outputFilePath = value; }
        public string UserName { get => userName; set => userName = value; }
        public string Keyword { get => keyword; set => keyword = value; }
        public string Blacklist { get => blacklist; set => blacklist = value; }
        public bool EncryptUsernames { get => encryptUsernames; set => encryptUsernames = value; }
        public bool HideNumbers { get => hideNumbers; set => hideNumbers = value; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConversationExporterConfiguration"/> class.
        /// </summary>
        /// <param name="inputFilePath">
        /// The input file path.
        /// </param>
        /// <param name="outputFilePath">
        /// The output file path.
        /// </param>
        /// <param name="encryptUserNames">
        /// Boolean flag indicating whether to encrypt usernames or not.
        /// </param>
        /// <param name="hideNumbers">
        /// Boolean flag indicating whether to hide numbers.
        /// </param>
        /// <param name="username">
        /// The username to filter. Can be null if not username filter is set.
        /// </param>
        /// <param name="keyword">
        /// The keyword to filter messages with. Can be null if not keyword filter is set.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any of the given arguments is <c>null</c>.
        /// </exception>
        public ConversationExporterConfiguration(string inputFilePath, string outputFilePath, bool encryptUserNames, bool hideNumbers, string username = null, string keyword = null, string blacklist = null)
        {
            // Exception handling to ensure arguments are not null.
            if (string.IsNullOrEmpty(inputFilePath) || string.IsNullOrEmpty(outputFilePath))
            {
                throw new ArgumentNullException("Arguments cannot be null or empty.");
            }

            this.InputFilePath = inputFilePath;
            this.OutputFilePath = outputFilePath;

            // Assignment of properties from variables.
            this.UserName = username;
            this.Keyword = keyword;
            this.Blacklist = blacklist;
            this.EncryptUsernames = encryptUserNames;
            this.HideNumbers = hideNumbers;
        }
    }
}
