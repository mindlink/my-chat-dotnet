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
        /// <summary>
        /// The message content.
        /// </summary>
        public string namefilter{ get; }

        public string keywordfilter{ get; }

        public string[] blacklisted{ get; }
        public ConversationExporter(string filterByUser, string filterByKeyword, string blacklist)
        {
            this.namefilter = filterByUser;
            this.keywordfilter = filterByKeyword;
            this.blacklisted = blacklist.Split(',');
            Console.WriteLine("blacklist = {0}", this.blacklisted[0]);
        }
        static void Main(string[] args)
        {
            // We use Microsoft.Extensions.Configuration.CommandLine and Configuration.Binder to read command line arguments.
            var configuration = new ConfigurationBuilder().AddCommandLine(args).Build();
            var exporterConfiguration = configuration.Get<ConversationExporterConfiguration>();

            var conversationExporter = new ConversationExporter(exporterConfiguration.filterByUser, exporterConfiguration.filterByKeyword, exporterConfiguration.blacklist);
            conversationExporter.ExportConversation(exporterConfiguration.InputFilePath, exporterConfiguration.OutputFilePath);
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
        public void ExportConversation(string inputFilePath, string outputFilePath)
        {
            Conversation conversation = this.ReadConversation(inputFilePath);

            this.EditConversation(conversation);

            this.WriteConversation(conversation, outputFilePath);

            Console.WriteLine("Conversation exported from '{0}' to '{1}'", inputFilePath, outputFilePath);
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
        public Conversation ReadConversation(string inputFilePath)
        {
            try
            {
                var reader = new StreamReader(new FileStream(inputFilePath, FileMode.Open, FileAccess.Read),
                    Encoding.ASCII);

                string conversationName = reader.ReadLine();
                var messages = new List<Message>();

                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var split = line.Split(' ');

                    var message = new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(split[0])), split[1], string.Join(" ",split[2..]));
                    messages.Add(message);
                }

                return new Conversation(conversationName, messages);
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

        public void EditConversation(Conversation conversation)
        {
            this.FilterConversationByUsername(conversation);
            this.FilterConversationByKeyword(conversation);
        }

        public void FilterConversationByUsername(Conversation conversation)
        {
            if (this.namefilter != null)
            {
                var editedMessages = new List<Message>();
                foreach(Message message in conversation.messages)
                {
                    if (this.namefilter == message.senderId)
                    {
                        editedMessages.Add(message);
                    }
                };
                conversation.messages = editedMessages;
            }
        }

        public void FilterConversationByKeyword(Conversation conversation)
        {
            if (this.keywordfilter != null)
            {
                var editedMessages = new List<Message>();
                foreach(Message message in conversation.messages)
                {
                    if (message.content.Contains(this.keywordfilter))
                    {
                        editedMessages.Add(message);
                    }
                };
                conversation.messages = editedMessages;
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
