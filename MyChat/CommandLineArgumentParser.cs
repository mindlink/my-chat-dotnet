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
            string userFilter = ParseParameterArguments();
            string userKey = ParseParameterArguments();
            string obfuscateID = ParseParameterArguments(); 
            
            return new ConversationExporterConfiguration(arguments[0], arguments[1]);
        }
        
        private string ParseParameterArguments (string flag, string[] arguments)
        {
            int index = Array.IndexOf(arguments, flag);
            if (index + 1 <arguments.Length && index != -1)
            {
                return arguments[index +1]; 
            }
            return arguments "";    
        }
        
        private List<string> ParseParameterBlackList (string flag, string[] arguments)
        {
            var blackList = new List<string>();
            int index = Array.IndexOf(arguments, flag);
            while (index + 1 < arguments.Length && index != -1){
                
                if (arguments[index + 1] == "-user" || arguments[index + 1] == "-keyword" || arguments[index + 1] == "-list") {
                    break;
                }
                blackList.Add(arguments[index + 1]);
                index++;
            }
            
            if (blackList.Count == 0) {
                
                return null; 
            }
            
            return blackList; 
        }
    }
}
