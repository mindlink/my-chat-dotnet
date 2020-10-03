namespace MindLink.Recruitment.MyChat
{
    using System;

    /// <summary>
    /// Stores the argument data need to edit the output of the exported JSON
    /// </summary>
    public sealed class EditorConfiguration
    {
         /// <summary>
        /// name filter.
        /// </summary>
        public string filterByUser { get; set; }

        /// <summary>
        /// keyword filter.
        /// </summary>
        public string filterByKeyword { get; set; }

        /// <summary>
        /// blacklisted words
        /// </summary>
        public string blacklist { get; set; }
        /// <summary>
        /// switch for wether to add a report
        /// </summary>
        public bool isReportNeeded { get; set; } = false;
        public EditorConfiguration(string[] args)
        {
            for(var i = 0; i < args.Length; i++) {
                if (args[i] == "--filterByUser") {
                    if (args[i + 1].Contains("-")) {
                        throw new ArgumentException("User needs to be specified");
                    }
                    this.filterByUser = args[i + 1];
                } else if (args[i] == "--filterByKeyword") {
                    if (args[i + 1].Contains("-")) {
                        throw new ArgumentException("Keyword needs to be specified");
                    }
                    this.filterByKeyword = args[i + 1];
                } else if (args[i] == "--blacklist") {
                    if (args[i + 1].Contains("-")) {
                        throw new ArgumentException("blacklistedword(s) need to be specified");
                    }
                    this.blacklist = args[i + 1];
                } else if (args[i] == "--report") {
                    this.isReportNeeded = true;
                } 
            }
        }
    }
}