
using CommandLine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


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
        /// 

        public String inputPath;
        public String outputPath;
        public String user;
        public String keyword;
        public List<string> blacklist;
        public bool hideSensitiveData;
        public bool obfustaceUserIDs;

        public class Options
        {
            [Option('i', "input", Required = true, HelpText = "Set input path")]
            public String Input { get; set; }

            [Option('o', "output", Required = true, HelpText = "Set output path")]
            public String Output { get; set; }

            [Option('u', "user", Required = false, HelpText = "Set user")]
            public String  User { get; set; }

            [Option('k', "keyword", Required = false, HelpText = "Set keyword")]
            public String Keyword { get; set; }

            [Option('b', "blacklist", Required = false, HelpText = "Set keyword")]
            public IEnumerable<String> Blacklist { get; set; }

            [Option('h', "hide", Required = false, HelpText = "Option to hide phone and credit card numbers")]
            public bool HideSensitiveData { get; set; }

            [Option('f', "obfuscate", Required = false, HelpText = "Option to obfuscate user IDs")]
            public bool ObfuscateUserIDs { get; set; }


        }

        public ConversationExporterConfiguration ParseCommandLineArguments(string[] arguments)
        {
            Parser.Default.ParseArguments<Options>(arguments)
                .WithParsed<Options>(o =>
                {
                    inputPath = o.Input;
                    outputPath = o.Output;
                    user = o.User != null ? o.User : null;
                    keyword = o.Keyword != null ? o.Keyword : null;
                    blacklist = o.Blacklist != null ? o.Blacklist.ToList() : null;
                    hideSensitiveData = o.HideSensitiveData;
                    obfustaceUserIDs = o.ObfuscateUserIDs;
                    
            });

            ConversationExporterConfiguration configuration = new ConversationExporterConfiguration(inputPath, outputPath);

            configuration.user = user;
            configuration.keyword = keyword;
            configuration.blacklist = blacklist;
            configuration.hideSensitiveData = hideSensitiveData;
            configuration.obfuscateUserIDs = obfustaceUserIDs;

            Console.WriteLine(configuration.inputFilePath);
            Console.WriteLine(configuration.outputFilePath);
            Console.WriteLine(configuration.user);
            Console.WriteLine(configuration.keyword);
            Console.WriteLine(configuration.hideSensitiveData);
            Console.WriteLine(configuration.obfuscateUserIDs);


            //configuration.blacklist.ForEach(x => Console.Write(x));
            Console.ReadLine();

            return configuration;
        }
    }
}
