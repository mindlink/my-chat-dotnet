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
            var filter = new ConversationFilter();

            var configuration = new CLAParser().ParseCommandLineArguments(args);
            var conversation = reader.ReadConversation(configuration);

            conversation = filter.FilterParser(configuration, conversation);
            writer.WriteConversation(conversation, configuration.OutputFilePath);
        }
    }
}