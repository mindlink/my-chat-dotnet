using MyChat;
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
        /// The name of the conversation.
        /// </summary>
        public ConversationExporterConfiguration configuration;


        /// <summary>
        /// Parses the given <paramref name="arguments"/> into the exporter configuration.
        /// </summary>
        /// <param name="arguments">
        /// The command line arguments.
        /// </param>
        /// <exception cref="NullReferenceException">
        /// Thrown when the <paramref name="arguments"/> is <c>null</c>.
        /// </exception>
        public void ParseCommandLineArguments(string[] arguments)
        {
            try
            {
                this.configuration = new ConversationExporterConfiguration(arguments[0], arguments[1]);
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException("The Input and Output file arguments were not specified.");
            }
        }

        /// <summary>
        /// Parses the given <paramref name="arguments"/> into the exporter configuration.
        /// </summary>
        /// <param name="arguments">
        /// The command line arguments.
        /// </param>
        /// <exception cref="NullReferenceException">
        /// Thrown when any of the given arguments is <c>null</c>.
        /// </exception>
        public void ParseCommandLineUserFilter(string[] arguments)
        {
            try
            {
                this.configuration.SetUserMessagesFilter(arguments[0]);
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException("The user filter argument was not specified.");
            }
        }

        /// <summary>
        /// Parses the given <paramref name="arguments"/> into the exporter configuration.
        /// </summary>
        /// <param name="arguments">
        /// The command line arguments.
        /// </param>
        /// <exception cref="NullReferenceException">
        /// Thrown when any of the given arguments is <c>null</c>.
        /// </exception>
        public void ParseCommandLineKeywordFilter(string[] arguments)
        {
            try
            {
                this.configuration.SetKeywordMessagesFilter(arguments[0]);
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException("The keyword filter argument was not specified.");
            }
        }

        /// <summary>
        /// Parses the given <paramref name="hiddenWords"/> into the exporter configuration.
        /// </summary>
        /// <param name="hiddenWords">
        /// The command line arguments.
        /// </param>
        /// <exception cref="NullReferenceException">
        /// Thrown when any of the given arguments is <c>null</c>.
        /// </exception>
        public void ParseCommandLineHiddenWordsFilter(string[] hiddenWords)
        {
            try
            {
                this.configuration.SetMessageHiddenWords(hiddenWords);
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException("The hidden words was not specified.");
            }
        
        }

        /// <summary>
        /// The application entry point.
        /// </summary>
        /// <param name="args">
        /// The command line arguments.
        /// </param>
        static void Main(string[] args)
        {
            try
            {
                string[] arguments = new string[2];
                CommandLineArgumentParser parser = new CommandLineArgumentParser();

                Console.Write("Please specify input file path:");
                arguments[0] = Console.ReadLine();

                Console.Write("Please specify output file path:");
                arguments[1] = Console.ReadLine();

                parser.ParseCommandLineArguments(arguments);

                Console.Write("Specify the user for which you want to filter messages (if no filter is applicable hit enter):");
                parser.ParseCommandLineUserFilter(Console.ReadLine().Split(' '));

                Console.Write("Specify the keyword for which you want to filter messages (if no filter is applicable hit enter):");
                parser.ParseCommandLineKeywordFilter(Console.ReadLine().Split(' '));

                Console.Write("Specify a list of hidden words (if no filter is applicable hit enter):");
                parser.ParseCommandLineHiddenWordsFilter(Console.ReadLine().Split(' '));

                new ConversationExporter().ExportConversation(parser.configuration);
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (IOException)
            {
                Console.WriteLine("Something went wrong with the IO.");
            }
        }
    }
}
