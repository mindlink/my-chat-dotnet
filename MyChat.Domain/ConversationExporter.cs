using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using Newtonsoft.Json;

namespace MindLink.MyChat.Domain
{
    /// <summary>
    ///     Represents a conversation exporter that can read a conversation and write it out in JSON.
    /// </summary>
    public sealed class ConversationExporter
    {
        private readonly ConversationExporterConfiguration configuration;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="configuration">
        ///     Configuration for exporter
        /// </param>
        public ConversationExporter(ConversationExporterConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        ///     Exports the conversation.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Thrown when a path is invalid.
        /// </exception>
        /// <exception cref="Exception">
        ///     Thrown when something bad happens.
        /// </exception>
        public void ExportConversation()
        {
            var conversation = this.ReadConversation();
            conversation = this.Filter(conversation);
            conversation = this.Transform(conversation);

            this.WriteConversation(conversation);

            Console.WriteLine("Conversation exported from '{0}' to '{1}'", this.configuration.InputFilePath,
                this.configuration.OutputFilePath);
        }

        private Conversation Transform(Conversation conversation)
        {
            var result = conversation.Messages.Select(
                m => this.configuration.Transformers.Aggregate(m,
                    (message, transformer) => transformer.TransformMessage(m)));
            
            return new Conversation(conversation.Name, result);
        }

        private Conversation Filter(Conversation conversation)
        {
            var result =
                conversation.Messages.Where(m => this.configuration.Filters.All(f => f.IncludeMessage(m)))
                    .ToList();

            return new Conversation(conversation.Name, result);
        }

        /// <summary>
        ///     Helper method to read the conversation from input file"/>.
        /// </summary>
        /// <returns>
        ///     A <see cref="Conversation" /> model representing the conversation.
        /// </returns>
        /// <exception cref="ArgumentException">
        ///     Thrown when the input file could not be found.
        /// </exception>
        /// <exception cref="Exception">
        ///     Thrown when something else went wrong.
        /// </exception>
        private Conversation ReadConversation()
        {
            try
            {
                using (var reader = File.OpenText(this.configuration.InputFilePath))
                {
                    var conversationName = reader.ReadLine();
                    var messages = new List<Message>();

                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        var split = line.Split(' ');

                        var content = string.Join(" ", split.Skip(2));
                        messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(split[0])),
                            split[1], content));
                    }

                    return new Conversation(conversationName, messages);
                }
            }
            catch (FileNotFoundException e)
            {
                throw new ArgumentException("The file was not found.", e);
            }
            catch (IOException e)
            {
                throw new Exception("Something went wrong in the IO.", e);
            }
        }

        /// <summary>
        ///     Helper method to write the <paramref name="conversation" /> as JSON to output file/>.
        /// </summary>
        /// <param name="conversation">
        ///     The conversation.
        /// </param>
        /// <exception cref="Exception">
        ///     Thrown when something else bad happens.
        /// </exception>
        private void WriteConversation(Conversation conversation)
        {
            try
            {
                using (var writer = File.CreateText(this.configuration.OutputFilePath))
                {
                    var serialized = JsonConvert.SerializeObject(conversation, Formatting.Indented);

                    writer.Write(serialized);
                }
            }
            catch (SecurityException e)
            {
                throw new ArgumentException("No permission to file.", e);
            }
            catch (DirectoryNotFoundException e)
            {
                throw new ArgumentException("Path invalid.", e);
            }
            catch (IOException e)
            {
                throw new Exception("Something went wrong in the IO.", e);
            }
        }
    }
}