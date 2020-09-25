namespace MindLink.Recruitment.MyChat
{
    /// <summary>
    /// Represents a helper to parse command line arguments.
    /// </summary>
    public sealed class CommandLineArgumentParser
    {
        // input is the path to text file
        string input;

        // output is the name of the json file
        string output;

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

            System.Console.WriteLine("File Name to be converted for e.g. chat.txt");

            input = System.Console.ReadLine();

            while (System.IO.File.Exists(input) != true)
            {
                // loop until user enters a valid file
                System.Console.WriteLine("file name doesnt exist, enter again");
                input = System.Console.ReadLine();

            }

            System.Console.WriteLine("Name of output file");
            output = System.Console.ReadLine() + ".json";

            return new ConversationExporterConfiguration(input, output);

        }
    }
}