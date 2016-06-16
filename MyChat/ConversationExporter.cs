namespace MyChat
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Security;
    using System.Text;
    using MindLink.Recruitment.MyChat;
    using Newtonsoft.Json;

      
    /// <summary>
    /// Represents a conversation exporter that can read a conversation and write it out in JSON.
    /// </summary>
    public sealed class ConversationExporter
    {
        /// <summary>
        /// The application entry point.
        /// </summary>
        /// <param name="args">
        /// The command line arguments.
        /// </param>
        static void Main(string[] args)
        {
            var conversationReaderWriter = new ConversationReaderWriter(); // an instance of this class
            ConversationExporterConfiguration configuration = new CommandLineArgumentParser().ParseCommandLineArguments(args); 
            //calls the method that retrieves both command line argumetns, these two arguments are stored into two strings ()
            
            if (configuration.option == null && configuration.parameter == null)
            {
                conversationReaderWriter.ExportConversation(configuration.inputFilePath, configuration.outputFilePath); // this simply calls the relevant methods to copy a file
            }
            else if (configuration.option.Equals("--FilterBySender"))
            {
                conversationReaderWriter.ExportConversation(configuration.inputFilePath, configuration.outputFilePath, "FilterBySender", configuration.parameter);
            }
            else if (configuration.option.Equals("--FilterByKeyword"))
            {
                conversationReaderWriter.ExportConversation(configuration.inputFilePath, configuration.outputFilePath, "FilterByKeyword", configuration.parameter);
            }
            else if (configuration.option.Equals("--Blacklist"))
            {
                conversationReaderWriter.ExportConversation(configuration.inputFilePath, configuration.outputFilePath, "Blacklist", configuration.parameter);
            }
            else
            {
                 Console.WriteLine("Invalid command");
            }
                           
        }
    }
}
