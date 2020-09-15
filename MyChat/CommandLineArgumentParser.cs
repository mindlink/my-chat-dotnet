using System;

namespace MindLink.Recruitment.MyChat
{
    
    public sealed class CommandLineArgumentParser
    {
        public ConversationExporterConfiguration ParseCommandLineArguments(string[] arguments)
        {
            if (arguments.Length != 2)
            {
                throw new ArgumentException($"You provided {arguments.Length} arguments when this tool needs 2");
            }
            
            return new ConversationExporterConfiguration(arguments[0], arguments[1]);

        }
    }
}
