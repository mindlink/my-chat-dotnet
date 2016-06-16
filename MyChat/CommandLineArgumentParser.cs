namespace MindLink.Recruitment.MyChat
{
    
    /// <summary>
    /// Represents a helper to parse command line arguments.
    /// </summary>
    public sealed class CommandLineArgumentParser
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
            if (arguments.Length == 2 )
            {
                return new ConversationExporterConfiguration(arguments[0], arguments[1]);
            }
            else if (arguments.Length == 4 ) 
            {
                return new ConversationExporterConfiguration(arguments[0], arguments[1], arguments[2], arguments[3]);
            }
            else if (arguments.Length > 4) // if more than four arguments are present then these are the black listed words
            {
                for (int i = 3; i < arguments.Length; i++ )
                {
                    arguments[3] = arguments[3] + " " + arguments[i];                    
                }
                
                return new ConversationExporterConfiguration(arguments[0], arguments[1], arguments[2], arguments[3]);
            }
            else
            {
                return null;// Invalid number of arguments
            }
                                               
        }


        
    }
}
