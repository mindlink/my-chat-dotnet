using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace MindLink.Recruitment.MyChat
{
    /// <summary>
    /// <para>
    /// This class represents the model of input arguments for the program, it is not static as the tool used to parse them requires an instance.
    /// </para>
    /// </summary>
    public class ProgramOptions
    {
        [Option('f', "file", Required = true, HelpText = "The input conversation")]
        public string InputFile { get; set; }

        [Option('o', "out", Required = true, HelpText = "The output path for the converted conversation")]
        public string OutputFile { get; set; }

        [Option('u', "user", Required = false, HelpText = "The specific user to filter messages for")]
        public string UserFilter { get; set; }

        [Option('k', "keyword", Required = false, HelpText = "The specific keyword to filter messages for")]
        public string KeywordFilter { get; set; }

        [OptionList('b', "blacklist", Separator = ',', Required = false, HelpText = "Specific keywords to blacklist. E.g: -b \"foo,bar\"")]
        public IEnumerable<string> BlacklistedWords { get; set; }

        [Option('h', "hide", Required=false, HelpText = "Hide Credit Card and phone numbers")]
        public bool HideCCAndPhoneNumbers { get; set; }

        [Option('m', "obfuscate", Required = false, HelpText = "Obfuscate User IDs")]
        public bool ObfuscateUserIDs { get; set; }

        [Option('r', "report", Required = false, HelpText = "Generate a report for the conversation")]
        public bool GenerateReport { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, ((HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current)));
        }
    }
}
