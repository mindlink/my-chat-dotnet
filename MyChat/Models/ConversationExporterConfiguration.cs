using System;

namespace MindLink.Recruitment.MyChat
{

    public sealed class ConversationExporterConfiguration
    {
        public  string InputFilePath { get; } 
        public string OutputFilePath { get; }
        public string FilterType { get; private set; }

        public ConversationExporterConfiguration(string inputFilePath, string outputFilePath)
        {
            InputFilePath = inputFilePath;
            OutputFilePath = outputFilePath;
            AskUserForFilterType();
        }

        public void AskUserForFilterType()
        {
            Console.WriteLine("Please Specify bellow if you want to filter the messages: \n 0 - No Filtering \n 1 - Filter by User ID \n 2 - Filter by Keyword ");
            string type = Console.ReadLine();
            switch (type[0])
            {
                case '0':
                    System.Console.WriteLine("The Messages will not be Filtered");
                    FilterType = "NoFiltering";
                    break;
                case '1':
                    System.Console.WriteLine("The Messages will be Filtered by UserID");
                    FilterType = "IdFiltering";
                    break;
                case '2':
                    System.Console.WriteLine("The Messages will be Filtered by a Keyword");
                    FilterType = "KeywordFiltering";
                    break;
                default:
                    System.Console.WriteLine("You pressed the wrong key, Please Try Again");
                    AskUserForFilterType();
                    break;
            }
        }
    }
}
