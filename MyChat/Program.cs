using System;
using MindLink.Recruitment.MyChat;

namespace MyChat
{
    public class Program
    {
        static void Main(string[] args)
        {
            var conversationExporter = new ConversationExporter();
            var configuration = new CommandLineArgumentParser().ParseCommandLineArguments(args);

            try
            {
                conversationExporter.ExportConversation(configuration);
            }
            catch (Exception ex)
            {
                Console.WriteLine("The conversation could not be exported due to the following error: " + ex.Message );
            }
        }
    }
 
}