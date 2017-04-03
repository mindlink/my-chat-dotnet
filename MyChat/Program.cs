using System.IO;
using Microsoft.Extensions.CommandLineUtils;
using MindLink.MyChat.Domain;
using MindLink.MyChat.Domain.Filters;
using MindLink.MyChat.Domain.Transformers;

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
            var keywordFilterFlag = application.Option("-f | --filter <keyword>",
                "Filter messages by specified keyword", CommandOptionType.SingleValue);

            var userFilterFlag = application.Option("-u | --user <username>",
                "Filter messages by specified username", CommandOptionType.SingleValue);

            var blacklistFlag = application.Option("-b | --blacklist <keyword>",
                "Hide specified keywords from messages", CommandOptionType.MultipleValue);

            var sensitiveFlag = application.Option("-s | --sensitive",
                "Hide sensitive data from messages", CommandOptionType.NoValue);

            var obfuscateUserFlag = application.Option("-o | --obfuscate",
                "Obfuscate usernames in conversation", CommandOptionType.NoValue);

            application.HelpOption("-? | -h | --help");
            application.OnExecute(() =>
            {
                if (string.IsNullOrEmpty(inputFile.Value) || string.IsNullOrEmpty(outputFile.Value))
                {
                    application.ShowHelp();
                    return 2;
                }

                var configuration = new ConversationExporterConfiguration(File.OpenRead(inputFile.Value), File.Create(outputFile.Value));

                if (keywordFilterFlag.HasValue())
                {
                    configuration.AddFilter(new KeywordFilter(keywordFilterFlag.Value()));
                }

                if (userFilterFlag.HasValue())
                {
                    configuration.AddFilter(new UserFilter(userFilterFlag.Value()));
                }

                if (blacklistFlag.HasValue())
                {
                    configuration.AddTransformer(new BlacklistTransformer(blacklistFlag.Values));
                }

                if (sensitiveFlag.HasValue())
                {
                    configuration.AddTransformer(new CreditCardTransformer());
                    configuration.AddTransformer(new PhoneNumberTransformer());
                }

                if (obfuscateUserFlag.HasValue())
                {
                    configuration.AddTransformer(new UserObfuscateTransformer());
                }

                new ConversationExporter(configuration).ExportConversation();
                return 0;
            });

            application.Execute(args);
        }
    }
}
