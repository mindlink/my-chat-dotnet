namespace MindLink.Recruitment.MyChat
{
    using System;

    public sealed class ConversationExporterConfiguration
    {
        public string inputFilePath { get; set; }

        public string outputFilePath { get; set; }
        
        public string UserToFilter { get; set; }
        
        public string KeywordToFilter { get; set; }
        
        public string BannedTerm { get; set; }

    }
}
