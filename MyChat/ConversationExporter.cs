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
    using Newtonsoft.Json.Linq;

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
            conversationExporter.FindUser(c, configuration.user);
            conversationExporter.SearchWord(c, configuration.user);
            conversationExporter.RedactWord(c, configuration.blacklistPath, configuration.redactedConversationPath);


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
            //try
            //{
            var messagez = new List<Message>();
            string[] linez = File.ReadAllLines(inputFilePath, Encoding.UTF8);
            //  int c = linez.Count();
            var reader = new StreamReader(new FileStream(inputFilePath, FileMode.Open, FileAccess.Read),
                   Encoding.ASCII);
            //  int xa = 1;
            // string conversationName = linez[0];
            //string linez;
            string conversationName = reader.ReadLine();

            foreach (var line in linez)
            {
                //creates three different groups of patterns for each part of the object - timestamp,senderrId,content
                string rx = @"(\d{10})\s(\w{2,10})\s(.*)";

                MatchCollection matches = Regex.Matches(line, rx);
                foreach (Match match in matches)
                {
                    Console.WriteLine(match.Groups[3].Value);
                    messagez.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(match.Groups[1].Value)), match.Groups[2].Value, match.Groups[3].Value)); //doesn't add the messages to the object and as a result - null value and breaks the code
                    Conversation qw = new Conversation(conversationName, messagez);
                    foreach (var io in qw.messages)
                    {
                        Console.WriteLine(io.content);
                    }
                }

                //foreach (Match match in matches)
                //{
                //    // Console.WriteLine(match.Groups[3].Value); //prints out the messsages
                //    messagez.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(match.Groups[1].Value)), match.Groups[2].Value,match.Groups[3].Value)); //doesn't add the messages to the object and as a result - null value and breaks the code


                //}



            }

            //not working try - to be deleted next commit

            //int j = xa++;
            //for (int y = 1; y < linez.Length; y++)
            //{
            //    string splitLines = linez[y];
            //    string[] splitoL = splitLines.Split(' ');
            //    int countSpaces = splitLines.Count(Char.IsWhiteSpace);

            //    for (int i = 4; i <= countSpaces; i++)
            //    {
            //        string xz = string.Concat(splitoL[countSpaces - 2], " ", splitoL[countSpaces - 1], " ", splitoL[countSpaces]);

            //       
            //    }

            //}


            //try
            //{





            //create array -each item is a message for each new line 
            //string[] messagesArray = new string[7];

            //  //for line 1, split by space,count number of items in the array, join the items taht are message in a string.Add teh string to the array
            //  string[] split = linez[1].Split(' ');
            //  int x = split.Count();


            //for (int k = 3; k <= x; k++)
            //{


            //    string h = String.Join(split[k - 1], split[k]);
            //    messagesArray[0] = h;


            //}

            //string[] splitTwo = linez[2].Split(' ');
            //int x2 = splitTwo.Count();
            //for (int k1 = 3; k1 <= x2; k1++)
            //{
            //    string h1 = String.Join(" ", splitTwo[k1 - 1]);
            //    messagesArray[1] = h1;
            //}
            //string[] splitThree = linez[3].Split(' ');
            //int x3 = splitThree.Count();
            //for (int k2 = 3; k2 <= x3; k2++)
            //{
            //    string h2 = String.Join(" ", splitThree[k2 - 1]);

            //    messagesArray[2] = h2;
            //}

            //string[] splitFive = linez[5].Split(' ');
            //int x4 = splitFive.Count();
            //for (int k3 = 3; k3 <= x4; k3++)
            //{
            //    string h3 = String.Join(" ", splitFive[k3 - 1]);

            //    messagesArray[3] = h3;
            //}
            //string[] splitSix = linez[6].Split(' ');
            //int x5 = splitSix.Count();
            //for (int k4 = 3; k4 <= x5; k4++)
            //{
            //    string h4 = String.Join(" ", splitSix[k4 - 1]);


            return new Conversation(conversationName, messagez); //not recognised by the method if try/catch is on 

            }


           


            //catch (FileNotFoundException)
            //{
            //    throw new ArgumentException("The file was not found.");
            //}
            //catch (IOException)
            //{
            //    throw new Exception("Something went wrong in the IO.");
            //}
            //catch (System.IndexOutOfRangeException)
            //{
            //    throw new ArgumentException("Outside the bounds of the array");
            //}
            //catch (RankException)
            //{
            //    Console.WriteLine("Array with wrong number of items is passed to the method");
            //}

            //catch (EndOfStreamException)
            //{
            //    Console.WriteLine("Trying to read past the ecnd of the file");
            //}
            //catch (FileLoadException)
            //{
            //    Console.WriteLine("File cannot load");
            //}

       // }


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
        /// <summary>
        /// Helper method to write the <paramref name="userFind "/> as JSON to <paramref name="path"/>.
        /// </summary>
        /// <param name="Userfind method">
        /// The  find conversation that has been written by  the user method.
        /// </param>
        /// <param name="path">
        /// The output file path - new folder called Userconversation.txt.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when there is a problem with the <paramref name="path"/>.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown when something else bad happens.
        /// </exception>
        public void FindUser(Conversation conversation, string user)
        {
            try
            {
                var serialized = JsonConvert.SerializeObject(conversation, Formatting.Indented);
                //deserialise object
                Conversation deserialisedChato = JsonConvert.DeserializeObject<Conversation>(serialized);
                Console.WriteLine(deserialisedChato.messages);
                //loop through messages
                foreach (var value in deserialisedChato.messages)
                {
                    //if the sender is the same as teh command line argument,convert to json,write the result to file - values are multiplied because the previous method Read is not  showing everything(to be redacted)
                    //if (value.SenderId == user)
                    //{
                    var result = value.content;

                    //convert to json
                    JObject convertToJson =
                                                 new JObject(
                                                     new JProperty("Result",
                                                     new JObject(
                                                          new JProperty("User", value.senderId),
                                                             new JProperty("Message", value.content))));

                    string path = Environment.CurrentDirectory + "\\" + "userConversation.txt";
                    //write to file
                    System.IO.File.AppendAllText(path, convertToJson.ToString());
                    // }
                    //else
                    //{
                    //    Console.WriteLine("a user has not been found");
                    //}
                }
            }
            catch (DirectoryNotFoundException)
            {
                throw new ArgumentException("Path invalid.");
            }
            catch (FormatException)

            {

                throw new ArgumentException("String for user was not in the correct format");

            }
            catch (SecurityException)
            {
                throw new ArgumentException("No permission to file.");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File is not found");
            }
            catch (FieldAccessException)
            {
                Console.WriteLine("Trying to access a private or protected field");
            }
            catch (EndOfStreamException)
            {
                Console.WriteLine("Trying to read past the ecnd of the file");
            }
            catch (FileLoadException)
            {
                Console.WriteLine("File cannot load");
            }

        }

        /// <summary>
        /// Helper method to write the <paramref name="SearchWord "/> as JSON to <paramref name="path"/>.
        /// </summary>
        /// <param name="SearchWord method">
        /// Find a conversation that has the word specified as a command -line argument
        /// </param>
        /// <param name="path">
        /// The output file path - new folder called userConversation.txt.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when there is a problem with the <paramref name="path"/>.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown when something else bad happens.
        /// </exception>
        public void SearchWord(Conversation conversation, string word)
        {
            try
            {
                Console.WriteLine(conversation.name);
                var serialized = JsonConvert.SerializeObject(conversation, Formatting.Indented);
                //deserialise object
                Conversation deserialisedChat = JsonConvert.DeserializeObject<Conversation>(serialized);
                //LINQ - if the messages contain the word from the command-line argument,select the messages
                var messageList = from x in deserialisedChat.messages
                                  where x.content.Contains(word)
                                  select x;
                //build them into strings
                StringBuilder sb = new StringBuilder();
                foreach (Message x in messageList)
                {
                    if (x.content.Contains(word))
                    {
                        sb.Append("UserName: " + x.senderId + "Message: " + x.content);
                        //convert to json
                        JObject convertWordToJson =
                      new JObject(
                          new JProperty("Result",
                          new JObject(
                               new JProperty("User", x.senderId),
                                  new JProperty("Message", x.content))));
                        string path = Environment.CurrentDirectory + "\\" + "userConversation.txt";
                        //write to file
                        System.IO.File.AppendAllText(path, convertWordToJson.ToString());
                    }
                    else
                    {
                        Console.WriteLine(" The word  is not found in any conversation");
                    }
                }

            }
            catch (DirectoryNotFoundException)
            {
                throw new ArgumentException("Path invalid.");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Wrong arguments are provided,Please check your order of argumnets");
            }
            catch (NullReferenceException)
            {
                Console.WriteLine(" A null object is being referenced");
            }
            catch (SecurityException)
            {
                throw new ArgumentException("No permission to file.");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File is not found");
            }
            catch (FieldAccessException)
            {
                Console.WriteLine("Trying to access a private or protected field");
            }
            catch (EndOfStreamException)
            {
                Console.WriteLine("Trying to read past the ecnd of the file");
            }
            catch (FileLoadException)
            {
                Console.WriteLine("File cannot load");
            }

        }

        /// <summary>
        /// Helper method to write the <paramref name=RedactWord "/>  to <paramref name="RedactConversationPath"/>.
        /// </summary>
        /// <param name="RedactWord method">
        /// Find a conversation that has the word specified from blacklist and redact it
        /// </param>
        /// <param name="path">
        /// The output file path - new file called redactConversation.txt.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when there is a problem with the <paramref name="path"/>.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown when something else bad happens.
        /// </exception>
        public void RedactWord(Conversation conversation, string blacklistPath, string redactedConversationPath)
        {
            try
            {
                //create a list that holds the blacklist words
                List<string> blacklist = new List<string>();
                // read the blacklist file
                string[] lines = System.IO.File.ReadAllLines(blacklistPath);
                //add words to the list
                foreach (string line in lines)
                {
                    blacklist.Add(line);

                }
                var serialized = JsonConvert.SerializeObject(conversation, Formatting.Indented);
                //deserialise object
                Conversation deserialisedChat = JsonConvert.DeserializeObject<Conversation>(serialized);
                //loop through blacklist,check if it contains bad word
                foreach (string blacklistWord in blacklist)
                {

                    var listRedacted = from j in deserialisedChat.messages
                                       where j.content.Contains(blacklistWord)
                                       select j;
                    //check if listRedacted does not contain any blacklisted words - needs to be better
                    //if (listRedacted != null)
                    //{
                    StringBuilder sb2 = new StringBuilder();
                    foreach (Message toBeRedacted in listRedacted)
                    {
                        sb2.Append("UserName: " + toBeRedacted.senderId + "Message: " + toBeRedacted.content);
                        // var r = sb3.Replace(blacklistWord, "*redacted*");
                        //Console.WriteLine(r);
                        System.IO.File.AppendAllText(redactedConversationPath, sb2.Replace(blacklistWord, "*redacted*").ToString());
                    }
                    // }
                    //else    // breaks
                    //    {
                    //        Console.WriteLine("No words on the blacklist in the conversation");
                    //    }
                }

            }
            catch (DirectoryNotFoundException)
            {
                throw new ArgumentException("Path invalid.");
            }
            catch (SecurityException)
            {
                throw new ArgumentException("No permission to file.");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Wrong arguments are provided,Please check your order of argumnets");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File is not found");
            }
            catch (FieldAccessException)
            {
                Console.WriteLine("Trying to access a private or protected field");
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Trying to reference a null object");
            }
            catch (EndOfStreamException)
            {
                Console.WriteLine("Trying to read past the ecnd of the file");
            }
            catch (FileLoadException)
            {
                Console.WriteLine("File cannot load");
            }

        }

        /// <summary> needs more work
        /// Helper method to write the <paramref name=RedactCard "/>  to <paramref name="RedactConversationPath"/>.
        /// </summary>
        /// <param name="RedactCard method">
        /// Find a card/phone number in teh conversationt and redact it
        /// </param>
        /// <param name="path">
        /// The output file path - new file called redactConversation.txt.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when there is a problem with the <paramref name="path"/>.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown when something else bad happens.
        /// </exception>

        //public void RedactCard(Conversation conversation, string blacklistPath, string redactedConversationPath) - not fully working
        //{
        //    try
        //    {
        //        //bool flag = true;

        //        ////source for phone number https://stackoverflow.com/questions/25155970/validating-uk-phone-number-regex-c
        //        //var patterns = new string[] { @"\d{4} \d{4} \d{4} \d{4}", @"\d{16}", @"\d{4}-\d{4}-\d{4}-\d{4}", @"^(((\+44\s?\d{4}|\(?0\d{4}\)?)\s?\d{3}\s?\d{3})|((\+44\s?\d{3}|\(?0\d{3}\)?)\s?\d{3}\s?\d{4})|((\+44\s?\d{2}|\(?0\d{2}\)?)\s?\d{4}\s?\d{4}))(\s?\#(\d{4}|\d{3}))?$" };
        //        //var rgPattern = new Regex(string.Join("|", patterns), RegexOptions.IgnoreCase);

        //        var serialized = JsonConvert.SerializeObject(conversation, Formatting.Indented);
        //        //deserialise object
        //        Conversation deserialisedChat = JsonConvert.DeserializeObject<Conversation>(serialized);
        //        //    MatchCollection creditCard = rgPattern.Matches(args[6]);
        //        //    for (int count = 0; count < creditCard.Count; count++)
        //        //    {

        //        //        foreach (var file in deserialisedChat)
        //        //        {
        //        //            var list4 = from y in seserialisedChat

        //        //                        select y.messages;
        //        //            if (rgPattern.IsMatch(list4.ToString()))
        //        //            {

        //        //                Console.WriteLine("loooo");
        //        //            }
        //        //            //var listRedacted = from k in RedactCardList
        //        //            //                   where k.content
        //        //            //                   select k;

        //        //            //                                   select y;
        //        //            // Console.WriteLine(creditCard[count].Value);
        //        //        }
        //    }
        //    catch (DirectoryNotFoundException)
        //    {
        //        throw new ArgumentException("Path invalid.");
        //    }

        //}
    }
}
    
