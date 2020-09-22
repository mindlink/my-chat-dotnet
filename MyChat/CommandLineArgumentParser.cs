namespace MindLink.Recruitment.MyChat
{
    /// <summary>
    /// Represents a helper to parse command line arguments.
    /// </summary>
    public static class CommandLineArgumentParser
    {

        /// <summary>
        /// Parses the given <paramref name="arguments"/> into the exporter configuration.
        /// </summary>
        /// <param name="arguments">
        /// The command line arguments.
        /// </param>
        /// <returns>
        /// A <see cref="ConversationExporterConfiguration"/> representing the command line arguments.
        /// </returns>
        public ConversationExporterConfiguration ParseCommandLineArguments(string[] arguments)
        {
            string userFilter = ParseParameterSingle();
            string userKey = ParseParameterSingle();
            string obfuscateID = ParseParameterSingle(); 
            
            return new ConversationExporterConfiguration(arguments[0], arguments[1]);
        }
        
        private string ParseParameterArguments (string[] arguments)
        {
            
            
        }
    }
}
