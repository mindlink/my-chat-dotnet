using System.ComponentModel.DataAnnotations;
using System.Linq;

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
            // Get the path to the JSON config file
            // string jsonConfig = new CommandLineArgumentParser().ParseCommandLineArguments(args);
            
            // Now setup the config based on what was in the JSON file. 
            // var config = JsonConvert.DeserializeObject<ConversationExporterConfiguration>(File.ReadAllText(jsonConfig));
            

            // var conversationExporter = new ConversationExporter();

            //
            // // Get a reader and do some reading.  
            // var reader = conversationExporter.GetStreamReader(config.inputFilePath, FileMode.Open, FileAccess.Read, Encoding.ASCII);
            // var conversation = conversationExporter.ExtractConversation(reader);
            //
            // Console.WriteLine(conversation.Name);
            //
            // // Get a writer and do some writing.
            // var writer = conversationExporter.GetStreamWriter(config.outputFilePath, FileMode.Create, FileAccess.ReadWrite);
            // conversationExporter.WriteConversation(writer, conversation, config.outputFilePath);
            //
            // // And we're done. 
            // Console.WriteLine($"Conversation exported from '{config.inputFilePath}' to '{config.outputFilePath}'");
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

        public TextReader GetStreamReader(string inputFilePath, FileMode mode, FileAccess access, Encoding encoding)
        {
            // GetStreamReader takes in a file path, file mode, access permission and encoding style and returns a
            // StreamReader configured for that.
            try
            {
                return new StreamReader(new FileStream(inputFilePath, mode, access), encoding);
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException($"The file {inputFilePath} was not found.");
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

        public TextWriter GetStreamWriter(string outputFilePath, FileMode mode, FileAccess access)
        {
            // GetStreamWriter takes in an output file path, file mode and access permission and returns a
            // Writer configured based on those options.
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
            var timestamp = StringToUnixTimeStamp(line[0]);
            var senderID = line[1];
            var content = string.Join(" ", line[2..]);
            return new Message(timestamp, senderID, content);
        }

        public DateTimeOffset StringToUnixTimeStamp(string s)
        {    // StringToUnixTimeStamp does as its name suggests: it takes in a string
            // and parses it into a unix timestamp. 
            //TODO: some error checks. Maybe a try parse and if that fails return. 
            return DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(s));
        }
        
        public Func<string, string, bool> StringEqual = (item, query) => item == query;
        
        public Func<string, string, bool> KeywordInMessage = (message, kw) => message.Split(" ").Any(word => word == kw);

        public string AdjustBlacklistedWord(string message, string blacklist)
        {
            if(!KeywordInMessage(message, blacklist))
            {
                return message;
            }

            List<string> final = new List<string>();
            
            foreach (var word in message.Split())
            {
                if(word == blacklist)
                {
                    final.Add("*redacted*");    
                    continue;
                }
                final.Add(word);
            }

            return string.Join(" ", final);
        }
    }
}