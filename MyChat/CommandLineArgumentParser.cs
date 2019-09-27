
using CommandLine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

        public ConversationExporterConfiguration configuration;

        public class Options
        {
            [Option('i', "input", Required = true, HelpText = "Required for the program to work. Set path for the input file")]
            public String Input { get; set; }

            [Option('o', "output", Required = true, HelpText = "Required for the program to work. If the file does not exist, it will be created at that directory.")]
            public String Output { get; set; }

            [Option('u', "user", Required = false, HelpText = "Filter messages by specifying a sender ID. Only messages sent by that user are added to the output file.")]
            public String  User { get; set; }

            [Option('k', "keyword", Required = false, HelpText = "Filter messages by specifying a keyword. Only messages containing that keyword are added to the output file.")]
            public String Keyword { get; set; }

            [Option('b', "blacklist", Required = false, HelpText = @"Enter one or more words, separated by space. They will be replaced by \*redacted *\ in the messages that contain those words.")]
            public IEnumerable<String> Blacklist { get; set; }

            [Option('h', "hide", Required = false, HelpText = "Hide phone and credit card numbers.")]
            public bool HideSensitiveData { get; set; }

            [Option('f', "obfuscate", Required = false, HelpText = "Obfuscate user IDs. In the report, they will be listed as user1, user2, user3 etc.")]
            public bool ObfuscateUserIDs { get; set; }

        }

        public ConversationExporterConfiguration ParseCommandLineArguments(string[] arguments)
        {

            if (arguments.Length != 0)
            {

                Parser.Default.ParseArguments<Options>(arguments)
                    .WithParsed<Options>(o =>
                    {
                        inputPath = o.Input;
                        outputPath = o.Output;
                   
                        if (!File.Exists(inputPath)) { throw new FileNotFoundException(Globals.EXCEPTION_FILE_NOT_FOUND); }
                        if (!Directory.Exists(Path.GetDirectoryName(outputPath))) { throw new DirectoryNotFoundException(Globals.EXCEPTION_DIRECTORY_NOT_FOUND); }
                     
                        configuration = new ConversationExporterConfiguration(inputPath, outputPath);

                        configuration.user = o.User != null ? o.User : null;
                        configuration.keyword = o.Keyword != null ? o.Keyword : null;
                        configuration.blacklist = o.Blacklist != null ? o.Blacklist.ToList() : null;
                        configuration.hideSensitiveData = o.HideSensitiveData;
                        configuration.obfuscateUserIDs = o.ObfuscateUserIDs;

                    });

            }

            else
            {
                throw new ArgumentException(Globals.EXCEPTION_ARGUMENT_NOT_FOUND);
            }
            
            return configuration;
        }
    }
}