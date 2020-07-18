namespace MindLink.Recruitment.MyChat
{
    /// <summary>
    /// Controller for conversation export operations.
    /// </summary>
    class ExportController
    {
        private IConversationReader reader;
        private IConversationWriter writer;
        private IConversationFilter filter;
        private ICommandLineParser cmdParser;
        private ConversationConfig config;

        /// <summary>
        /// Initialises a new instance of the <see cref="ExportController"/> class.
        /// </summary>
        /// <param name="reader"></param>
        /// Reference to the conversation reader.
        /// <param name="writer"></param>
        /// Reference to the conversation writer.
        /// /// <param name="filter"></param>
        /// Reference to the conversation filter.
        /// <param name="cmdParser"></param>
        /// Reference to the command line argument parser.
        public ExportController(IConversationReader reader, IConversationWriter writer, IConversationFilter filter, ICommandLineParser cmdParser)
        {
            this.reader = reader;
            this.writer = writer;
            this.filter = filter;
            this.cmdParser = cmdParser;
        }

        /// <summary>
        /// Exports the conversation at <paramref name="args[0]"/> as JSON to <paramref name="args[1]"/> Additional args are processed as conversation filters.
        /// </summary>
        /// <param name="args"></param>
        public void Export(string[] args)
        {
            config = cmdParser.ParseCommandLineArguments(args);
            Conversation conversation = reader.ReadConversation(config);
            conversation = filter.FilterConversation(config, conversation);
            writer.WriteConversation(conversation, config.OutputFilePath);            
        }
    }
}