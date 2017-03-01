using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Text;
using Newtonsoft.Json;
using System.Linq;
using Newtonsoft.Json.Linq;
using MyChat.Models;
using MyChat.Tools;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyChat.Exporter
{

    /// <summary>
    /// A class used for exporting a conversation from a TXT file to a JSON file.
    /// </summary>
    public sealed class ConversationExporter
    {

        /// <summary>
        /// The input file path.
        /// </summary>
        public string inputFilePath;

        /// <summary>
        /// The output file path.
        /// </summary>
        public string outputFilePath;

        /// <summary>
        /// Initializes an instance of the class.
        /// </summary>
        /// <param name="inputPath">
        /// The input file path.
        /// </param>
        /// <param name="outputPath">
        /// The output file path.
        /// </param>
        public ConversationExporter (string inputPath, string outputPath)
        {
            this.inputFilePath = inputPath;
            this.outputFilePath = outputPath;
        }

        /// <summary>
        /// Reads a <see cref="Conversation"/> from the input file path and exports it to as JSON file.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// Thrown when there is a problem with the any of the arguments.
        /// </exception>
        /// <exception cref="IOException">
        /// Thrown when something with the IO went wrong.
        /// </exception>
        public bool ExportConversation(ConversationFilters filters)
        {
            try
            {
                Conversation conversation = this.ReadConversation();
                if (conversation == null)
                {
                    return false;
                }
                Conversation filteredConversation = filters.ApplyFilters(conversation);
                return  this.WriteConversation(filteredConversation);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Something went wrong while exporting the conversation. Please restart the appliction and try again.");
                Console.WriteLine(e.Message);
                return false;
            }
            catch (OutOfMemoryException e)
            {
                Console.WriteLine("The application ran out of memory while exporting the conversation. Please restart the appliction and try again.");
                Console.WriteLine(e.Message);
                return false;
            }
            catch (RegexMatchTimeoutException e)
            {
                Console.WriteLine("Appying the iflters is taking too long. Please restart the appliction and try again.");
                Console.WriteLine(e.Message);
                return false;
            }
            catch (IOException e)
            {
                Console.WriteLine("Something went wrong in the IO while exporting the conversation. Please restart the appliction and try again.");
                Console.WriteLine(e.Message);
                return false;
            }
        }

        /// <summary>
        ///A method that reads from the input file path.
        /// </summary>
        /// <returns>
        /// A <see cref="Conversation"/> model representing the conversation.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when there is a problem with the any of the arguments.
        /// </exception>
        /// <exception cref="IOException">
        /// Thrown when something with the IO went wrong.
        /// </exception>
        /// <exception cref="OutOfMemoryException">
        /// Thrown when the file is too large.
        /// </exception>
        public Conversation ReadConversation()

        {
            try
            {
                Conversation conversation = new Conversation();

                StreamReader reader = new StreamReader(new FileStream(this.inputFilePath, FileMode.Open, FileAccess.Read), Encoding.ASCII);

                conversation.name = reader.ReadLine();
                var messages = new List<Message>();

                if (conversation.name == null)
                {
                    Console.WriteLine("The input file is empty. The application will terminate.");
                    return null;
                }

                string line;

                while (( line = reader.ReadLine()) != null)
                {
                    var split = line.Split(' ');
                    Message message = new Message();
                    User user = new User();
                    message.timestamp = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(split[0]));
                    user.username = split[1];
                    message.sender = user;
                    message.content = String.Join(" ", split.Skip(2));
                    messages.Add(message);

                }

                conversation.messages = messages;

                return conversation;
            }
            catch (DirectoryNotFoundException)
            {
                throw new ArgumentException("Directory path of the input file was invalid.");
            }
            catch (FileNotFoundException)
            {
                throw new ArgumentException("The file specified by the input path was not found.");
            }
            catch (SecurityException)
            {
                throw new ArgumentException("No permission to the file specified by the input path");
            }
            catch (FormatException)
            {
                throw new ArgumentException("The format of the timestamp in one of the messages in the input file is wrong.");
            }
            catch (IndexOutOfRangeException)
            {
                throw new ArgumentException("The messages in the input file contain insufficient number of arguments.");
            }
            catch (OutOfMemoryException)
            {
                throw new OutOfMemoryException("The input file is too large to read.");
            }
            catch (IOException)
            {
                throw new IOException("Something went wrong in the IO while readint from the input file.");
            }
        }

        /// <summary>
        /// A method that writes the <paramref name="conversation"/> as JSON to the output path.
        /// </summary>
        /// <param name="conversation">
        /// An instance of the <see cref="Conversation"/>.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when there is a problem with the any of the arguments.
        /// </exception>
        /// <exception cref="IOException">
        /// Thrown when something with the IO went wrong.
        /// </exception>
        public bool WriteConversation(Conversation conversation)
        {
            try
            {
                StreamWriter writer = new StreamWriter(new FileStream(this.outputFilePath, FileMode.Create, FileAccess.ReadWrite));

                var serialized = JsonConvert.SerializeObject(conversation, Formatting.Indented);
                
                writer.Write(serialized);

                writer.Flush();

                writer.Close();

                return true;
            }
            catch (SecurityException)
            {
                throw new ArgumentException("No permission to the file specified by the output path.");
            }
            catch (DirectoryNotFoundException)
            {
                throw new ArgumentException("Directory path of the ouput file was invalid.");
            }
            catch (IOException)
            {
                throw new IOException("Something went wrong in the IO while writing to the output file.");
            }
        }

    }
}
