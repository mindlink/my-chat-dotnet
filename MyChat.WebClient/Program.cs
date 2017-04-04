using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;

namespace MindLink.MyChat.WebClient
{
    class Program
    {
        static void Main(string[] args)
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

                var query = new List<KeyValuePair<string, object>>();

                if (keywordFilterFlag.HasValue())
                {
                    query.Add(new KeyValuePair<string, object>("filter", keywordFilterFlag.Value()));
                }

                if (userFilterFlag.HasValue())
                {
                    query.Add(new KeyValuePair<string, object>("user", userFilterFlag.Value()));
                }

                if (blacklistFlag.HasValue())
                {
                    query.AddRange(blacklistFlag.Values.Select(value => new KeyValuePair<string, object>("blacklist", value)));
                }

                if (sensitiveFlag.HasValue())
                {
                    query.Add(new KeyValuePair<string, object>("sensitive", true));
                }

                if (obfuscateUserFlag.HasValue())
                {
                    query.Add(new KeyValuePair<string, object>("obfuscate", true));
                }

                using (var client = new HttpClient())
                {
                    var conversationContent = new StreamContent(File.OpenRead(inputFile.Value));
                    conversationContent.Headers.ContentType = MediaTypeHeaderValue.Parse("text/plain");

                    var requestContent = new MultipartFormDataContent { { conversationContent, "file1", "chat.txt" } };

                    var response = client.PostAsync(BuildUri(query), requestContent).Result;

                    response.Content.CopyToAsync(File.Create(outputFile.Value)).Wait();
                }
                return 0;
            });

            application.Execute(args);
        }

        private static string BuildUri(IEnumerable<KeyValuePair<string, object>> query)
        {
            var parameters = query.Select(p => p.Key + "=" + p.Value);
            return "http://localhost:55778/conversation/?" + string.Join("&", parameters);
        }
    }
}
