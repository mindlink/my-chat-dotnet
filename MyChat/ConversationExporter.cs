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
            var conversationExporter = new ConversationExporter();
            ConversationExporterConfiguration configuration = new CommandLineArgumentParser().ParseCommandLineArguments(args);

            int choice = 0; 
            do
            {
                StringBuilder blackListString = new StringBuilder();
                if (configuration.wordsBlacklist != null)
                {
                    blackListString.Append("[");
                    foreach (string str in configuration.wordsBlacklist)
                    {

                        blackListString.Append(str + ",");
                    }
                    blackListString.Remove(blackListString.Length-1, 1);
                    blackListString.Append("]");
                }
                Console.WriteLine("What to do next?");
                Console.WriteLine("1. --Filter By Username-- {0}\n2. --Filter By Keyword-- {1}\n3. --Hide Specific Words-- {2}\n4. --Obfuscate user IDs-- {3}\n5. Clear Filters\n6. Convert and Export", configuration.usernameFilter, configuration.keyword, blackListString.ToString(), configuration.obfuscateUserIDsFlag);
                //read user input
                string key = Console.ReadKey().Key.ToString();
                Console.WriteLine();
                switch (key)
                {
                    case "D1": //filter by username 
                        {
                            do
                            {
                                configuration.usernameFilter = conversationExporter.PromptUserForInput("Please give the username to filter with");
                                configuration.filtersActive = true;
                                break;

                            }while(true);

                            break;
                        }
                    case "D2": //filter by keyword
                        {
                            configuration.keyword = conversationExporter.PromptUserForInput("Please give the keyword to filter with");
                            configuration.filtersActive = true;
                            break;
                        }
                    case "D3"://hide keywords
                        {
                            StringBuilder input = new StringBuilder();
                            input.Append(conversationExporter.PromptUserForInput("Enter words to hide separated with comma(,): "));
                            input.Replace(" ", "");
                            if (input[0] == ',')
                            {
                                input.Remove(0, 1);
                            }
                            if (input[input.Length - 1] == ',')
                            {
                                input.Remove(input.Length - 1, 1);
                            }
                            configuration.wordsBlacklist = input.ToString().Split(',');
                            break;
                        }
                    case "D4"://obfuscate user ids
                        {
                            configuration.obfuscateUserIDsFlag = true;
                            break;
                        }
                    case "D5"://clear filters
                        {
                            configuration.usernameFilter = null;
                            configuration.wordsBlacklist = null;
                            configuration.keyword        = null;
                            configuration.filtersActive  = false;
                            configuration.obfuscateUserIDsFlag = false;
                            break;
                        }
                    case "D6"://export
                        {
                            Console.WriteLine("Exporting...\n");
                            choice = 6;
                            break;
                        }
                }

            } while (choice != 6);

            conversationExporter.ExportConversation(configuration);

        }//end main method

        /// <summary>
        /// Helper method that prompts the user for console input.
        /// </summary>
        /// <param name="msg">
        /// The message to be displayed to the user
        /// </param>
        /// <returns>
        /// Returns User input
        /// </returns>
        public string PromptUserForInput(String msg)
        {
            string input = "";

            do
            {
                Console.WriteLine(msg);
                input = Console.ReadLine();

            } while (input.Length == 0);

            return input;
        }

        /// <summary>
        /// Exports the conversation using the <paramref name="configuration"/> as export configuration.
        /// </summary>
        /// <param name="configuration">
        /// The configuration which containg the inputFilePath, outputFilePath and various filters.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when a path is invalid.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown when something bad happens.
        /// </exception>
        public void ExportConversation(ConversationExporterConfiguration configuration)
        {
            Conversation conversation = this.ReadConversation(configuration);
            
            if ( conversation == null)
            {
                Console.WriteLine("Could not read conversation");
                Console.WriteLine("Press any key to continue...");
                Console.Read();
            }
            else
            {
                this.WriteConversation(conversation, configuration);
                Console.WriteLine("Conversation exported from '{0}' to '{1}'", configuration.inputFilePath, configuration.outputFilePath);
                Console.WriteLine("Press any key to continue...");
                Console.Read();
            }
            

        }

        /// <summary>
        /// Helper method to read the conversation from <paramref name="configuration"/>.
        /// </summary>
        /// <param name="configuration">
        /// The configuration which contains the inputFilePath
        /// </param>
        /// <returns>
        /// A <see cref="Conversation"/> model representing the conversation.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// User can provide a new path for coversation file
        /// </exception>
        /// <exception cref="Exception">
        /// User has to give new conversation file path
        /// </exception>
        public Conversation ReadConversation(ConversationExporterConfiguration configuration)
        {
            string inputFilePath = configuration.inputFilePath;
            try
            {
                var reader = new StreamReader(new FileStream(inputFilePath, FileMode.Open, FileAccess.Read),
                    Encoding.ASCII);

                string conversationName = reader.ReadLine();
                var messages = new List<Message>();

                string line;
                
                while ((line = reader.ReadLine()) != null)
                {
                    string timestamp ="";
                    string userID = "";
                    StringBuilder messageContent = new StringBuilder();
                    string []chatLine = line.Split(' ');
                    timestamp = chatLine[0].Substring(0, 10); //timestamps are only 10 length
                    userID = chatLine[1];
                    
                    for (int i = 2; i<chatLine.Length; i++)
                    {
                        messageContent.Append(chatLine[i]+" ");
                    }
                    messageContent.Remove(messageContent.Length-1,1);

                    Message msg = new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(timestamp)), userID.ToString(), messageContent.ToString());

                    if (configuration.filtersActive && checkUsername(configuration,msg) && checkKeyWord(configuration, msg))
                    {
                        if (configuration.obfuscateUserIDsFlag)
                        {
                            obfuscateUserIDs(configuration, msg);
                        }
                        hideSpecificWords(configuration, msg);
                        messages.Add( msg ); 
                    }
                    else if (!configuration.filtersActive)
                    {
                        if (configuration.obfuscateUserIDsFlag)
                        {
                            obfuscateUserIDs(configuration, msg);
                        }
                        hideSpecificWords(configuration, msg);
                        messages.Add(msg);
                    }

                }//end of while loop
                return new Conversation(conversationName, messages);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("The input conversation file was not found.");
                configuration.inputFilePath = PromptUserForInput("Please give new path for file.");
                return ReadConversation(configuration);
            }
            catch (Exception)
            {
                Console.WriteLine("Something went wrong in the IO.");
                configuration.inputFilePath = PromptUserForInput("Please provide a different conversation or fix chat file.");
                return ReadConversation(configuration);
            }

        }//end of read conversation

        /// <summary>
        /// Helper method to write the <paramref name="conversation"/> as JSON to outputFilePath in <paramref name="configuration"/>.
        /// </summary>
        /// <param name="conversation">
        /// The conversation.
        /// </param>
        /// <param name="configuration">
        /// The export configuration.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when there is a problem with the <paramref name="outputFilePath"/>.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown when something else bad happens.
        /// </exception>
        public void WriteConversation(Conversation conversation, ConversationExporterConfiguration configuration)
        {
            try
            {
                var writer = new StreamWriter(new FileStream(configuration.outputFilePath, FileMode.Create, FileAccess.ReadWrite));

                var serialized = JsonConvert.SerializeObject(conversation, Formatting.Indented);

                writer.Write(serialized);

                writer.Flush();

                writer.Close();
            }
            catch (SecurityException)
            {
                Console.WriteLine("No permission to file.");
                configuration.outputFilePath = PromptUserForInput("Please give new file path for output");
                WriteConversation(conversation, configuration);
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Path invalid.");
                configuration.outputFilePath = PromptUserForInput("Please give new file path for output");
                WriteConversation(conversation, configuration);
            }
            catch (Exception)
            {
                Console.WriteLine("Something went wrong in the IO.");
                configuration.outputFilePath = PromptUserForInput("Please give new file path for output ");
                WriteConversation(conversation, configuration);
            }
        }



        /// <summary>
        /// Helper method that checks if a <paramref name="msg"/> is sent by usernameFilter in <paramref name="configuration"/>
        /// </summary>
        /// <param name="configuration">
        /// The Export Configuration
        /// </param>
        /// <param name="msg">
        /// The message to be checked
        /// </param>
        /// <returns></returns>
        public bool checkUsername(ConversationExporterConfiguration configuration, Message msg)
        {
            //Check if username exists in the message
            if (configuration.usernameFilter != null && msg.senderId.ToLower().Equals(configuration.usernameFilter.ToLower()))
            {
                return true;
            }
            else if(configuration.usernameFilter == null)
            {
                return true;
            }
           
            return false;

        }//end of username check

        /// <summary>
        /// Helper method that checks if a <paramref name="msg"/> contains the specified keyword in <paramref name="configuration"/>
        /// </summary>
        /// <param name="configuration">
        /// The Export Configuration
        /// </param>
        /// <param name="msg">
        /// The message to be checked
        /// </param>
        /// <returns></returns>
        public bool checkKeyWord(ConversationExporterConfiguration configuration, Message msg)
        {
            //Check if keywork exists in the message
            if (configuration.keyword != null && msg.content.ToLower().Contains(configuration.keyword.ToLower()))
            {
                return true;
            }
            else if (configuration.keyword == null)
            {
                return true;
            }

            return false;
        }//end of keyword check

        /// <summary>
        /// Helper method that hides specific words in a <paramref name="msg"/> 
        /// </summary>
        /// <param name="configuration">
        /// The Export Configuration
        /// </param>
        /// <param name="msg">
        /// The message to be checked
        /// </param>
        /// <returns></returns>
        public void hideSpecificWords(ConversationExporterConfiguration configuration, Message msg)
        {
            string [] msgContent ;
            if (configuration.wordsBlacklist != null)
            {
                foreach (string wordToHide in configuration.wordsBlacklist)
                {
                    msgContent = msg.content.Split(' ' );

                    foreach(string word in msgContent)
                    {
                        if (word.ToLower().Contains(wordToHide.ToLower()))
                        {
                            string temp = word;
                            temp = temp.ToLower().Replace(wordToHide.ToLower(), "*redacted*");
                            msg.content = msg.content.Replace(word, temp);
                        }
                    }
                    
                }
                
            }
        }//end of hide specific words 



        /// <summary>
        /// Helper method to Obfuscate user IDs from <paramref name="msg"/> using the list from <paramref name="configuration"/>
        /// </summary>
        /// <param name="configuration">
        /// The Exporter's configuration
        /// </param>
        /// <param name="msg"> the message to obfuscate the user id </param>
        public void obfuscateUserIDs(ConversationExporterConfiguration configuration, Message msg)
        {
            if (configuration.usersMapper.ContainsKey(msg.senderId))
            {
                msg.senderId = configuration.usersMapper[msg.senderId];
            }
            else
            {
                int count = configuration.usersMapper.Count;
                configuration.usersMapper.Add(msg.senderId, "User" + (count + 1));
                msg.senderId = configuration.usersMapper[msg.senderId];
            }
        }
        
    }
}
