namespace MindLink.Recruitment.MyChat.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    /// <summary>
    /// Represents the configuration for the exporter.
    /// </summary>
    public sealed class ConversationExporterConfiguration
    {
        /// <summary>
        /// The input file path.
        /// </summary>
        public string inputFilePath {get; set;}

        /// <summary>
        /// The output file path.
        /// </summary>
        public string outputFilePath { get; set; }

        public IEnumerable<ExtraArgument> Extra { get; set; }



        public IEnumerable<ExtraArgument> getFilterArgsByType(ExtraArgument.FilterType input)
        {
            return Extra.Where(i => i.Filter == input);
        }
    }
}
