namespace MyChat
{
    using MindLink.Recruitment.MyChat;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Security;
    using System.Text;

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

            conversationExporter.ExportConversation(
                configuration.InputFilePath, 
                configuration.OutputFilePath, 
                configuration.UserName, 
                configuration.Keyword, 
                configuration.Blacklist, 
                configuration.EncryptUsernames,
                configuration.HideNumbers);
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
        /// <param name="userName">
        /// The username to filter.
        /// </param>
        /// <param name="encryptUserNames">
        /// Boolean flag indicating whether to encrypt usernames or not.
        /// </param>
        /// <param name="hideNumbers">
        /// Boolean flag indicating whether to hide numbers or not.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when a path is invalid.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown when something bad happens.
        /// </exception>
        public void ExportConversation(
            string inputFilePath, 
            string outputFilePath, 
            string userName, 
            string keyword, 
            string blacklist, 
            bool encryptUserNames, 
            bool hideNumbers)
        {
            Conversation conversation = this.ReadConversation(inputFilePath, userName, keyword, blacklist);

            if (hideNumbers)
            {
                ContentParser.HideNumbersFromConvesation(conversation);
            }

            if (encryptUserNames)
            {
                UserNameEncryption.EncryptUserNames(conversation);
            }

            Activity.SetUserActivityInConversation(conversation);

            this.WriteConversation(conversation, outputFilePath);

            Console.WriteLine("Conversation exported from '{0}' to '{1}'", inputFilePath, outputFilePath);
        }

        /// <summary>
        /// Helper method to read the conversation from <paramref name="inputFilePath"/>.
        /// </summary>
        /// <param name="inputFilePath">
        /// The input file path.
        /// </param>
        /// <param name="userName">
        /// The string representing the username to filter messages with.
        /// </param>
        /// <param name="keyword">
        /// The string used in to match message content by keyword.
        /// </param>
        /// <param name="blacklist">
        /// The strings to be blacklisted from message content.
        /// </param>
        /// <param name="encryptUsernames">
        /// Boolean flag indicating whether to encrypt usernames or not.
        /// </param>
        /// <param name="hideNumbers">
        /// Boolean flag indicating whether to hide numbers or not.
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
        public Conversation ReadConversation(string inputFilePath, string userName, string keyword, string blacklist)
        {
            try
            {
                FileStream stream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read);
                // using statement to properly dispose reader at end of scope.
                using (StreamReader reader = new StreamReader(stream, Encoding.ASCII))
                {
                    string conversationName = reader.ReadLine();
                    var messages = new List<Message>();

                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        Message msg = MessageGenerator.CreateNewMessage(line, blacklist);

                        if (MessageFilter.Filter(msg, userName, keyword))
                        {
                            // add the newly instantiated message in List.
                            messages.Add(msg);
                        }
                    }
                    
                    return new Conversation(conversationName, messages);
                }
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException("The file was not found.");
            }
            catch (IOException)
            {
                throw new IOException("Something went wrong in the IO.");
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
                FileStream stream = new FileStream(outputFilePath, FileMode.Create, FileAccess.ReadWrite);
                // using statement to properly dispose writer at end of scope.
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    var serialized = JsonConvert.SerializeObject(conversation, Formatting.Indented);

                    writer.Write(serialized);

                    writer.Flush();
                }
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
                throw new IOException("Something went wrong in the IO.");
            }
        }
    }
}
