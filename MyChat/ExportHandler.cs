using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Security;
using MindLink.Recruitment.MyChat;
using Newtonsoft.Json;


namespace MindLink.Recruitment.MyChat
{
    /// <summary>
    /// This is the main class for 
    /// handling the export of a conversation 
    /// to a json file
    /// </summary>
    public class ExportHandler
    {
        /// <summary>
        ///The blaclisted words are replaced with the specific string       
        /// </summary>
        const string REDACTED = @"\*redacted\*";    
        
        /// <summary>
        /// An instance of the ExportCreteria class 
        /// that holds the export specifications of the user
        /// </summary>
        ExportCreteria exportCreteria;    
        
        /// <summary>
        /// A constructor for the ExportHandler     
        /// </summary>                    
        public ExportHandler(ExportCreteria exportCreteria)
        {
            this.exportCreteria = exportCreteria;
        }
        
        /// <summary>
        /// A function that redacts the exported messages
        /// </summary>
        public static void RedactMessages(ExportCreteria exportCreteria, IEnumerable<Message> messages)
        {
            string newMsg;
           
            List<string>.Enumerator blackListEnumerator;
            
            try
            {
                foreach (Message msg in messages)
                {
                    blackListEnumerator = exportCreteria.ReturnBlackListedWords().GetEnumerator();

                    while (blackListEnumerator.MoveNext())                     //The blacklisted words
                    {
                        if (Regex.IsMatch(msg.Content,"\\b"+blackListEnumerator.Current+"\\b",RegexOptions.IgnoreCase)) //if a match is found
                        {
                            newMsg = Regex.Replace(msg.Content,blackListEnumerator.Current, REDACTED,RegexOptions.IgnoreCase);  //replace the blacklisted word
                            msg.Content = newMsg;
                        }
           
                    }
                }
            }
            catch (NullReferenceException nullRefException)
            {
                Console.WriteLine("There has been an error regarding the defined black list");
                Console.WriteLine(nullRefException.Message);
            }
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
        public static void ExportConversation(ExportCreteria exportCreteria)
        {
            try
            {
                Conversation conversation = ReadConversation(exportCreteria);

                if (exportCreteria.ReturnBlackListedWords() != null)
                    RedactMessages(exportCreteria,conversation.Messages);

                WriteConversation(conversation, exportCreteria);

                Console.WriteLine("Conversation exported from '{0}' to '{1}'", exportCreteria.InputFilePath, exportCreteria.OutputFilePath);

            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException("File not found");
            }
            catch (IOException)
            {
                throw new IOException("There was a problem during the export operation");
            }
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
        public static Conversation ReadConversation(ExportCreteria exportCreteria)
        {
            //A string that holds the content of a message 
            string content = "";

            //A flag to include or not a specific message in case of filtering         
            Boolean addMessage = false;

            //Export conversation by the specific user   
            User user = exportCreteria.Export_by_User;

            //Export conversation by the specific keyword
            string keyword = exportCreteria.Export_by_Keyword;      
            int stringArrayIndex = 2;

            try
            {
                var reader = new StreamReader(new FileStream(exportCreteria.InputFilePath, FileMode.Open, FileAccess.Read),
                    Encoding.ASCII);

                string conversationName = reader.ReadLine();
                var messages = new List<Message>();

                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var split = line.Split(' ');

                    //export whole content , not just a snapsot of it
                    while (stringArrayIndex < split.Length)                
                    {
                        content += split[stringArrayIndex++];
                        if(stringArrayIndex < split.Length)
                            content += " ";
                    }

                    //user defined export by user AND keyword
                    if (user.Name != "" && keyword != "")                      
                    {  
                        //check if both creteria match  
                        if (user.Name.Equals(split[1], StringComparison.InvariantCultureIgnoreCase))
                            addMessage = Regex.IsMatch(content, "\\b" + keyword + "\\b", RegexOptions.IgnoreCase);
                    }

                    //user defined export by user
                    else if (user.Name != "")                                
                    {
                        if (user.Name.Equals(split[1], StringComparison.InvariantCultureIgnoreCase))
                            addMessage = true;
                    }

                    //user defined export by keyword
                    else if (keyword != "")                                 
                    {
                        addMessage = Regex.IsMatch(content, "\\b" + keyword + "\\b", RegexOptions.IgnoreCase);
                    }

                    //user did not defined anything so we export the whole dialog
                    else if (user.Name == "" && keyword == "")                    
                        addMessage = true;

                    //if we found a case match then we add the message to the message list
                    if (addMessage)                                          
                    {
                        //check the ''hide id'' flag
                        if (exportCreteria.HideUserName)
                        {
                            messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(split[0])), user.GetMd5Hash(user.MD5hash, split[1]), content));
                            
                            //Because the user id must be hidden we add it to the blacklisted words.
                            //That way the user id's is hidden from the messages
                            exportCreteria.ReturnBlackListedWords().Add(split[1]); 
                        }
                        else
                            messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(split[0])), split[1], content));
                    }
                    //in case we did not find any match with the specific filters
                    addMessage = false;                                    
                    content = null;
                    stringArrayIndex = 2;
                }

                return new Conversation(conversationName, messages);
            }
            catch(FileNotFoundException)
            {
                throw new FileNotFoundException("The file specified was not found");
            }
            catch (IOException)
            {
                throw new IOException("Something went wrong in the IO.");
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException("There was an error encrypting user id's");
            }

        }

        /// <summary>
        /// A function that '' exports '' the users of a conversation to a user list
        /// </summary>
        public static List<User> ReturnUserList(Conversation conversation)
        {
            List<User> userList = new List<User>();

            //users are exported from the conversation
            foreach (Message msg in conversation.Messages)         
            {
                //A flag to include or not the specific user
                bool addUser = true;

                //init of a user instance
                User userFromMessageList = new User(msg.SenderId);

                //if the user list is empty 
                //add the first user found
                if (userList.Count == 0)                            
                {
                    userList.Add(userFromMessageList);              
                }

                foreach (User userFromList in userList)             
                {
                    //if the user is allready in the list
                    if (userFromList.Name.Equals(userFromMessageList.Name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        //just increase his message activity
                        userFromList.NumberOfMessages++;            
                        addUser = false;
                    }

                }
                //if the user does not belong to the list
                if (addUser)                                        
                {
                    //increase his message activity
                    userFromMessageList.NumberOfMessages++;
                    //add him to the user list
                    userList.Add(userFromMessageList);              
                }
            }
            return userList;
        }
        
        /// <summary>
        /// A function that arranges user list by user activity
        /// </summary>
        public static List<User> ArangeUsersByActivity(List<User> userList)
        {

            try
            {
                //using linq to arrange list
                List<User> orderedList = userList.OrderByDescending(x => x.NumberOfMessages).ToList(); 
                
                return orderedList;
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("There was an error arranging user list"); 
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
        public static void WriteConversation(Conversation conversation, ExportCreteria exportCreteria)
        {
            try
            {
                var writer = new StreamWriter(new FileStream(exportCreteria.OutputFilePath, FileMode.Create, FileAccess.ReadWrite));

                var serialized = JsonConvert.SerializeObject(conversation, Formatting.Indented);

                if (exportCreteria.IncludeReport)
                {
                    var report = ReturnUserList(conversation).Select(d => string.Format("User id:{0}, Messages:{1}", d.Name, string.Join(",", d.NumberOfMessages)));
         
                    var serializedReport = JsonConvert.SerializeObject(report,Formatting.Indented);

                    writer.Write(serialized + serializedReport);
                }
                else
                {
                    writer.Write(serialized);
                }

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
