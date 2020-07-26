namespace MyChat
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security;
    using System.Text;
    using System.Text.RegularExpressions;
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

            /// <summary>
            /// The conversation is read, input file is specified as chat.txt
            /// </summary>
            /// <param name="c">
            /// the returned object from ReadConversation c is used in WriteConversation
            /// </param>
            ///  /// <summary>
            /// The conversation is exported as json
            /// </summary>
            conversationExporter.ReadConversation(configuration.inputFilePath);
           Conversation c = conversationExporter.ReadConversation(configuration.inputFilePath);
            conversationExporter.WriteConversation(c, configuration.outputFilePath);
            conversationExporter.ExportConversation(configuration.inputFilePath, configuration.outputFilePath);

            /// <summary>
            /// My tries to understand the code and how to use WriteConversation method as well as input conversation, to be deleted in next commit
            /// </summary>

            // DateTimeOffset dateOffset1;
            //   dateOffset1 = DateTimeOffset.Now;
            // Message m = new Message(dateOffset1, "x","y");
            // List<Message> list = new List<Message>();
            // list.Add(m);
            // IEnumerable<Message> en = list;
            // Conversation c = new Conversation("name",en);
            //conversationExporter.WriteConversation(new Conversation(ReadConversation(configuration.inputFilePath), configuration.outputFilePath);
            //conversationExporter.ExportConversation(configuration.inputFilePath, configuration.outputFilePath);
            // configuration.inputFilePath = @"C:\Users\UseR\source\repos\Chat\searchuser.txt";
            //Directory.GetCurrentDirectory() + "netcoreapp3.1"+ "chat.txt";
        }

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
        public void ExportConversation(string inputFilePath, string outputFilePath)
        {
           
            Conversation conversation = this.ReadConversation(inputFilePath);
            
            this.WriteConversation(conversation, outputFilePath);

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
        public Conversation ReadConversation(string inputFilePath)
        {
            try
            {
               
              var messages = new List<Message>();
                string[] linez = File.ReadAllLines(inputFilePath, Encoding.UTF8);
                int c = linez.Count();
              
                int xa = 1;
                string conversationName = linez[0];
              
                    int j = xa++;
                    for(int k=1; k<linez.Length; k++)
                    {
                        string splitLines = linez[k];
                        string[] splitoL = splitLines.Split(' ');
                        int countSpaces = splitLines.Count(Char.IsWhiteSpace);
                   
                    for (int i = 4; i <= countSpaces; i++)
                    {
                            string x = string.Concat(splitoL[countSpaces - 2]," ",splitoL[countSpaces - 1]," ",splitoL[countSpaces]);
                             
                            messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(splitoL[0])), splitoL[1], x));
                    }
                  
                
             //tried string builder

                //while ((line = reader.ReadLine()) != null)
                //{
                //    string[] splito = line.Split(' ');
                //    StringBuilder sb = new StringBuilder();
                //  //  string x = sb.Append(splito[2], splito[3]);
                        

                //        Console.WriteLine(splito[0]);

                //    }
                   

                    //try regex for space and end lines
                    //string[] split = Regex.Split(line, @"(?<=[\.!\?])\s+");
                   
                       //string[] split = line.Split(' ');
                    // StringSplitOptions.RemoveEmptyEntries);
                 //   Console.WriteLine("***");
                 //   Console.WriteLine(split[2] + split[3]);
                  //  Console.WriteLine(split[2].Concat(split[3]);
                 

                        // }
                        //string x = split[i];
                        //Console.WriteLine();

                        // Console.WriteLine(messages);

                      

                    }

                   
               
                 return  new Conversation(conversationName, messages);
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
    }
}
