namespace MyChat
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Security;
    using System.Text;
    using MindLink.Recruitment.MyChat;
    using Newtonsoft.Json;
    using System.Diagnostics;

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
            ConversationExporterConfiguration configuration = new CommandLineArgumentParser().ParseCommandLineArguments(args);
            conversationExporter.ExportConversation(configuration);
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
        /// 
        //public void ExportConversation(string inputFilePath, string outputFilePath, string userFilter)
        public void ExportConversation(ConversationExporterConfiguration configuration)
        {
            Conversation conversation = this.ReadConversation(configuration);
            this.WriteConversation(conversation, configuration.outputFilePath);

            Console.WriteLine("Conversation exported from '{0}' to '{1}'", configuration.inputFilePath, configuration.outputFilePath);
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
        /// <exception cref="FormatException">
        /// Misformed and empty Inputlines will cause a Console Log, not throwned
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the input file could not be found.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown when something else went wrong.
        /// </exception>
        public Conversation ReadConversation(ConversationExporterConfiguration configuration)
        {
            int maxSplit = 3;                                       // max times to split the input=string
            string delimitterString = " ";                          // possible delimitter strings, add others without separator
            char[] delimitter = delimitterString.ToCharArray();     // char arr with delimitters
            string line = null;                                     // input line to be stript down
            string[] split = null;

            Console.WriteLine("Converter Log\n-------------");    // write Log Title

            try
            {
                var reader = new StreamReader(new FileStream(configuration.inputFilePath, FileMode.Open, FileAccess.Read),
                    Encoding.ASCII);

                string conversationName = reader.ReadLine();
                var messages = new List<Message>();

                while ((line = reader.ReadLine()) != null)
                {
                    split = line.Split(delimitter, maxSplit);       // split inputline as maxSplit times, on delimitters

                    try
                    {
                        messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(split[0])), split[1], split[2]));
                    }
                    catch (FormatException e)
                    {
                        // occurs at empty line feeds because of missformed date/time inputs
                        // write console log and ignore this line
                        Console.WriteLine(e.Message.ToString() + " Input: " + line.ToString());
                        continue;
                    }
                }
                return new Conversation(conversationName, messages, configuration);
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
