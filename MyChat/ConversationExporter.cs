namespace MyChat
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Security;
    using System.Text;
    using MindLink.Recruitment.MyChat;
    using Newtonsoft.Json;
    using System.Linq;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Represents a conversation exporter that can read a conversation and write it out in JSON.
    /// </summary>
    public sealed class ConversationExporter
    {

        /// <summary>
        /// Exports the conversation at <paramref name="inputFilePath"/> as JSON to <paramref name="outputFilePath"/>.
        /// </summary>
        /// <exception cref="NullReferenceException">
        /// Thrown when something is wrong while apploying the filters.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when there is a problem with the <paramref name="configuration.inputFilePath"/>..
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown when something else went wrong.
        /// </exception>
        public void ExportConversation(ConversationExporterConfiguration configuration)
        {
            try
            {
                Conversation conversation = new ConversationFilters().ApplyFilters(this.ReadConversation(configuration.inputFilePath), configuration);
                this.WriteConversation(conversation,configuration.outputFilePath);
                Console.WriteLine("Conversation exported from '{0}' to '{1}'", configuration.inputFilePath, configuration.outputFilePath);
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
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
        /// Thrown when there is a problem with the <paramref name="inputFilePath"/>.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown when something else went wrong.
        /// </exception>
        public Conversation ReadConversation(string inputFilePath)

        {
            try
            {
                StreamReader reader = new StreamReader(new FileStream(inputFilePath, FileMode.Open, FileAccess.Read), Encoding.ASCII);

                string conversationName = reader.ReadLine();
                var messages = new List<Message>();

                string line;

                while (( line = reader.ReadLine()) != null)
                {
                    var split = line.Split(' ');

                    messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(split[0])), split[1], String.Join(" ", split.Skip(2))));

                }

                return new Conversation(conversationName, messages);
            }
            catch (DirectoryNotFoundException)
            {
                throw new ArgumentException("Directory path invalid.");
            }
            catch (FileNotFoundException)
            {
                throw new ArgumentException("The file was not found.");
            }
            catch (SecurityException)
            {
                throw new ArgumentException("No permission to file.");
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("Something went wrong while reading a message.");
            }
            catch (FormatException)
            {
                throw new ArgumentException("The format of the timestamp is wrong.");
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
                StreamWriter writer = new StreamWriter(new FileStream(outputFilePath, FileMode.Create, FileAccess.ReadWrite));

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
                throw new ArgumentException("Directory path invalid.");
            }
            catch (IOException)
            {
                throw new Exception("Something went wrong in the IO.");
            }
        }

    }
}
