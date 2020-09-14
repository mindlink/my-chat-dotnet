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
            var conversationExporter = new ConversationExporter();
            var configuration = new CommandLineArgumentParser().ParseCommandLineArguments(args);

            var reader = GetStreamReader(configuration.inputFilePath, FileMode.Open, FileAccess.Read, Encoding.ASCII);
            
            var conversation = conversationExporter.ReadConversation(reader);
            
            conversationExporter.ExportConversation(conversation, configuration.inputFilePath, configuration.outputFilePath);
        }

        public void ExportConversation(Conversation c, string inputFilePath, string outputFilePath)
        {
            //Takes in a conversation and writes it to an output file path.
            WriteConversation(c, outputFilePath);
            Console.WriteLine("Conversation exported from '{0}' to '{1}'", inputFilePath, outputFilePath);
        }
        
        public Conversation ReadConversation(TextReader reader)
        {
            var messages = new List<Message>();
            string line;
            
            try
            {
                string conversationName = reader.ReadLine();
                while ((line = reader.ReadLine()) != null)
                {    
                    var split = line.Split(' ');
                    
                    var timestamp = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(split[0]));
                    var senderID = split[1];
                    var content = string.Join(" ",split[2..]);

                    //TODO: Now, check the item. If it's ok, do the appropriate validation.
                    
                    messages.Add(new Message(timestamp, senderID, content));
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
        
        public static TextReader GetStreamReader(string FilePath, FileMode mode, FileAccess access, Encoding encoding)
        {   //GetStreamReader takes in a file path, file mode, access permission and encoding style and returns a
            //StreamReader configured for that.
            //TODO: error handling with the attempt to create a reader
            return new StreamReader(new FileStream(FilePath, mode, access), encoding);
        }
    }
}
