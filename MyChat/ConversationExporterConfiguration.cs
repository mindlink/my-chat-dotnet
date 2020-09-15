namespace MindLink.Recruitment.MyChat
{
    using System;

    public sealed class ConversationExporterConfiguration
    {
        public string inputFilePath;

        public string outputFilePath;

        public ConversationExporterConfiguration(string inputFilePath, string outputFilePath)
        {
            this.inputFilePath = inputFilePath;
            this.outputFilePath = outputFilePath;
        }
    }
}
