namespace MyChat
{
    using System;
    using System.Linq;
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
            string jsonConfig = new CommandLineArgumentParser().ParseCommandLineArguments(args);

            // Now setup the config based on what was in the JSON file. 
            var config = JsonConvert.DeserializeObject<ConversationExporterConfiguration>(File.ReadAllText(jsonConfig));

            var conversationExporter = new ConversationExporter();

            // Get a reader and do some reading.  
            var reader = conversationExporter.GetStreamReader(config.inputFilePath, FileMode.Open, FileAccess.Read,
                Encoding.ASCII);

            // Extract conversation according to the rules specified by the user 
            var conversation = conversationExporter.ExtractConversation(reader, config);

            // Get a writer and do some writing.
            var writer =
                conversationExporter.GetStreamWriter(config.outputFilePath, FileMode.Create, FileAccess.ReadWrite);
            conversationExporter.WriteConversation(writer, conversation, config.outputFilePath);

            // And we're done. 
            Console.WriteLine($"Conversation exported from '{config.inputFilePath}' to '{config.outputFilePath}'");
        }

        public Conversation ExtractConversation(TextReader reader, ConversationExporterConfiguration rules)
        {
            // ExtractConversation reads lines of text. At each line, it checks the rules from the config
            // to see if a line should be in the final output.
            var messages = new List<Message>();

            // We assume that the first line will always be the conversation title.
            string conversationName = reader.ReadLine();
            string line;
            
            while ((line = reader.ReadLine()) != null)
            {
                var array = line.Split(' ');

                var message = ArrayToMessage(array);

                List<bool> validationResults = new List<bool>();
                
                //You've got a line, now check it depending on the rules in the config.
                //TODO: this absolutely needs to be refactored into its own function that returns a bool if all pass.
                if (rules.UserToFilter != null)
                {
                    Console.WriteLine("Filtering by user name.");
                    validationResults.Add(UsernameFound(message, rules.UserToFilter));
                }

                if (rules.KeywordToFilter != null)
                {
                    Console.WriteLine("Filtering by keyword.");
                    validationResults.Add(KeywordInMessage(message, rules.KeywordToFilter));
                }

                if (!validationResults.All(x => x))
                {
                    continue;//The validation failed so you ditch this line and move to the next one.
                }

                Console.WriteLine("Got through validation.");
                if (rules.BlacklistedTerm != null)
                {    
                    Console.WriteLine("Checking for and going to erase banned term.");
                    var MessageAfterAdjustment = AdjustBlacklistedWord(message, rules.BlacklistedTerm);
                    messages.Add(MessageAfterAdjustment);
                }
                else{
                    Console.WriteLine($"Adding: {message.timestamp}, {message.senderId}, {message.content}"); 
                    messages.Add(message);
                }
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
                throw new SecurityException(
                    $"Couldn't open the file {inputFilePath} because of a permissions issue");
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
        {
            // StringToUnixTimeStamp does as its name suggests: it takes in a string
            // and parses it into a unix datetimeoffset timestamp. 
            //TODO: some error checks. Maybe a try parse and if that fails return. 
            return DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(s));
        }

        public Func<Message, string, bool> UsernameFound = (message, query) => message.senderId == query;

        public Func<Message, string, bool> KeywordInMessage = (m, kw) => m.content.Split(" ").Any(j => j == kw);

        public Message AdjustBlacklistedWord(Message message, string bannedTerm)
        {
            //TODO: need to be able to handle the case where conversation='pie?' and banned-word='pie'. 
            if (!KeywordInMessage(message, bannedTerm))
            {
                return message;
            }

            List<string> final = new List<string>();

            foreach (var word in message.content.Split())
            {
                if (word == bannedTerm)
                {
                    final.Add("*redacted*");
                    continue;
                }

                final.Add(word);
            }

            return new Message(message.timestamp, message.senderId, string.Join(" ", final));
        }

    }
}