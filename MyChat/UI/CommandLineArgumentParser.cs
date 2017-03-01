using System;
using System.IO;
using MyChat.Exporter;
using MyChat.Tools;

namespace MyChat.UI
{

    /// <summary>
    /// A class used for the interaction with the user through the command line.
    /// </summary>
    public sealed class CommandLineArgumentParser
    {

        /// <summary>
        /// Parses the arguments for the input/output file paths from the command line.
        /// </summary>
        /// <returns>
        /// A new instance of the <see cref="ConversationExporter"/>
        /// </returns>
        /// <exception cref="OutOfMemoryException">
        /// Thrown when the systems rans out of memory.
        /// </exception>
        /// <exception cref="IOException">
        /// Thrown when there is a problem with the IO.
        /// </exception>
        public ConversationExporter ParseCommandLineFilePaths()
        {
            try
            {
                Console.Write("Please specify input file path:");
                string inputFilePath = Console.ReadLine();

                Console.Write("Please specify output file path:");
                string outputFilePath = Console.ReadLine();

                if (inputFilePath.Equals("") || outputFilePath.Equals(""))
                {
                    Console.WriteLine("The file path or Output file path was not specified. The application will terminate.");
                    return null;
                }

                Console.WriteLine("Start exporting the conversation from '{0}' to '{1}' ...", inputFilePath, outputFilePath);
                return new ConversationExporter(inputFilePath, outputFilePath);
            }
            catch (OutOfMemoryException)
            {
                throw new OutOfMemoryException("The application ran out of memory while reading the input/output file paths.");
            }
            catch (IOException)
            {
                throw new IOException("Something went wrong in the IO while reading the input/output file paths.");
            }
        }

        /// <summary>
        /// Parses the arguments for the filters from the command line.
        /// </summary>
        /// <returns>
        /// A new instance of the <see cref="ConversationFilters"/> class.
        /// </returns>
        /// <exception cref="OutOfMemoryException">
        /// Thrown when the systems rans out of memory.
        /// </exception>
        /// <exception cref="IOException">
        /// Thrown when there is a problem with the IO.
        /// </exception>
        public ConversationFilters ParseCommandLineFilters()
        {
            try
            {
                Console.Write("Specify the user for which you want to filter messages (if no filter is applicable hit enter):");
                string userFilter = Console.ReadLine();

                Console.Write("Specify the keyword for which you want to filter messages (if no filter is applicable hit enter):");
                string keywordFilter = Console.ReadLine();

                Console.Write("Specify a list of hidden words (if no filter is applicable hit enter):");
                string[] hiddenWords = Console.ReadLine().Split(' ');

                return new ConversationFilters(userFilter, keywordFilter, hiddenWords);
            }
            catch (OutOfMemoryException e)
            {
                throw new OutOfMemoryException("The application ran out of memory while reading the filters.");
            }
            catch (IOException)
            {
                throw new IOException("Something went wrong in the IO while reading the filters.");
            }
        }

        /// <summary>
        /// The application entry point.
        /// </summary>
        /// <param name="args">
        /// The command line arguments.
        /// </param>
        public static void Main(string[] args)
        {
            try
            {
                CommandLineArgumentParser parser = new CommandLineArgumentParser();

                ConversationExporter exporter = parser.ParseCommandLineFilePaths();

                ConversationFilters filters = parser.ParseCommandLineFilters();
                
                if (exporter.ExportConversation(filters))
                {
                    Console.WriteLine("Conversation exported successfully");
                }
            }
            catch (OutOfMemoryException e)
            {
                Console.WriteLine("The application ran out of memory. Please restart the appliction and try again.");
                Console.WriteLine(e.InnerException.Message);
            }
            catch (IOException e)
            {
                Console.WriteLine("Something went wrong in the IO while reading the input from the user. Please restart the appliction and try again.");
                Console.WriteLine(e.InnerException.Message);
            }
        }
    }
}
