namespace MyChat
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Security;
    using System.Text;
    using Microsoft.Extensions.Configuration;
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
            // We use Microsoft.Extensions.Configuration.CommandLine and Configuration.Binder to read command line arguments.
            var configuration = new ConfigurationBuilder().AddCommandLine(args).Build();
            var exporterConfiguration = configuration.Get<ConversationExporterConfiguration>();

            if(Array.IndexOf(args, "--report") != -1)
            {
                exporterConfiguration.Report = true;
            }
            ManageArguments(exporterConfiguration);
        }

        /// <summary>
        /// Helper method to initialise the command line arguments and throw argument null errors.
        /// </summary>
        /// <param name="exporterConfiguration">
        /// The configuration for the conversation to be exported.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when input path is null.
        /// </exception>
        public static void ManageArguments(ConversationExporterConfiguration exporterConfiguration)
        {
            var conversationExporter = new ConversationExporter();
            String[] blacklist = null;
            if(exporterConfiguration.BlackList != null)
            {
                blacklist = exporterConfiguration.BlackList.Split(',');
            }
            if (exporterConfiguration.InputFilePath != null)
            {
                if (exporterConfiguration.OutputFilePath == null)
                {
                    exporterConfiguration.OutputFilePath = exporterConfiguration.InputFilePath.Replace(".txt", ".json");
                }
                conversationExporter.ExportConversation(exporterConfiguration.InputFilePath,
                                                            exporterConfiguration.OutputFilePath,
                                                            exporterConfiguration.FilterByUser,
                                                            exporterConfiguration.FilterByKeyword,
                                                            blacklist,
                                                            exporterConfiguration.Report);
            }
            else
            {
                throw new ArgumentNullException("Input File Path");
            }
        }

        /// <summary>
        /// Helper function to simplify testing when only the basic parameters are necessary.
        /// </summary>
        /// <param name="InputFilePath">
        /// The input file path.
        /// </param>
        /// <param name="outputFilePath">
        /// The output file path.
        /// </param>
        public void ExportConversation(string InputFilePath, string outputFilePath) 
        {
            ExportConversation(InputFilePath, outputFilePath, null, null, null, false);
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
        /// <param name="filterByUser">
        /// The userId to filter by, null is there is none.
        /// </param>
        /// <param name="filterByKeyword">
        /// The keyword to filter by, null if there is none.
        /// </param>
        /// <param name="blacklist">
        /// Lists of strings to redact from the conversation.
        /// </param>
        /// <param name="report">
        /// Boolean marker for whether to include the report after the messages.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when a path is invalid.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown when something bad happens.
        /// </exception>
        public void ExportConversation(string inputFilePath, string outputFilePath, string filterByUser, string filterByKeyword, string[] blacklist, bool report)
        {
            Conversation conversation = this.ReadConversation(inputFilePath, filterByUser, filterByKeyword, blacklist, report);

            this.WriteConversation(conversation, outputFilePath);

            Console.WriteLine("Conversation exported from '{0}' to '{1}'", inputFilePath, outputFilePath);
        }

        /// <summary>
        /// Helper method to read the conversation from <paramref name="inputFilePath"/>.
        /// </summary>
        /// <param name="inputFilePath">
        /// The input file path.
        /// </param>
        /// <param name="filterByUser">
        /// The userId to filter by, null is there is none.
        /// </param>
        /// <param name="filterByKeyword">
        /// The keyword to filter by, null if there is none.
        /// </param>
        /// <param name="blacklist">
        /// Lists of strings to redact from the conversation.
        /// </param>
        /// <param name="report">
        /// Boolean marker for whether to include the report after the messages.
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
        public Conversation ReadConversation(string inputFilePath, string filterByUser, string filterByKeyword, string[] blacklist, bool report)
        {
            try
            {
                var reader = new StreamReader(new FileStream(inputFilePath, FileMode.Open, FileAccess.Read),
                    Encoding.ASCII);

                string conversationName = reader.ReadLine();

                var messages = new List<Message>();
                var messageCount = new Dictionary<string, int>();

                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var split = line.Split(' ');
                    var senderId = split[1];
                    var content = string.Join(' ', split[2..split.Length]);

                    if (messageCount.ContainsKey(senderId))
                    {
                        messageCount[senderId]++;
                    }
                    else
                    {
                        messageCount[senderId] = 1;
                    }
                    if(!IsInFilters(senderId, content, filterByUser, filterByKeyword))
                    {
                        continue;
                    }
                    content = ApplyBlacklist(content, blacklist);

                    messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(split[0])), senderId, content));
                }

                var activity = GenerateReport(messageCount, report);
                
                return new Conversation(conversationName, messages, activity);
            }
            catch (FormatException)
            {
                throw new FormatException("The file could not be converted.");
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
        /// Helper function to generate a report of activity as a list.
        /// </summary>
        /// <param name="messageCount">
        /// Dictionary holding a tally of number of messages sent by each sender.
        /// </param>
        /// <param name="report">
        /// Boolean marker for whether to include the report after the messages. 
        /// </param>
        /// <returns>
        /// A list of message counts for each sender sorted in descending order.
        /// </returns>
        private List<Activity> GenerateReport(Dictionary<string, int> messageCount, bool report)
        {
            var activity = new List<Activity>();
            if (report)
            {
                foreach (KeyValuePair<string, int> entry in messageCount)
                {
                    activity.Add(new Activity(entry.Key, entry.Value));
                }
                activity.Sort((x, y) => -x.count.CompareTo(y.count));
            }
            else
            {
                activity = null;
            }
            return activity;
        }

        /// <summary>
        /// Helper function which applies redaction on the <paramref name="content"/> using the <paramref name="blacklist"/>
        /// </summary>
        /// <param name="content">
        /// Message string on which to apply redactions.
        /// </param>
        /// <param name="blacklist">
        /// Array of strings to be replaced.
        /// </param>
        /// <returns>
        /// String <paramref name="content"/> with blacklisted words replaced with "\\*redacted\\*"
        /// </returns>
        private string ApplyBlacklist(string content, string[] blacklist)
        {
            if(blacklist != null)
            {
                foreach (string redaction in blacklist)
                {
                    content = content.Replace(redaction, @"\*redacted\*", true, null);
                }
            }
            return content;
        }

        /// <summary>
        /// Helper function to check whether given conversation entry is within given filters.
        /// </summary>
        /// <param name="senderId">
        /// String username of the message sender.
        /// </param>
        /// <param name="content">
        /// String contents of the message.
        /// </param>
        /// <param name="filterByUser">
        /// SenderId by which to filter the conversation.
        /// </param>
        /// <param name="filterByKeyword">
        /// Keyword by which to filter the conversation.
        /// </param>
        /// <returns>
        /// Returns true iff message passes all filters, false if not. 
        /// </returns>
        public bool IsInFilters(string senderId, string content, string filterByUser, string filterByKeyword)
        {

            if (filterByUser != null && senderId != filterByUser)
            {
                return false;
            }
            if (filterByKeyword != null && !content.Contains(filterByKeyword))
            {
                return false;
            }
            return true;
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
        public void WriteConversation(Conversation conversation, string outputFilePath)
        {
            try
            {
                var writer = new StreamWriter(new FileStream(outputFilePath, FileMode.Create, FileAccess.ReadWrite));

                var serialized = JsonConvert.SerializeObject(conversation, Formatting.Indented, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

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
