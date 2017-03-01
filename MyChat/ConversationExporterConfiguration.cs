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
        /// The specified user based on which messages are filtered.
        /// </summary>
        public string userMessagesFilter;

        /// <summary>
        /// The specified keyword based on which messages are filtered.
        /// </summary>
        public string keywordMessagesFilter;

        /// <summary>
        /// The set of words which are hidden from messages.
        /// </summary>
        public string[] messageHiddenWords;

        /// <summary>
        /// The replacement word for the every word in the specified set of hidden words.
        /// </summary>
        public string messageHiddenWordReplacement;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConversationExporterConfiguration"/> class.
        /// </summary>
        /// <param name="inputFilePath">
        /// The input file path.
        /// </param>
        /// <param name="outputFilePath">
        /// The output file path.
        /// </param>
        public ConversationExporterConfiguration(string inputFilePath, string outputFilePath)
        {
            this.inputFilePath = inputFilePath;
            this.outputFilePath = outputFilePath;
        }

        /// <summary>
        /// Sets the object attribute <value name="userMessagesFilter"/> to the given <paramref name="userIdMessagesFilter"/>.
        /// </summary>
        /// <param name="userMessagesFilter">
        /// The specified user based on which messages are filtered.
        /// </param>
        public void SetUserMessagesFilter (string userMessagesFilter)
        {
            this.userMessagesFilter = userMessagesFilter;
        }


        /// <summary>
        /// Sets the object attribute <value name="keywordMessagesFilter"/> to the given <paramref name="keywordMessagesFilter"/>.
        /// </summary>
        /// <param name="keywordMessagesFilter">
        /// The set of words which are hidden from messages.
        /// </param>
        public void SetKeywordMessagesFilter(string keywordMessagesFilter)
        {
            this.keywordMessagesFilter = keywordMessagesFilter;
        }

        /// <summary>
        /// Sets the object <value name="messageHiddenWords"/> to the given <paramref name="messageHiddenWords"/>.
        /// </summary>
        /// <param name="messageHiddenWords">
        /// The set of words which are hidden from messages.
        /// </param>
        public void SetMessageHiddenWords(string[] messageHiddenWords)
        {
            this.messageHiddenWords = messageHiddenWords;
            this.messageHiddenWordReplacement = "\\*redacted\\*";
        }
    }
}
