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
            var exporterParameters = new ConversationExporterParameters(exporterConfiguration);

            conversationExporter.ExportConversation(exporterParameters);
        }

        /// <summary>
        /// Exports the conversation at InputFilePath as JSON to OutputFilePath, where both file paths are stored in <paramref name="exporterParameters"/>.
        /// </summary>
        /// <param name="exporterParameters">
        /// Class containing all the parameters for the exporter
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when a path is invalid.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown when something bad happens.
        /// </exception>
        public void ExportConversation(ConversationExporterParameters exporterParameters)
        {
            Conversation conversation = this.ReadConversation(exporterParameters);

            this.WriteConversation(conversation, exporterParameters.OutputFilePath);

            Console.WriteLine("Conversation exported from '{0}' to '{1}'", exporterParameters.InputFilePath, exporterParameters.OutputFilePath);
        }

        /// <summary>
        /// Helper method to read the conversation from <paramref name="inputFilePath"/>.
        /// </summary>
        /// <param name="exporterParameters">
        /// Class containing all the parameters for the exporter
        /// </param>"
        /// <returns>
        /// A <see cref="Conversation"/> model representing the conversation.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the input file could not be found.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown when something else went wrong.
        /// </exception>
        public Conversation ReadConversation(ConversationExporterParameters exporterParameters)
        {
            try
            {
                var reader = new StreamReader(new FileStream(exporterParameters.InputFilePath, FileMode.Open, FileAccess.Read),
                    Encoding.ASCII);

                string conversationName = reader.ReadLine();

                var messages = new List<Message>();

                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var split = line.Split(' ');
                    var timestamp = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(split[0]));
                    var senderId = split[1];
                    var content = string.Join(' ', split[2..split.Length]);
                    var message = ConversationModifier.ApplyMessageModifiers(timestamp, senderId, content, exporterParameters);
                    if(message != null)
                    {
                        messages.Add(message);
                    }

                }

                var activity = ConversationModifier.GenerateReport(messages, exporterParameters);
                
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
