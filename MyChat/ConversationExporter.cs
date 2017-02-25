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
    using System.Threading.Tasks;
    using System.Threading;

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

        public static List<string> bList = new List<string>();
        public static List<string> logUsers = new List<string>();
        public List<ActiveUsers> activeUsers = new List<ActiveUsers>();
        public static ConversationExporter conversationExporter = new ConversationExporter();
        public List<Message> messageList = new List<Message>();
        static void Main(string[] args)
        {
            ConversationExporterConfiguration configuration = new CommandLineArgumentParser().ParseCommandLineArguments(args);

            conversationExporter.ExportConversation(configuration);
            
        }

        /*stammary>
        /// <param name="inputFilePath">
        /// The statictic async void SendAsychTask(ConversationExporterConfiguration configuration)
        {
            await Task.Run(() => 
        }*/

        /// <summary>
        /// Exports the conversation at <paramref name="inputFilePath"/> as JSON to <paramref name="outputFilePath"/>.
        /// </suinput file path.
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
            Task n = Task.Run(() => ReadConversation(configuration.inputFilePath, configuration.outputFilePath, configuration.user, configuration.keyboard, configuration.blacklist));
            n.Wait();
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
        public async Task<Conversation> ReadConversation(string inputFilePath, string outputFilePath, string user, string keyword = "", string blacklist = "")
        {
            StreamReader reader = null;
            Conversation conversation;

            reader = HandleFileAsync(inputFilePath);

            Task< Conversation > task =  getLines(reader, user, keyword, blacklist);
            conversation = await task;


            //find out active users
            messageActiveUsers mActiveUsers =  this.findActive(conversation);

            this.WriteConversation(conversation, mActiveUsers, outputFilePath);

            Console.WriteLine("Conversation exported from '{0}' to '{1}'", inputFilePath, outputFilePath);

            return conversation;

           
        }

        static async Task<Conversation> getLines(StreamReader reader, string user, string keyword = "", string blacklist = "")
        {
            string conversationName = reader.ReadLine();
            Conversation conversation;

            var messages = new List<Message>();

            string line;

            //check for black words
            if (blacklist != "")
            {
                bList = blacklist.Split(new char[] { ',' }).ToList();
            }

           
            while ((line = await reader.ReadLineAsync()) != null)
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
                    foreach (var word in bList)
                    {
                        message = message.ToLower().Replace(word.ToLower(), "*redacted*");
                    }
                }

                logUsers.Add(split[1]);
                messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(split[0])), split[1], message));
            }
            conversation = new Conversation(conversationName, messages);

            return conversation;
        }

        public StreamReader HandleFileAsync(string inputFilePath)
        {

            FileStream input = null;
            StreamReader reader = null;

            try
            {
                input = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read);

            }
            catch (FileNotFoundException file)
            {
                throw new Exception("File not found");

            }

            //check if file is empty
            if (input.Length < 10)
                throw new Exception("File cannot be empty");

            //check file size
            if (input.Length > 104857600)
                throw new Exception("File should be smaller than 100MB");

            try
            {
                reader = new StreamReader(input, Encoding.ASCII);
            }
            catch (Exception e)
            {

                throw new Exception("Something went wrong in the IO.");
            }

            return reader;
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
        public void WriteConversation(Conversation conversation, messageActiveUsers mActive, string outputFilePath)
        {
            StreamWriter writer = null;

            try
            {
                writer = new StreamWriter(new FileStream(outputFilePath, FileMode.Create, FileAccess.ReadWrite));
            }            
            catch (SecurityException)
            {
                throw new ArgumentException("No permission to file.");
            }
            catch (DirectoryNotFoundException)
            {
                throw new ArgumentException("Path invalid.");
            }

            try
            {
                Output output = new Output()
                { conversation = conversation, mActiveUsers = mActive };
                var serialized = JsonConvert.SerializeObject(output, Formatting.Indented);
                               
                writer.Write(serialized);
          
                writer.Flush();

                writer.Close();

            }
            catch (IOException)
            {
                throw new Exception("Something went wrong in the IO.");
            }
        }

        private messageActiveUsers findActive(Conversation conversation)
        {
            messageActiveUsers mActive = new messageActiveUsers();

            if (logUsers.Count > 0)
               {
                   List<string> uniqueUsers = logUsers.Distinct().ToList();

                   foreach(string us in uniqueUsers)
                   {
                       int count = logUsers.Where(s => s == us).Count();
                       ActiveUsers acUser = new ActiveUsers() { name = us, times = count };
                       activeUsers.Add(acUser);

                   }

                mActive.mostActiveUser = activeUsers[0].name;
                mActive.activeUsers = activeUsers;

               }

            return mActive;
        }
    }
}
