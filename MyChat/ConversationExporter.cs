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
            var conversationExporter = new ConversationExporter();
            var configuration = new CommandLineArgumentParser().ParseCommandLineArguments(args);

            conversationExporter.ExportConversation(configuration.inputFilePath, configuration.outputFilePath);
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
            var conversation = this.ReadConversation(inputFilePath);

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
                using (var reader = new StreamReader(new FileStream(inputFilePath, FileMode.Open, FileAccess.Read),
                    Encoding.ASCII))
                {
                    var conversationName = reader.ReadLine();
                    var messages = new List<Message>();

                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        var split = line.Split(' ');

                        messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(split[0])), split[1], split[2]));
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
                using (var writer = new StreamWriter(new FileStream(outputFilePath, FileMode.Create, FileAccess.ReadWrite)))
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
