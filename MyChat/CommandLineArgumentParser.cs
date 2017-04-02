using CommandLine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MindLink.Recruitment.MyChat
{
    /// <summary>
    /// Represents a helper to parse command line arguments.
    /// </summary>
    public class CommandLineArgumentParser
    {
        /// <summary>
        /// required option for setting the input file
        /// </summary>
        [Option('i', "input", Required = true, HelpText = "Input file to read.")]
        public string InputFile { get; set; }

        /// <summary>
        /// required option for setting the output file
        /// </summary>
        [Option('o', "output", Required = true, HelpText = "Input file to write.")]
        public string OutputFile { get; set; }

        /// <summary>
        /// option for setting the user name filter
        /// </summary>
        [Option('f', "filter by user", Required = false, HelpText = "Filter export by user")]
        public string User { get; set; }

        /// <summary>
        /// option for setting the keyword filter
        /// </summary>
        [Option('k', "filter by keyword", Required = false, HelpText = "Filter export by keyword")]
        public string Keyword { get; set; }

        /// <summary>
        /// option for setting the blacklisted words
        /// </summary>
        [OptionList('L', "black List", Separator = ',',Required = false, HelpText = "Redact word list")]
        public List<string> blackList { get; set; }

        /// <summary>
        /// option for setting the export flag
        /// </summary>
        [Option('r', "report", Required = false, HelpText = "Include a Report with user activity")]
        public bool IncludeReport { get; set; }

        /// <summary>
        /// option for setting the ''hide user name'' flag
        /// </summary>
        [Option('h', "Hide user Id's", HelpText = "Hide user Id's at the output file")]
        public bool HideUserId { get; set; }

        /// <summary>
        /// A guidance for proper user of the program
        /// </summary>
        [HelpOption]
        public string GetUsage()
        {

             var usage = new StringBuilder();
             usage.AppendLine("Conversation Exporter 1.0");
             usage.AppendLine("-i input,the input file");
             usage.AppendLine("-o output, the output file");
             usage.AppendLine("-f user , filter by user ");
             usage.AppendLine("-k keyword, filter by keyword");
             usage.AppendLine("-L word1,word2,...,wordn  -- set black list");
             usage.AppendLine("-r include report");
             usage.AppendLine("-h hide user id");
 
             return usage.ToString();
         }
    }
}
