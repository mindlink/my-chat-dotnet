namespace MindLink.Recruitment.MyChat
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Represents the configuration for the exporter.
    /// </summary>
    public class ConversationExporterConfiguration
    {
        /// <summary>
        /// The input file path.
        /// </summary>
        public string InputFilePath { get; set; }

        /// <summary>
        /// The output file path.
        /// </summary>
        public string OutputFilePath { get; set; }

        /// <summary>
        /// The filter word.
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// Filter by name switch.
        /// </summary>
        public bool FilterID { get; set; }

        /// <summary>
        /// Filter by blacklist switch.
        /// </summary>
        public bool Blacklist { get; set; }

        /// <summary>
        /// Check personal information.
        /// </summary>
        public bool PersonalNumbers { get; set; }

        /// <summary>
        /// A list of blacklisted words.
        /// </summary>
        public List<String> BlacklistWords;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConversationExporterConfiguration"/> class.
        /// </summary>
        public ConversationExporterConfiguration()
        {
            BlacklistWords = new List<String>();
        }
    }
}
