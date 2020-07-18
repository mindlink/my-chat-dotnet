namespace MindLink.Recruitment.MyChat
{
    /// <summary>
    /// Stores conversation configuration data.
    /// </summary>
    public class ConversationConfig
    {
        /// <summary>
        /// The input file path.
        /// </summary>
        public string InputFilePath;

        /// <summary>
        /// The output file path.
        /// </summary>
        public string OutputFilePath { get; set; }

        /// <summary>
        /// The user to filter by.
        /// </summary>
        public string UserFilter { get; set; }

        /// <summary>
        /// The keyword to filter by.
        /// </summary>
        public string KeywordFilter { get; set; }

        /// <summary>
        /// The keywords to filter out.
        /// </summary>
        public string[] KeywordBlacklist { get; set; }

        /// <summary>
        /// Redacts credit card numbers if true
        /// </summary>
        public bool HideCreditCards { get; set; }

        /// <summary>
        /// Redacts phone numbers if true.
        /// </summary>
        public bool HidePhoneNumbers { get; set; }

        /// <summary>
        /// Obfuscates user ids if true
        /// </summary>
        public bool ObfuscateUserID { get; set; }
    }
}
