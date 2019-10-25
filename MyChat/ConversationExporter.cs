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
            try
            {
                var conversationExporter = new ConversationExporter();
                ConversationExporterConfiguration configuration = new CommandLineArgumentParser().ParseCommandLineArguments(args);
                conversationExporter.ExportConversation(configuration);
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
        /// Exports the conversation at <paramref name="inputFilePath"/> as JSON to <paramref name="outputFilePath"/>.
        /// </summary>
        /// <param name="inputFilePath">
        /// The input file path.
        /// </param>
        /// <param name="outputFilePath">
        /// The output file path.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when file IO fails due to an invalid argument.
        /// </exception>
        /// <exception cref="Exception">
        /// Throw when file IO fails for a reason unrelated to the specified 
        /// arguments.
        /// </exception>
        public void ExportConversation(ConversationExporterConfiguration configuration)
        {
            try
            {
                Conversation conversation = this.ReadConversation(configuration.inputFilePath);
                this.ProcessConversation(conversation, configuration);
                this.WriteConversation(conversation, configuration.outputFilePath);
                Console.WriteLine("Conversation exported from '{0}' to '{1}'", configuration.inputFilePath, configuration.outputFilePath);
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Apply the necessary transormations to the specified <see cref="Conversation">Conversation</see> object 
        /// based on the specified <see cref="ConversationExporterConfiguration">ConversationExporterConfiguration</see> object. 
        /// </summary>
        /// <param name="conversation">The <see cref="Conversation">Conversation</see> to be transformed</param>
        /// <param name="configuration">The <see cref="ConversationExporterConfiguration"/>ConversationExporterConfiguration</see> to use</param>
        public void ProcessConversation(Conversation conversation, ConversationExporterConfiguration configuration)
        {
            if (!String.IsNullOrEmpty(configuration.userToFilter))
            {
                conversation.FilterByUser(configuration.userToFilter);
            }

            if (!String.IsNullOrEmpty(configuration.keywordToFilter))
            {
                conversation.FilterByKeyword(configuration.keywordToFilter);
            }

            if (!String.IsNullOrEmpty(configuration.blacklistedWords))
            {
                conversation.FilterBlacklist(configuration.blacklistedWords.Split(','));
            }

            if (configuration.hideNumbers)
            {
                conversation.HideCreditCardAndPhoneNumbers();
            }

            if (configuration.obfuscateUserID)
            {
                conversation.ObfuscateUserID();
            }

            conversation.generateMostActiveUsersReport();
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
        /// Thrown when reading file fails for another reason.
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
                    var split = line.Split('\t');

                    messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(split[0])), split[1], split[2]));
                }

                return new Conversation(conversationName, messages);
            }
            catch (FileNotFoundException)
            {
                throw new ArgumentException("The file was not found.");
            }
            catch (IOException)
            {
                throw new Exception("Input file could not be read.");
            }
            catch (Exception e)
            {
                throw new Exception("Input file could not be read");
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
        /// Thrown when writing the output file fails for any other reason.
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
                throw new ArgumentException("Insufficient permissions to write to this path");
            }
            catch (DirectoryNotFoundException)
            {
                throw new ArgumentException("The output path provided does not exist");
            }
            catch (IOException)
            {
                throw new Exception("The output file could not be written.");
            }
            catch (Exception)
            {
                throw new Exception("The output file could not be written");
            }
        }
    }
}
