namespace MindLink.Recruitment.MyChat
{
    class Program
    {
        /// <summary>
        /// The application entry point.
        /// </summary>
        /// <param name="args">
        /// The command line arguments.
        /// </param>
        static void Main(string[] args)
        {
            var parser = new CLAParser();
            var reader = new ConversationReader();
            var writer = new ConversationWriter();
            var configuration = new CLAParser().ParseCommandLineArguments(args);
            var conversation = reader.ReadConversation(configuration);
            var filter = new ConversationFilter(configuration, conversation);
            writer.WriteConversation(filter.newConversation, configuration.OutputFilePath);
        }
    }
}
