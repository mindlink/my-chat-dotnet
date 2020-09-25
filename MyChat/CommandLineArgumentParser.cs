

namespace MindLink.Recruitment.MyChat
{
    public sealed class CommandLineArgumentParser
    {
        public ConversationExporterConfiguration ParseCommandLineArguments(string[] arguments)
        {
            return new ConversationExporterConfiguration(arguments[0], arguments[1]);


        }
    }
}
