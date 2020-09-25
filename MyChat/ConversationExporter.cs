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

    /// <summary>
    /// Represents a conversation exporter that can read a conversation and write it out in JSON.
    /// </summary>
    public sealed class ConversationExporter
    {
        // user input
        string filterType = "";
        string User;

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
            Conversation conversation = this.ReadConversation(inputFilePath);

            //FilterConversation(conversation);

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
                    var RefactoredContent = split.Skip(2).ToArray();
                    var CompleteSentence = string.Join(" ", RefactoredContent);

                    messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(split[0])), split[1], CompleteSentence)); //split[2]
                }
                // after reading text file is complete optional message filtering is after

                // insatnce of custom filter class
                TextFilter filter = new TextFilter();

                Console.WriteLine("Select filter type(Name|Keyword|Redacted) type anything else to ignore");

                filterType = Console.ReadLine();

                switch (filterType.ToLower())
                {// if user wants to filter by name
                    case "name":
                        Console.WriteLine("Filter Type: By Name");
                        Console.WriteLine("Which Name?");
                        User = Console.ReadLine();
                        var NamefilteredMessages = filter.NameFilter(messages, User);
                        return new Conversation(conversationName, NamefilteredMessages);

                    // if user wants to filter by keyword      
                    case "keyword":
                        Console.WriteLine("Filter Type: By Key Word");
                        Console.WriteLine("Which word?");
                        User = Console.ReadLine();
                        var wordfilteredMessages = filter.KeywordFilter(messages, User);
                        return new Conversation(conversationName, wordfilteredMessages);

                    // if user wants to redact a certain word in messages
                    case "redacted":
                        Console.WriteLine("Filter Type: Redact specific word");
                        Console.WriteLine("Which word?");
                        User = Console.ReadLine();
                        var redactFilteredMessages = filter.RedactedWordFilter(messages, User);
                        return new Conversation(conversationName, redactFilteredMessages);

                    // shown if no filter is applied       
                    default:
                        Console.WriteLine("No Filter selected");
                        break;
                }


                return new Conversation(conversationName, messages);//messages //filteredmessages
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
                //var messages = new List<Message>();

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