namespace MyChat
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Security;
    using System.Text;
    using MindLink.Recruitment.MyChat;
    using Newtonsoft.Json;

    public sealed class ConversationExporter
    {
        static void Main(string[] args)
        {
            // Get the conversation exporter and parse the file arguments. 
            var conversationExporter = new ConversationExporter();
            var config = new CommandLineArgumentParser().ParseCommandLineArguments(args);

            // Get a reader and do some reading.  
            var reader = GetStreamReader(config.inputFilePath, FileMode.Open, FileAccess.Read, Encoding.ASCII);
            var conversation = conversationExporter.ExtractConversation(reader);
            
            // Get a writer and do some writing.
            var writer = GetStreamWriter(config.outputFilePath, FileMode.Create, FileAccess.ReadWrite);
            conversationExporter.WriteConversation(writer, conversation, config.outputFilePath);

            // And we're done. 
            Console.WriteLine($"Conversation exported from '{config.inputFilePath}' to '{config.outputFilePath}'");
        }

        public Conversation ExtractConversation(TextReader reader)
        {
            var messages = new List<Message>();
            
            // We assume that the first line will always be the conversation title.
            string conversationName = reader.ReadLine();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var array = line.Split(' ');
                
                //TODO: You've split the line out, now perform some validation.
                messages.Add(ArrayToMessage(array));
            }

            return new Conversation(conversationName, messages);
        }

        public void WriteConversation(TextWriter writer, Conversation conversation, string outputFilePath)
        {
            var serialized = JsonConvert.SerializeObject(conversation, Formatting.Indented);

            writer.Write(serialized);
            writer.Flush();
            writer.Close();
        }

        public static TextReader GetStreamReader(string inputFilePath, FileMode mode, FileAccess access, Encoding encoding)
        {
            // GetStreamReader takes in a file path, file mode, access permission and encoding style and returns a
            // StreamReader configured for that.
            // TODO: error handling with the attempt to create reader
            try
            {
                return new StreamReader(new FileStream(inputFilePath, mode, access), encoding);
            }
            catch (FileNotFoundException)
            {
                throw new ArgumentException($"The file {inputFilePath} was not found.");
            }
            catch (IOException)
            {
                throw new IOException("Something went wrong in the IO.");
            }
            catch (SecurityException)
            {
                throw new SecurityException($"Couldn't open the file {inputFilePath} because of a permissions issue");
            }
        }

        public static TextWriter GetStreamWriter(string outputFilePath, FileMode mode, FileAccess access)
        {
            // GetStreamWriter takes in an output file path, file mode and access permission and returns a
            // StreamReader configured based on those options.
            try
            {
                return new StreamWriter(new FileStream(outputFilePath, mode, access));
            }
            catch (SecurityException)
            {
                throw new SecurityException($"No permission to access {outputFilePath}.");
            }
            catch (DirectoryNotFoundException)
            {
                throw new DirectoryNotFoundException($"The path to {outputFilePath} is invalid and wasn't found.");
            }
            catch (IOException)
            {
                throw new IOException("Something went wrong in the IO.");
            }
        }

        public Message ArrayToMessage(string[] line)
        {
            var timestamp = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(line[0]));
            var senderID = line[1];
            var content = string.Join(" ", line[2..]);
            return new Message(timestamp, senderID, content);
        }
    }
}