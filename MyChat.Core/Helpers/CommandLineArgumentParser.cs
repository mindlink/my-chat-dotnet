using MindLink.Recruitment.MyChat.Core;
using MyChat.Core.Helpers;
using System;
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
        public ConversationExporterConfiguration ParseCommandLineArguments(string[] arguments)
        {

            if (arguments.Length >= 2)
            {
                var ret = new ConversationExporterConfiguration
                {
                    inputFilePath = arguments[0],
                    outputFilePath = arguments[1],
                    Extra = new List<ExtraArgument>(),
                };


                if (arguments.Length > 2)
                    ret.Extra = parseExtraArgs(arguments.Skip(2).ToArray());
                

                return ret;
            }
            else
            {
                Logger.Log("Invalid arguments passed");
                return null;
            }

        }

        private IEnumerable<ExtraArgument> parseExtraArgs(string[] input)
        {

            List<ExtraArgument> ret = new List<ExtraArgument>();
            ExtraArgument buf = new ExtraArgument();


            foreach (string item in input)
            {                                   
                int idx = Array.IndexOf(input, item);

                if (item == null) // handle incorect input
                {
                    input[idx] = String.Empty;
                    continue;
                }

                if (item.Contains("-"))
                {
                    if (buf.Filter != ExtraArgument.FilterType.Empty)
                        ret.Add(buf);

                    buf = new ExtraArgument();

                    if (item.Equals("-u".ToLower()))
                        buf.Filter = ExtraArgument.FilterType.User;
                    
                    if(item.Equals("-k".ToLower()))
                       buf.Filter = ExtraArgument.FilterType.Keyword;

                    if(item.Equals("-h".ToLower()))
                        buf.Filter = ExtraArgument.FilterType.HideWord;

                    continue;
                 }

                if (buf.Filter == ExtraArgument.FilterType.Empty)
                    continue;

                buf.Value.Add(item);
            
            }

            if (buf.Filter != ExtraArgument.FilterType.Empty)
                ret.Add(buf);

            return ret;
        }


    }
}
