using MindLink.Recruitment.MyChat.Interfaces.ControllerInterfaces;
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
        public ConversationExporterConfiguration ParseCommandLineArguments(string[] arguments, IFilterController filterController)
        {
            try
            {
                if (arguments.Length > 2)
                {
                    // INITIALISE an array of string, max length 18, gives some over head 
                    // for each filter to have an argument passed for it
                    string[] filters = new string[18];
                    // CALL arguments CopyTo method, copying to the array for filters, 
                    // starting at index 2 (this is any string after the output file)
                    arguments.CopyTo(filters, 2);
                    // CALL to filterController CheckForFilters method, to let it handle the filters
                    filterController.CheckForFilters(filters);

                }

                return new ConversationExporterConfiguration(arguments[0], arguments[1]);
            }
            catch (IndexOutOfRangeException inner)
            {
                throw new IndexOutOfRangeException("No output file specified/Not enough arguments passed", inner);
            }
            catch (ArgumentNullException inner)
            {
                throw new ArgumentNullException("No input or output paths were specified", inner);
            }
        }
    }
}
