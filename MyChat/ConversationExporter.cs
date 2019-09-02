namespace MyChat
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Linq;
    using MindLink.Recruitment.MyChat;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a conversation exporter that can read a conversation and write it out in JSON.
    /// </summary>
    public static class ConversationExporter
    {
        /// <summary>
        /// The application entry point.
        /// </summary>
        /// <param name="arguments">
        /// The command line arguments.
        /// </param>
        static void Main(string[] args)
        {
            ExportConversation(args);
        }

        public static void ExportConversation(string[] args)
        {
            CommandLineArgumentParser.ReturnHelp(args);

            string inputFilePath = CommandLineArgumentParser.GetInputFilePath(args);
            string outputFilePath = CommandLineArgumentParser.GetOutputFilePath(args);

            string blacklistFilePath = CommandLineArgumentParser.GetBlacklistFilePath(args);
            Blacklist blacklist = BlacklistReader.TextToBlacklist(blacklistFilePath);

            Conversation conversation = ConversationReader.TextToConversation(inputFilePath);

            conversation = MessageFilter.FilterMessageByUsername(conversation, CommandLineArgumentParser.GetUsernameFilter(args));
            conversation = MessageFilter.FilterMessageByKeyword(conversation, CommandLineArgumentParser.GetKeywordFilter(args));
            conversation = MessageFilter.FilterMessageByBlacklist(conversation, blacklist);
            conversation = MessageFilter.ObfuscateIdentity(conversation, CommandLineArgumentParser.GetObfuscationState(args));

            conversation.activityReport = ActivityReporter.ActivityReport(conversation);

            ConversationWriter.ConversationToJson(conversation, outputFilePath);
            Console.WriteLine("Conversation exported from '{0}' to '{1}'", inputFilePath, outputFilePath);
        }

    }
}
