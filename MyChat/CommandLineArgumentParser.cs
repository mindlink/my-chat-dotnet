using System;
using System.IO;

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
            //Checks if the user gave the minimum arguments on the command line, otherwise it prompts for the path of input and output files
            if (arguments.Length < 2)
            {

                arguments = new string[2];

                do
                {
                    Console.WriteLine("Please provide a valid path for the input file");
                    arguments[0] = Console.ReadLine();

                } while (!File.Exists(arguments[0]));

                Console.WriteLine("Path for input file {0}\n", arguments[0]);

                do
                {
                    Console.WriteLine("Please provide a valid path for the output file");
                    arguments[1] = Console.ReadLine();

                } while (arguments[1].Length == 0);

                Console.WriteLine("Path for output file {0}\n", arguments[1]);
            }

            return new ConversationExporterConfiguration(arguments[0], arguments[1]);
          
        }
    }
}
