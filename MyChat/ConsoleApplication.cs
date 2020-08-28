namespace MindLink.Recruitment.MyChat
{
    using System;

    public sealed class ConsoleApplication
    {
        /// <summary>
        /// The application entry point.
        /// </summary>
        /// <param name="args">
        /// The command line arguments.
        /// </param>
        static void Main(string[] args)
        {
            ConversationExporter conversationExporter = new ConversationExporter();
            ConversationFilter conversationFilter = new ConversationFilter();
            ConversationCensor conversationCensor = new ConversationCensor();
            ConversationExporterConfiguration configuration;

            Console.WriteLine("Welcome to MyChat\n");
            Console.WriteLine("Set Load/Save Filepath? (select 'No' to load default settings)");
            Console.WriteLine("Y = Yes \nN = No\n");

            string configPath = Console.ReadLine().Trim();

            if (configPath.Equals("yes", StringComparison.OrdinalIgnoreCase) || configPath.Equals("y", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Enter filepath e.g. C:\\Users\\Robert Woodhouse\\Documents\\chat.txt");
                
                string inputFilePath = Console.ReadLine();
                
                Console.WriteLine("\nSet file destination e.g. C:\\Users\\Robert Woodhouse\\Documents\\chat.json");
                
                string outputFilePath = Console.ReadLine();

                configuration = new CommandLineArgumentParser().ParseCommandLineArguments(new string[] { inputFilePath, outputFilePath });
            }
            else
            {
                configuration = new CommandLineArgumentParser().ParseCommandLineArguments(new string[] { "chat.txt", "chat.json" });
            }

            Console.WriteLine("\nSelect from list of filters to apply to the conversation\n");
            Console.WriteLine("Censor Card Numbers?");
            Console.WriteLine("Y = Yes \nN = No\n");

            string censorCard = Console.ReadLine().Trim();

            Console.WriteLine("\nCensor Phone Numbers? (UK numbers only)");
            Console.WriteLine("Y = Yes \nN = No\n");

            string censorPhone = Console.ReadLine().Trim();

            Console.WriteLine("\nObfuscate UserID?");
            Console.WriteLine("Y = Yes \nN = No\n");

            string obfuscateUser = Console.ReadLine().Trim();

            Console.WriteLine("\nEnter number for export type:");
            Console.WriteLine("1. Filter User \n2. Filter Keyword \n3. Blacklist Word\n0. No Filter\n");

            int filterMethod;

            var isNum = int.TryParse(Console.ReadLine(), out filterMethod);

            if(isNum)
            {
                string filterWord = "";
                if (filterMethod >= 1 && filterMethod <= 3)
                {
                    if (filterMethod.Equals(1)) Console.WriteLine("\nEnter UserID value: \n");
                    if (filterMethod.Equals(2)) Console.WriteLine("\nEnter Keyword value: \n");
                    if (filterMethod.Equals(3)) Console.WriteLine("\nEnter Blacklisted word: \n");
                    filterWord = Console.ReadLine().Trim();
                }

                switch (filterMethod)
                {
                    case 1:
                        conversationExporter.ExportConversation(configuration.inputFilePath, configuration.outputFilePath, filterWord, censorCard, censorPhone, obfuscateUser, conversationFilter.FilterUser);
                        break;

                    case 2:
                        conversationExporter.ExportConversation(configuration.inputFilePath, configuration.outputFilePath, filterWord, censorCard, censorPhone, obfuscateUser, conversationFilter.FilterKeyword);
                        break;

                    case 3:
                        conversationExporter.ExportConversation(configuration.inputFilePath, configuration.outputFilePath, filterWord, censorCard, censorPhone, obfuscateUser, conversationCensor.BlacklistWord);
                        break;
                    default:
                        conversationExporter.ExportConversation(configuration.inputFilePath, configuration.outputFilePath, censorCard, censorPhone, obfuscateUser);
                        break;
                }
            }
            else
            {
                Console.WriteLine("Default export selected");
                conversationExporter.ExportConversation(configuration.inputFilePath, configuration.outputFilePath);
            }
        }
    }
}
