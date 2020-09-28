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
            string jsonFilePath = new CommandLineArgumentParser().ParseCommandLineArguments(args);
            
            // Setup the config based on what was in the JSON file. 
            var config =  JsonConvert.DeserializeObject<ConversationExporterConfiguration>(File.ReadAllText(jsonFilePath));
            
            ConversationExporter ce = new ConversationExporter();
            
            //Now run the code with all of the different dependencies that we need to get going.
            //Idea here is that future tests could fake IWhatever and for the important thing under test
            //then we can actually create that and pass it in to run.
            ce.Run(config, new FilterChainMaker(), new AdjusterChainMaker(), new TextReaderFactory(), new TextWriterFactory(), new JSONConversationWriter());
        }

        public void Run( ConversationExporterConfiguration config, IFilterChainMaker fcm, IAdjusterChainMaker acm, IReaderFactory trf, IWriterFactory twf, IConversationWriter cw) 
        {
            //Create chain of IFilters to go through.
            var filterChain = fcm.CreateFilterChain(config);
            
            //Create chain of IAdjusters to go through.
            var adjusterChain = acm.CreateAdjusterChain(config);

            // Get the reader.  
            var reader = trf.GetStreamReader(config);
            
            // Extract lines from conversation based on filtering rules and adjustment rules
            var conversation = ExtractConversation(reader, filterChain, adjusterChain);
            
            //Get the writer
            var writer = twf.GetStreamWriter(config);
            
            //Write the conversation out using our particular conversation writer style.
            cw.WriteConversation(writer, conversation);
            
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
    }
}