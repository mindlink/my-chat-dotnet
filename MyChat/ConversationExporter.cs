using System;
using System.Collections.Generic;
using System.Text;

namespace MyChat
{
    using System.IO;
    using MindLink.Recruitment.MyChat;
    using Newtonsoft.Json;

    public sealed class ConversationExporter
    {
        static void Main(string[] args)
        {
            ConversationExporter ce = new ConversationExporter();
            
            string jsonFilePath = new CommandLineArgumentParser().ParseCommandLineArguments(args);
            
            // Setup the config based on what was in the JSON file. 
            var config =
                JsonConvert.DeserializeObject<ConversationExporterConfiguration>(File.ReadAllText(jsonFilePath));
            
            //Create chain of IFilters to go through.
            var filterChain = FilterChainMaker.CreateFilterChain(config);
            
            //Create chain of IAdjusters to go through.
            var adjusterChain = AdjusterChainMaker.CreateAdjusterChain(config);

            // Get the reader.  
            var reader = TextReaderFactory.GetStreamReader(config.inputFilePath, FileMode.Open, FileAccess.Read,
                Encoding.ASCII);
            
            // Extract lines from conversation based on filtering rules and adjustment rules
            var conversation = ce.ExtractConversation(reader, filterChain, adjusterChain);
            
            //Get the writer
            var writer = TextWriterFactory.GetStreamWriter(config.outputFilePath, FileMode.Create,
                FileAccess.ReadWrite);
            
            //Write the conversation out.
            ce.WriteConversation(writer, conversation);
            
            Console.WriteLine($"Conversation exported from '{config.inputFilePath}' to '{config.outputFilePath}'");
        }


        public Conversation ExtractConversation(TextReader reader, IFilterer filterChain, IAdjuster bannedTermChain)
        {
            // ExtractConversation reads lines of text. At each line, it checks if a message passes validation
            // and adjusts the message according to a set of rules (if specified). 
            var messages = new List<IMessage>();

            // We assume that the first line will always be the conversation title.
            string conversationName = reader.ReadLine();
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                var array = line.Split(' ');

                var message = new Message(array);

                //Filter if we need to and validate that everything passes.
                if (filterChain != null && !filterChain.Filter(message))
                {
                    //Failed validation. Skip this line and move to the next one. 
                    continue;
                }

                //The validation passed.
                //Now let's adjust it if needed.
                // Go through our adjuster chain and adjust the message as needed.
                if (bannedTermChain != null)
                {
                    var newMessage = bannedTermChain.Adjust(message);
                    messages.Add(newMessage);
                }
                else
                {
                    messages.Add(message);
                }
            }

            return new Conversation(conversationName, messages);
        }


        public void WriteConversation(TextWriter writer, Conversation conversation)
        {
            var serialized = JsonConvert.SerializeObject(conversation, Formatting.Indented);

            writer.Write(serialized);
            writer.Flush();
            writer.Close();
        }
    }
}