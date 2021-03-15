namespace MindLink.Recruitment.MyChat
{
    using System;

    /// <summary>
    /// Represents the configuration for the exporter.
    /// </summary>
    public sealed class ConversationExporterParameters
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
        /// The user by which to filter messages.
        /// </summary>
        public string FilterByUser { get; set; }

        /// <summary>
        /// The keyword by which to filter messages.
        /// </summary>
        public string FilterByKeyword { get; set; }

        /// <summary>
        /// The list of blacklisted words.
        /// </summary>
        public string[] Blacklist { get; set; } = null;

        public bool Report { get; set; }

        public ConversationExporterParameters(ConversationExporterConfiguration exporterConfiguration)
        {
            try
            {
                InputFilePath = exporterConfiguration.InputFilePath;
            
                if (exporterConfiguration.OutputFilePath == null)
                {
                    OutputFilePath = InputFilePath.Replace(".txt", ".json");
                }
                else
                {
                    OutputFilePath = exporterConfiguration.OutputFilePath;
                }
            }
            catch (NullReferenceException)
            {
                throw new ArgumentNullException("Input File Path");
            }

            FilterByUser = exporterConfiguration.FilterByUser;
            FilterByKeyword = exporterConfiguration.FilterByKeyword;

            if (exporterConfiguration.Blacklist != null)
            {
                Blacklist = exporterConfiguration.Blacklist.Split(',');
            }

            Report = exporterConfiguration.Report;

        }

        public ConversationExporterParameters(string inputFilePath, string outputFilePath, string filterByUser, string filterByKeyword, string[] blacklist, bool report)
        {
            InputFilePath = inputFilePath;
            OutputFilePath = outputFilePath;
            FilterByUser = filterByUser;
            FilterByKeyword = filterByKeyword;
            Blacklist = blacklist;
            Report = report;
        }
    }
}
