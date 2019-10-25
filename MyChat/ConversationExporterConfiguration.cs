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
        /// The user to filter the conversation by
        /// </summary>
        public string userToFilter;

        /// <summary>
        /// The keyword to filter the conversation by
        /// </summary>
        public string keywordToFilter;

        /// <summary>
        /// Array of words that should be redacted from the 
        /// conversation.
        /// </summary>
        public string blacklistedWords;

        /// <summary>
        /// Should phone and credit card numbers be hidden? Default is false
        /// </summary>
        public bool hideNumbers;

        /// <summary>
        /// Should user IDs be obfuscated in the output? Default is false.
        /// </summary>
        public bool obfuscateUserID;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConversationExporterConfiguration"/> class.
        /// </summary>
        /// <param name="inputFilePath">
        /// The input file path.
        /// </param>
        /// <param name="outputFilePath">
        /// The output file path.
        /// </param>
        /// <param name="userToFilter">
        /// The user to filter by
        /// </param>
        /// <param name="keywordToFilter">
        /// The keyowrd to filter by
        /// </param>
        /// <param name="blacklistedWords">
        /// List of words separated by commas to redact from the conversation.
        /// </param>
        /// <param name="hideNumbers">
        /// Should credit card and phone numbers be hidden? Defaults to false.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// PROBLEM THIS EXCEPTION IS NEVER THROWN
        /// Thrown when any of the given arguments is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// PROBLEM THIS EXCEPTION IS NEVER THROWN
        /// Thrown when any of the given arguments is empty.
        /// </exception>
        public ConversationExporterConfiguration(string inputFilePath, string outputFilePath, string userToFilter = null, string keywordToFilter = null, string blacklistedWords = null, bool hideNumbers = false, bool obfuscateUserID = false)
        {
            this.inputFilePath = inputFilePath;
            this.outputFilePath = outputFilePath;
            this.userToFilter = userToFilter;
            this.keywordToFilter = keywordToFilter;
            this.blacklistedWords = blacklistedWords;
            this.hideNumbers = hideNumbers;
            this.obfuscateUserID = obfuscateUserID;
        }
    }
}
