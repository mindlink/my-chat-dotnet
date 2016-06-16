using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindLink.Recruitment.MyChat;
using System.IO;
using System.Security;
using Newtonsoft.Json;

namespace MindLink.Recruitment.MyChat
{
    public class ConversationReaderWriter
    {
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
        /// Thrown when a path is invalid.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown when something bad happens.
        /// </exception>
        public void ExportConversation(string inputFilePath, string outputFilePath) //simply copies a file from txt to json
        {
            Conversation conversation = this.ReadConversation(inputFilePath); // gets the message ID content, senderID, Timestamo

            this.WriteConversation(conversation, outputFilePath);

            Console.WriteLine("Conversation exported from '{0}' to '{1}'", inputFilePath, outputFilePath);
        }

        /// <summary>
        /// An overloaded function Exports the conversation at <paramref name="inputFilePath"/> as JSON to <paramref name="outputFilePath"/> but accepts more arguments
        /// allowing the user to specify an option and its arguments
        /// </summary>
        /// <param name="inputFilePath">
        /// The input file path.
        /// </param>
        /// <param name="outputFilePath">
        /// The output file path.
        /// </param>
        /// <param name="FilterSettings">
        /// An option to be specified by the user
        /// </param>
        /// <param name="parameterVal">
        /// an argument to go with the option
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when a path is invalid.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown when something bad happens.
        /// </exception>                        
        public void ExportConversation(string inputFilePath, string outputFilePath,string FilterSetting ,string parameterVal ) 
        {          
            if(FilterSetting.Equals("FilterBySender"))
            {
                Conversation conversation = this.ReadConversationAndFilterBySender(inputFilePath, parameterVal);
                this.WriteConversation(conversation, outputFilePath);
            }
            else if (FilterSetting.Equals("FilterByKeyword"))
            {
                Conversation conversation = this.ReadConversationAndFilterByKeyword(inputFilePath, parameterVal); // gets the message ID content, senderID, Timestamo
                this.WriteConversation(conversation, outputFilePath);
            }
            else if (FilterSetting.Equals("Blacklist"))
            {
                Conversation conversation = this.ReadConversationAndDepricate(inputFilePath, parameterVal); // gets the message ID content, senderID, Timestamo
                this.WriteConversation(conversation, outputFilePath);                               
            }
            
            Console.WriteLine("Conversation exported from '{0}' to '{1}'", inputFilePath, outputFilePath);
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
        /// Thrown when something else went wrong.
        /// </exception>
        private Conversation ReadConversation(string inputFilePath)
        {
            try
            {
                var reader = new StreamReader(new FileStream(inputFilePath, FileMode.Open, FileAccess.Read), Encoding.ASCII); // sets the reading setting

                string conversationName = reader.ReadLine(); // this would be the first line
                var messages = new List<Message>(); //A data structure containing Message (SenderID, content, converstaion time stamp, incorrect order)

                string line;

                while ((line = reader.ReadLine()) != null)
                {

                    var split = line.Split(' '); // splits whenever ' ' is encountered and stores each split within an array
                    
                    for (int i = 3 ; i< split.Length; i++) // this fixes the bug that caused the initial tests to fail, it is because the third split contained a single word rather than the full message
                    {
                           split[2] = split[2] + " " +  split[i]; 
                    
                    }

                    messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(split[0])), split[1], split[2] )); // there must be more splits, maybe we should avoid storing them
                }

                return new Conversation(conversationName, messages); // Message is the ine we are after contains message type
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


        /// <summary>
        /// Helper method to read the conversation from <paramref name="inputFilePath"/> whie filtering them by the specified SenderID.
        /// </summary>
        /// <param name="inputFilePath">
        /// The input file path.
        /// </param>
        /// <param name="sender">
        /// The senderID to filter messages by
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
        private Conversation ReadConversationAndFilterBySender(string inputFilePath, string Sender)
        {
            try
            {
                var reader = new StreamReader(new FileStream(inputFilePath, FileMode.Open, FileAccess.Read), Encoding.ASCII); // sets the reading setting

                string conversationName = reader.ReadLine(); // this would be the first line
                var messages = new List<Message>(); //A data structure containing Message (SenderID, content, converstaion time stamp, incorrect order)

                string line;

                while ((line = reader.ReadLine()) != null)
                {

                    var split = line.Split(' '); // splits whenever ' ' is encountered and stores each split within an array
                   
                    for (int i = 3; i < split.Length; i++)
                    {
                        split[2] = split[2] + " " + split[i]; // bad parctice keeps on creating new instances

                    }

                    if (split[1].Equals(Sender))
                    {
                        messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(split[0])), split[1], split[2])); // there must be more splits, maybe we should avoid storing them
                    }
                    else
                    {
                        //do nothing
                    }
                }

