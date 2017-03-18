using System;
using System.CodeDom;
using System.Diagnostics;
using System.Linq;
using System.Net;

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
        /// Todo: Add filtering by user for messages - DONE
        /// Todo: Add filtering by keywords for messages - DONE
        /// Todo: Add filtering by blacklist for specific words - DONE
        /// </param>
        /// <returns>
        /// A <see cref="ConversationExporterConfiguration"/> representing the command line arguments.
        /// </returns>
        public ConversationExporterConfiguration ParseCommandLineArguments(string[] arguments)
        {
            try
            {
                if (arguments.Length == 0)
                {
                    throw new ArgumentNullException("arguments",
                        "You have not specified any command line arguments, Exiting...");
                }
                if (arguments.Length == 2) goto DefaultOutput;

                foreach (string arg in arguments)
                    if (arg.Contains("|"))
                    {
                        string[] includeBadWords = arg.Split('|');
                        if (includeBadWords.Length == 0 || includeBadWords.Contains(""))
                            throw new InvalidOperationException("Invalid formatting of command, Exiting...");

                   //Configuring command for sensitive information removal.. See: PrepareEvalString() method for details.
                        if (arguments.Last().Equals("-Sensitive") && includeBadWords.Length > 0)
                            return new ConversationExporterConfiguration(arguments[0], arguments[1], includeBadWords,
                                true);
                 //Configuring command for hiding each sender's ID on export. See: ObfuscateIds() and ReadConversation() methods for details.
                        if(arguments.Length.Equals(3) && arguments.Last().Equals("-Sensitive+OBF"))
                            return new ConversationExporterConfiguration(arguments[0], arguments[1], true);
                //Configuring command for bad words removal..
                        return new ConversationExporterConfiguration(arguments[0], arguments[1], includeBadWords);
                    }

                //Configuring command for filtering message per user/per keyword..
                return new ConversationExporterConfiguration(arguments[0], arguments[1], arguments[2]);
            }



            catch (InvalidOperationException errorOpException)
            {
                Console.WriteLine(errorOpException.Message);
                Console.ReadKey();

                GC.Collect();
                Environment.Exit(344);
            }

            catch (ArgumentNullException argEx)
            {
                Console.WriteLine(argEx.Message);
                Console.ReadKey();

                GC.Collect();
                Environment.Exit(345);
            }

            catch (IndexOutOfRangeException idxRangeEx)
            {
                Console.WriteLine(String.Format("You have specified an invalid number of arguments error:['{0}']",
                    idxRangeEx.Message));
                Console.WriteLine(
                    "Current format of command is: <inputfile.txt> <outputfile.json> <filteringoption> OR: <blacklistwords:word1|word2|word3...wordN>");
                Console.ReadKey();

                GC.Collect();
                Environment.Exit(346);
            }

            //Parsing only two arguments implies using input-output file only.
            //Export whole conversation.
                DefaultOutput: 
                    //Default configuration - export all content from input to output..
                    return new ConversationExporterConfiguration(arguments[0], arguments[1]);
                
        }
    }
}