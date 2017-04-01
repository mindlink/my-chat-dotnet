using Microsoft.Extensions.CommandLineUtils;
using MindLink.MyChat.Filters;

namespace MindLink.MyChat
{
    public static class Program
    {
        /// <summary>
        /// The application entry point.
        /// </summary>
        /// <param name="args">
        /// The command line arguments.
        /// </param>
        private static void Main(string[] args)
        {
            var application = new CommandLineApplication(false);

            var inputFile = application.Argument("input", "Input file");
            var outputFile = application.Argument("output", "Output file");
            var keywordFilter = application.Option("-f | --filter <keyword>",
                "Filter messages by specified keyword", CommandOptionType.SingleValue);

            var userFilter = application.Option("-u | --user <username>",
                "Filter messages by specified username", CommandOptionType.SingleValue);

            application.HelpOption("-? | -h | --help");
            application.OnExecute(() =>
            {
                if (string.IsNullOrEmpty(inputFile.Value) || string.IsNullOrEmpty(outputFile.Value))
                {
                    application.ShowHelp();
                    return 2;
                }

                var configuration = new ConversationExporterConfiguration(inputFile.Value, outputFile.Value);

                if (keywordFilter.HasValue())
                {
                    configuration.AddFilter(new KeywordFilter(keywordFilter.Value()));
                }

                if (userFilter.HasValue())
                {
                    configuration.AddFilter(new UserFilter(userFilter.Value()));
                }

                new ConversationExporter(configuration).ExportConversation();
                return 0;
            });

            application.Execute(args);
        }
    }
}