                return new Conversation(conversationName, messages); // Message is the ine we are after contains message type
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


        /// <summary>
        /// Helper method to read the conversation from <paramref name="inputFilePath"/> whie filtering them by the specified keywords
        /// </summary>
        /// <param name="inputFilePath">
        /// The input file path.
        /// </param>
        /// <param name="Keyword">
        /// The Keyword to filter messages by
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
        private Conversation ReadConversationAndFilterByKeyword(string inputFilePath, string Keyword)
        {
            try
            {
                var reader = new StreamReader(new FileStream(inputFilePath, FileMode.Open, FileAccess.Read), Encoding.ASCII); // sets the reading setting

                string conversationName = reader.ReadLine(); // this would be the first line
                var messages = new List<Message>(); //A data structure containing Message (SenderID, content, converstaion time stamp, incorrect order)

                string line;

                while ((line = reader.ReadLine()) != null)
                {

                    var split = line.Split(' '); // splits whenever ' ' is encountered and stores each split within an array
                    //split = Radicate(split);

                    for (int i = 3; i < split.Length; i++)
                    {
                        split[2] = split[2] + " " + split[i]; // bad parctice keeps on creating new instances

                    }

                    if (split[2].Contains(Keyword))
                    {
                        messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(split[0])), split[1], split[2])); // there must be more splits, maybe we should avoid storing them
                    }
                    else
                    {
                        //do nothing
                    }
                }

                return new Conversation(conversationName, messages); // Message is the ine we are after contains message type
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

        /// <summary>
        /// Helper method to read the conversation from <paramref name="inputFilePath"/> whie Radicating the vlacklisted words.
        /// </summary>
        /// <param name="inputFilePath">
        /// The input file path.
        /// </param>
        /// <param name="Blacklist">
        /// The words to be blacklisted seperated by spaces
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
        private Conversation ReadConversationAndDepricate(string inputFilePath, string Blacklist)
        {
            try
            {
                var reader = new StreamReader(new FileStream(inputFilePath, FileMode.Open, FileAccess.Read), Encoding.ASCII); // sets the reading setting

                string conversationName = reader.ReadLine(); // this would be the first line
                var messages = new List<Message>(); //A data structure containing Message (SenderID, content, converstaion time stamp, incorrect order)

                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var split = line.Split(' '); // splits whenever ' ' is encountered and stores each split within an array
                    var BlackListedWords = Blacklist.Split(' ');
                    
                    for (int j = 3; j < split.Length; j++)
                    {
                        for (int k = 0; k < BlackListedWords.Length ; k++)
                        {
                            if (split[j].Contains(BlackListedWords[k]) || split[j].Equals(BlackListedWords[k]))
                            {
                                split[j] = "*redacted*";
                            }
                        }
                    }

                    for (int i = 3; i < split.Length; i++)
                    {
                        split[2] = split[2] + " " + split[i]; // bad parctice keeps on creating new instances
                    }
                                        
                    messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(split[0])), split[1], split[2])); // there must be more splits, maybe we should avoid storing them
                    
                }

                return new Conversation(conversationName, messages); // Message is the ine we are after contains message type
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
        private void WriteConversation(Conversation conversation, string outputFilePath)
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

        //this function can be used to filter out credit card details and phone number once I know how they might appear within text
        private string[] Radicate (string[] splits)
        {

            string[] RadicateThese = {"pie","you"};

            for (int i = 0; i < splits.Length; i++ )
            {
                for(int k = 0; k < RadicateThese.Length ; k++)
                {
                    if (splits[i].Equals(RadicateThese[k]) || splits[i].Contains(RadicateThese[k]))
                    {
                        splits[i] = "*redacted*";
                    }
                }             

            }
            
            return splits;

        }

    }
    
}

