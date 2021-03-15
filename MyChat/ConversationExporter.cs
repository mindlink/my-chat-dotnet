﻿namespace MyChat
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Security;
    using System.Text;
    using Microsoft.Extensions.Configuration;
    using MindLink.Recruitment.MyChat;
    using MindLink.Recruitment.MyChat.Blacklisting;
    using MindLink.Recruitment.MyChat.Exceptions.CustomIOExceptions;
    using MindLink.Recruitment.MyChat.Filters;
    using MindLink.Recruitment.MyChat.Text_Processing;
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
            // We use Microsoft.Extensions.Configuration.CommandLine and Configuration.Binder to read command line arguments.
            var configuration = new ConfigurationBuilder().AddCommandLine(args).Build();
            var exporterConfiguration = configuration.Get<ConversationExporterConfiguration>();
            var conversationExporter = new ConversationExporter();

            foreach (var arg in args)
            {
                if (arg.Equals("--report"))
                {
                    exporterConfiguration.generateReport = true;
                    break;
                }
            }

            conversationExporter.ExportConversation(exporterConfiguration);
        }

        /// <summary>
        /// Exports the conversation at <paramref name="inputFilePath"/> as JSON to <paramref name="outputFilePath"/>.
        /// </summary>
        /// <param name="inputFilePath">
        /// The input file path.
        /// </param>
        /// <param name="outputFilePath">
        /// The output file path.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when a path is invalid.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown when something bad happens.
        /// </exception>
       
        public void ExportConversation(ConversationExporterConfiguration exporterConfiguration)
        {
            var filters = new List<BaseFilter>();
            var blacklists = new List<IBlacklist>();
            Conversation conversation;

            if (exporterConfiguration.filterByUser != null)
            {
                var userFilter = new UserFilter(new string[] { exporterConfiguration.filterByUser });
                filters.Add(userFilter);
            }

            if (exporterConfiguration.filterByKeyword != null)
            {
                var keywordFilter = new KeywordFilter(new string[] { exporterConfiguration.filterByKeyword });
                filters.Add(keywordFilter);
            }

            if (exporterConfiguration.blacklist != null)
            {
                var blacklistWords = exporterConfiguration.blacklist.Split(',');
                blacklists.Add(new KeywordBlacklist(blacklistWords, "*redacted*"));

            }

            if (exporterConfiguration.generateReport)
                conversation = this.ReadConversation(exporterConfiguration.InputFilePath, filters.ToArray() , blacklists.ToArray(),true);
            else
                conversation = this.ReadConversation(exporterConfiguration.InputFilePath, filters.ToArray(), blacklists.ToArray());

            this.WriteConversation(conversation, exporterConfiguration.OutputFilePath);

            Console.WriteLine("Conversation exported from '{0}' to '{1}'", exporterConfiguration.InputFilePath, exporterConfiguration.OutputFilePath);
        }

        /// <summary>
        /// Helper method to read the conversation from <paramref name="inputFilePath"/>.
        /// </summary>
        /// <param name="inputFilePath">
        /// The input file path.
        /// </param>
        /// <returns>
        /// A <see cref="Conversation"/> model representing the conversation.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the input file could not be found.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown when something else went wrong.
        /// </exception>
        /// 






        public Conversation ReadConversation(string inputFilePath, BaseFilter[] filters, IBlacklist[] blacklists, bool generateReport = false)
        {
            try
            {
                var reader = new StreamReader(new FileStream(inputFilePath, FileMode.Open, FileAccess.Read),
                    Encoding.ASCII);
                var messages = new List<Message>();
                var conversationName = reader.ReadLine();


                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var split = line.Split(' ');
                    var messageInfo = new string[3];
                    var processingParser = new ProcessingParser(filters, blacklists);

                    if (split[1].Length <= 0)
                        throw new InvalidUserIdException("Invalid User ID, Check input");

                    if (split.Length <= 2)
                        throw new MessageFormatException("Empty or Incorrectly formatted Message, Check input");

                    messageInfo[0] = split[0];
                    messageInfo[1] = split[1];
                    messageInfo[2] = line.Substring(split[0].Length + split[1].Length + 1).Trim();
                    messageInfo = processingParser.RunLineCheck(messageInfo[0], messageInfo[1], messageInfo[2]);

                    //if timestamp is empty, then message didn't pass through filter or blacklist
                    if (messageInfo[0] != "")
                    {
                        messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(messageInfo[0])), messageInfo[1], messageInfo[2]));
                    }
                }

                if (generateReport)
                {
                    var reported = new ConversationReport(conversationName, messages);

                    reported.GenerateReportData();
                    reported.SortDataAscending();
                    return reported;
                }
                else
                {
                    return new Conversation(conversationName, messages);
                }
            }
            catch (FormatException)
            {
                throw new Exception("Incorrect Unix Format, Please Check input");
            }
            catch (FileNotFoundException)
            {
                throw new ArgumentException("The file was not found.");
            }
            catch (IOException)
            {
                throw new Exception("Something went wrong in the IO.");
            }
        }

        /// <summary>
        /// Helper method to write the <paramref name="conversation"/> as JSON to <paramref name="outputFilePath"/>.
        /// </summary>
        /// <param name="conversation">
        /// The conversation.
        /// </param>
        /// <param name="outputFilePath">
        /// The output file path.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when there is a problem with the <paramref name="outputFilePath"/>.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown when something else bad happens.
        /// </exception>
        /// 







        public void WriteConversation(Conversation conversation, string outputFilePath)
        {
            try
            {
                var writer = new StreamWriter(new FileStream(outputFilePath, FileMode.Create, FileAccess.ReadWrite));

                var serialized = JsonConvert.SerializeObject(conversation, Formatting.Indented);

                writer.Write(serialized);

                writer.Flush();

                writer.Close();
            }
            catch (SecurityException)
            {
                throw new ArgumentException("No permission to file.");
            }
            catch (DirectoryNotFoundException)
            {
                throw new ArgumentException("Path invalid.");
            }
            catch (IOException)
            {
                throw new Exception("Something went wrong in the IO.");
            }
        }
    }
}
