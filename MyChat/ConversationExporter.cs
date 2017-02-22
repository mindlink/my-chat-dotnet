namespace MyChat
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Security;
    using System.Text;
    using MindLink.Recruitment.MyChat;
    using Newtonsoft.Json;
    using System.Linq;

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
        /// 

        public List<string> bList = new List<string>();
        public List<string> logUsers = new List<string>();
        public List<ActiveUsers> activeUsers = new List<ActiveUsers>();


        static void Main(string[] args)
        {
            var conversationExporter = new ConversationExporter();
            ConversationExporterConfiguration configuration = new CommandLineArgumentParser().ParseCommandLineArguments(args);

            //Console.WriteLine(configuration.inputFilePath + " " + configuration.outputFilePath);
            conversationExporter.ExportConversation(configuration);
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
        public void ExportConversation(ConversationExporterConfiguration configuration)
        {
            Conversation conversation = this.ReadConversation(configuration.inputFilePath, configuration.user, configuration.keyboard, configuration.blacklist);

            this.WriteConversation(conversation, configuration.outputFilePath);

            Console.WriteLine("Conversation exported from '{0}' to '{1}'", configuration.inputFilePath, configuration.outputFilePath);
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
        public Conversation ReadConversation(string inputFilePath, string user, string keyword = "", string blacklist = "")
        {
            try
            {                

                var reader = new StreamReader(new FileStream(inputFilePath, FileMode.Open, FileAccess.Read),
                    Encoding.ASCII);

                string conversationName = reader.ReadLine();
                var messages = new List<Message>();

                string line;

                //check for black words
                if (blacklist != "")
                {
                    bList = blacklist.Split(new char[] { ',' }).ToList();
                }

                while ((line = reader.ReadLine()) != null)
                {
                    var split = line.Split(' ');//.Contains(user);
                    if (user != null)
                    {
                        if (split[1] != user)
                        {
                            continue;
                        }
                    }

                    //Combine the message
                    string message = String.Join(" ", split.Where((x, index) => index >= 2).ToArray());
                    if (keyword != null)
                    {
                        if (!message.ToLower().Contains(keyword.ToLower()))
                        {
                            continue;
                        }
                    }

                    if (bList.Count > 0)
                    {
                        //check if any black word exists in message
                        foreach(var word in bList)
                        {
                            message = message.ToLower().Replace(word.ToLower(), "*redacted*");
                        }
                    }

                    logUsers.Add(split[1]);
                    messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(split[0])), split[1], message));
                }

                //find out active users
                if (logUsers.Count > 0)
                {
                    List<string> uniqueUsers = logUsers.Distinct().ToList();

                    foreach(string us in uniqueUsers)
                    {
                        int count = logUsers.Where(s => s == us).Count();
                        ActiveUsers acUser = new ActiveUsers() { name = us, times = count };
                        activeUsers.Add(acUser);
               
                    }

                }

                return new Conversation(conversationName, messages, activeUsers[0].name, activeUsers);
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
