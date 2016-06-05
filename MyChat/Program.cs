namespace MyChat
{
    using MindLink.Recruitment.MyChat;
    using System;
    public sealed class Program
    {
        static void Main(string[] args)
        {
            var parsedArguments = new ProgramOptions();
            var result = CommandLine.Parser.Default.ParseArguments(args, parsedArguments);
            if (!result)
            {
                return;
            }

            ConversationExporter exporter = new ConversationExporter();
            if (exporter.ExportConversation(parsedArguments))
            {
                Console.WriteLine($"Successfully converted {parsedArguments.InputFile} and wrote it out to {parsedArguments.OutputFile}");
            }
            else
            {
                Console.WriteLine("There was an error and the conversation could not converted.");
            }
        }
    }
}
