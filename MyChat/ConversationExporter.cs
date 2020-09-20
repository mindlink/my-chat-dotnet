using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace MyChat
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.IO;
    using MindLink.Recruitment.MyChat;
    using Newtonsoft.Json;

    public sealed class ConversationExporter
    {
        static void Main(string[] args)
        {
            var conversationExporter = new ConversationExporter();
            string jsonFilePath = new CommandLineArgumentParser().ParseCommandLineArguments(args);

            // Now setup the config based on what was in the JSON file. 
            var config =
                JsonConvert.DeserializeObject<ConversationExporterConfiguration>(File.ReadAllText(jsonFilePath));

            var filters = conversationExporter.CreateValidationFuncs(config);

            // Get the reader.  
            var reader = conversationExporter.GetStreamReader(config.inputFilePath, FileMode.Open, FileAccess.Read,
                Encoding.ASCII);

            // Extract lines from conversation based on filtering rules. 
            //TODO: pass in an 'adjuster' that does message modification if it needs to. 
            var conversation = conversationExporter.ExtractConversation(reader, filters, config);

            // Write out the conversation to the file.. 
            conversationExporter.WriteConversation(
                conversationExporter.GetStreamWriter(config.outputFilePath, FileMode.Create, FileAccess.ReadWrite)
                , conversation, config.outputFilePath);

            Console.WriteLine($"Conversation exported from '{config.inputFilePath}' to '{config.outputFilePath}'");
        }

        public Conversation ExtractConversation(TextReader reader, List<Func<Message, bool>> ValidationRules,
            ConversationExporterConfiguration rules)
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

                if (!ValidationRules.All(f => f(message)))
                {
                    //Failed validation. Skip this line and move to the next one. 
                    continue;
                }

                if (rules.BannedTerm != null)
                {
                    messages.Add(SanitiseMessage(message, rules.BannedTerm));
                }
                else
                {
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
            // GetStreamWriter takes in an output file path, file mode and access permission and returns a Writer
            // configured based on those options.
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
            // StringToUnixTimeStamp does as its name suggests: it takes in a string and parses it to datetimeoffset.
            return DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(s));
        }

        public Func<Message, bool> UsernameFound(string nameFromConfig)
        {
            return (Message m) => m.senderId == nameFromConfig;
        }

        public Func<Message, bool> KeywordInMessage(string keywordToFind)
        {
            //KeywordInMessage will search a message for a given keyword. 
            //Upper and lowercase orthographic differences DO NOT matter. Hello and hello are the same.
            //If the equality test for two lowercase words fails, it will also attempt to make sure we're not being foxed
            //by punctuation. Therefore, if a user wants "foxed" and the message contains "foxed!", we'll find it.

            return (Message m) =>
            {
                return m.content.Split(" ").Any(word => word.ToLower() == keywordToFind.ToLower() ||
                                                        StripPunctuation(word).ToLower() == keywordToFind.ToLower());
            };
        }

        public Message SanitiseMessage(Message message, string bannedTerm)
        {
            //SanitiseMessage takes in a message and a bannedTerm and returns a new
            //message where the banned terms have been removed. It isn't case sensitive but it
            //is 'aware' of terminal punctuation. That means a word of "hello" and "hello!" will be "*redacted*!" in the output. 

            if (!KeywordInMessage(bannedTerm)(message))
            {
                return message;
            }

            List<string> final = new List<string>();

            foreach (var word in message.content.Split(" "))
            {
                //The word and the banned term match exactly:
                if (String.Equals(word, bannedTerm, StringComparison.CurrentCultureIgnoreCase))
                {
                    final.Add("*redacted*");
                    continue;
                }

                //Check if the bannedTerm is actually hidden inside a word.
                //If it is, is that word being padded on the right by terminal punctuation marks (. ! ; ... ?) ?
                if (word.Contains(bannedTerm, StringComparison.CurrentCultureIgnoreCase) &&
                    WordsMatchAfterTerminalPunctuationRemoved(FindStartOfTerminalPunctuation_English(word), word, bannedTerm))
                {
                    final.Add($"*redacted*{word.Substring(FindStartOfTerminalPunctuation_English(word))}");
                }

                //Word and banned term don't match.
                else
                {
                    final.Add(word);
                }
            }

            return new Message(message.timestamp, message.senderId, string.Join(" ", final));
        }


        public List<Func<Message, bool>> CreateValidationFuncs(ConversationExporterConfiguration rules)
        {
            // CreateValidationFuncs create a list of filtering functions based on what the user gave us in the config.
            // The idea then is that, based on a single Message, you can call .All() on them. If all the Funcs pass,
            // then the message is ok.
            List<Func<Message, bool>> Funcs = new List<Func<Message, bool>>();

            if (rules.UserToFilter != null)
            {
                Funcs.Add(UsernameFound(rules.UserToFilter));
            }

            if (rules.KeywordToFilter != null)
            {
                Funcs.Add(KeywordInMessage(rules.KeywordToFilter));
            }
            return Funcs;
        }

        public string StripPunctuation(string word)
        {
            var builder = new StringBuilder();
            foreach (var c in word)
            {
                if (!char.IsPunctuation(c))
                {
                    builder.Append(c);
                }
            }
        
            return builder.ToString();
        }

        public int FindStartOfTerminalPunctuation_English(string word)
        {
            // This function searches through a word and returns the point at which the first piece of terminal
            // punctuation is found. It will return -1 if no punctuation is found or if the first piece of punctuation
            // is followed by alphanumeric characters. 

            // This function exists to help with a specific case where the word you're looking for is affected by
            // terminal punctuation. For example, if the keyword is 'pie' and you find 'pie...'.  

            // The _English suffix foregrounds that this is brittle. If we start searching through a Spanish chat log
            // and messages start with ¿, then we'll struggle.

            //Find out where the first piece of punctuation exists.
            var FirstPuncIndex = word.ToList().FindIndex(char.IsPunctuation);

            //Now check that only punctuation follows after the first occurence of punctuation in the word.
            bool OnlyPunctuationAfterFirst = word.Substring(FirstPuncIndex).All(char.IsPunctuation);


            if (!OnlyPunctuationAfterFirst)
            {
                // We bail here because we've either not found punctuation OR we've found punctuation followed by letters 
                return -1;
            }

            return FirstPuncIndex;
        }

        public bool WordsMatchAfterTerminalPunctuationRemoved(int index, string word, string bt)
        {
            //If there's a non-zero index, we can infer there's terminal punctuation that exists in the word. 
            //If that's the case and you get rid of that chunk of punctuation, do the word and the bt word match?
            return index != -1 &&
                   String.Equals(word.Substring(0, index), bt, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}