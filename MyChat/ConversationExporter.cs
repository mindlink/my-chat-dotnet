using System.CodeDom;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

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
    public class ConversationExporter
    {
        /// <summary>
        /// The application entry point.
        /// </summary>
        /// <param name="args">
        /// The command line arguments.
        /// </param>
        static void Main(string[] args)
        {

            //Display general information for usage of tool.
            string screen = System.IO.File.ReadAllText(@".\InitialScreen.txt");
            Console.WriteLine(screen);

            args = Console.ReadLine().Split(' ');
            if (args.Length == 0) return;

            var conversationExporter = new ConversationExporter();
            ConversationExporterConfiguration configuration = new CommandLineArgumentParser().ParseCommandLineArguments(args);

            //Checks for specific formatting of command and exports the relevant output to json file.
            if (args.Length >= 3)
            {
                if ( configuration.blacklistedStrings != null && configuration.blacklistedStrings.Any() || configuration.RemoveSensitiveInfo )
                {
                 conversationExporter.ExportConversation(configuration.inputFilePath, configuration.outputFilePath, configuration.blacklistedStrings, configuration.RemoveSensitiveInfo);
                }
                if (args.Length.Equals(3) && args.Last() == "-Sensitive+OBF")
                {
                    configuration.ObfuscateSenders = true;
                    conversationExporter.ExportConversation(configuration.inputFilePath, configuration.outputFilePath,
                        configuration.ObfuscateSenders);
                }
                else if(configuration.Filter != null && configuration.Filter.Equals(args[2]))
                    conversationExporter.ExportConversation(configuration.inputFilePath, configuration.outputFilePath,
                        configuration.Filter);
            }
            else
            conversationExporter.ExportConversation(configuration.inputFilePath, configuration.outputFilePath);
        }

        /// <summary>
        /// The filtering method for messages. It filters messages by senderId or by specific word.
        /// </summary>
        /// <param name="conv">
        /// The input messages conversation to filter.
        /// </param>
        /// <param name="filtVal">
        /// The value by which the filtering takes place.
        /// </param>
        /// <returns> Either <see cref="Conversation"/>a messages list filtered by name or messages filtered by keyword.</returns>
        public IEnumerable<Message> FilteredBy(Conversation conv, string filtVal)
        {
            if (conv.messages.Any(m => m.senderId == filtVal))
            //Return messages based on conversation sender name.
            return conv.messages.Where(msg => msg.senderId == filtVal);

            //Ignore casing when searching for specific strings in conversation content.
            return
                conv.messages.Where(message => message.content.IndexOf(filtVal, StringComparison.OrdinalIgnoreCase) >= 0);
        }



        /// <summary>
        /// The method for replacing bad words in the content of conversation messages.
        /// </summary>
        /// <param name="badConversation">
        /// The input of the conversation for bad words redaction.
        /// </param>
        /// <param name="badStrings">
        /// The list of censored words for removal.
        /// </param>
        /// <param name="sensitive">
        /// The option to remove also credit card numbers or phone numbers.
        /// </param>
        /// <returns> The modified content with the relevant bad words redaction.</returns>
        public IEnumerable<Message> RedactBadWords(Conversation badConversation, string[] badStrings, bool sensitive)
        {
            Regex checkForRedactions = new Regex(PrepareEvalString());
            var allMsgs = badConversation.messages.Select(badwordMsg => badwordMsg).ToList();
            List<Message> badList = new List<Message>();
            int e = 0;

            //Removal of all censored words occurs here.
            //This is used with Regex for phone numbers etc.
            do
            {
                badList = allMsgs.Select(msg =>
                {
                    //Use either option for content filtering 
                    msg.content = sensitive
                        ? checkForRedactions.Replace(msg.content, "*redacted*")
                        : msg.content.Replace(badStrings.ElementAt(e), "*redacted*");

                    //Use both options for sensitive info and blacklisted words.
                    if (sensitive && badStrings.Length > 0)
                    {
                         msg.content = checkForRedactions.Replace(msg.content, "*redacted*");
                         msg.content = msg.content.Replace(badStrings.ElementAt(e), "*redacted*");
                    }
                    return msg;
                }).ToList();

                e++;
            } while (e < badStrings.Length);
           return badList.Select(redacted => redacted).AsEnumerable();
        }

        /// <summary>
        /// Todo: Option for further content redaction for credit cards and phone numbers. - DONE
        /// Redact specific content such as credit card numbers or phone numbers.
        /// 
        /// Currently supported formats:
        /// ----------------------------
        /// Phone Number Styles:
        /// (xxx)xxx-xxxx
        /// xxxxxxxxxx
        /// xxx xxx xxxx
        /// xxx.xxx.xxxx
        /// +xxxxxxxxxx
        /// xxxxxxxx
        /// +xxx xxx xxxx
        /// +xxx-xxx-xxxx
        /// 
        /// Credit Card Styles:
        /// xxxx-xxxx-xxxx-xxxx
        /// xxxx/xxxx/xxxx/xxxx
        /// xxxx.xxxx.xxxx.xxxx
        /// xxxx xxxx xxxx xxxx
        /// xxxx_xxxx_xxxx_xxxx
        /// 
        /// Currently not supported:
        /// ------------------------
        /// - Email addresses
        /// - Invalid phone numbers
        /// - IP addresses
        /// ...
        /// </summary>
        /// <returns> The full regular expression used for content filtering as a single string.</returns>
        public string PrepareEvalString()
        {
            //Evaluation Regex string for matching  credit card info or phones.
            const string evaluator =
                @"[0-9]{4}(-|\/|.|_|[ ])[0-9]{4}(-|\/|.|_|[ ])[0-9]{4}(-|\/|.|_|[ ])[0-9]{4}|(\([0-9]{3}\))[0-9]{3}(-)[0-9]{4}|(\+)[0-9]{8,15}([ ]|(.)|(-))[0-9]{3}([ ]|(.)|(-))[0-9]{4}|[0-9]{10}|[0-9]{8}|(\+)[0-9]{10}|[0-9]{3}([ ]|(.))[0-9]{3}([ ]|(.))[0-9]{4}";
            return evaluator;
        }
        

        /// <summary>
        /// Todo: Option for generating hidden names for senderIds. - DONE
        /// Obfuscate the senderIds of the conversation file for exporting.
        /// </summary>
        /// <param name="senderId">
        /// The senderId to be used for obfuscation.
        /// </param>
        /// <returns> A randomized string of 10 characters representing each sender Id.</returns>
        public string ObfuscateIds(string senderId)
        {
            //Use instead of TickCount() to get the same senderId values at each export - Lessen randomness on output.
            //const int addToSeed = 3893;
            int addToSeed = 0;
            for (int i = 0; i < senderId.Length; i++)
               addToSeed += senderId.Select(c => c).ElementAt(i);

            Random randChar = new Random(addToSeed + System.Environment.TickCount);

            StringBuilder sbUIdBuilder = new StringBuilder();
            const string charSet = "abcedfgihkljmnorpqstuvwxzy84932104576";

            //Select one-by-one random character from a fixed string and return it as a whole string.
            for (int i = 0; i < 10; i++)
              senderId =  sbUIdBuilder.Append(charSet.ElementAt(randChar.Next(0, charSet.Length))).ToString();
            return senderId;
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
            Conversation conversation = this.ReadConversation(inputFilePath, false);

            this.WriteConversation(conversation, outputFilePath);

            Console.WriteLine("Conversation exported from '{0}' to '{1}'", inputFilePath, outputFilePath);
            Console.ReadLine();
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
        /// <param name="Filter">
        /// The filtering option for the output file's data.
        /// </param>
        public void ExportConversation(string inputFilePath, string outputFilePath, string Filter)
        {
            Conversation conversation = this.ReadConversation(inputFilePath, false);
            conversation.messages = FilteredBy(conversation, Filter);

            this.WriteConversation(conversation, outputFilePath);

            Console.WriteLine("Conversation exported from '{0}' to '{1}', filtered by name: '{2}'", inputFilePath, outputFilePath, Filter);
            Console.ReadLine();
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
        /// <param name="blacklistedStrings">
        /// The filtering option for the output file's bad words removal.
        /// </param>
        /// <param name="sensitiveInfo">
        /// Including/Excluding sensitive info such as credit card numbers or phone numbers.
        /// </param>
        public void ExportConversation(string inputFilePath, string outputFilePath, string[] blacklistedStrings, bool sensitiveInfo)
        {
            Conversation conversation = this.ReadConversation(inputFilePath, false);
            conversation.messages = RedactBadWords(conversation, blacklistedStrings,sensitiveInfo);

            this.WriteConversation(conversation, outputFilePath);

            Console.WriteLine("Conversation exported from '{0}' to '{1}' - *BAD WORDS REDACTED OUTPUT*", inputFilePath, outputFilePath);
            Console.ReadLine();
            //Environment.Exit(222);
        }

        /// <summary>
        /// Method used for exporting obfuscated senderIds on output. This does not support also the previous options.
        /// Such as filtering, word redactions etc.
        /// </summary>
        /// <param name="inputFilePath">
        /// The input file path.
        /// </param>
        /// <param name="outputFilePath">
        /// The output file path.
        /// </param>
        /// <param name="forObfuscation">
        /// The obfuscating option to hide all senderIds in the exported file.
        /// </param>
        public void ExportConversation(string inputFilePath, string outputFilePath, bool forObfuscation)
        {
            Conversation conversation =  forObfuscation ? this.ReadConversation(inputFilePath, true) : this.ReadConversation(inputFilePath, false);

            this.WriteConversation(conversation, outputFilePath);

            Console.WriteLine("Conversation exported from '{0}' to '{1}' - **Sender IDs Obfuscated**", inputFilePath, outputFilePath);
            Console.ReadLine();

        }

        /// <summary>
        /// Todo: Appending the rest of the strings in the completed message. - DONE
        /// Method used for appending the elements of a message per line.
        /// </summary>
        /// <param name="strEnum">
        /// The enumerator of the list of strings per message.
        /// </param>
        /// <returns>The completed string of the line for each message</returns>
        public string AppendMessageAsString(IEnumerator strEnum)
        {
            StringBuilder sbMessage = new StringBuilder();
            while (strEnum.MoveNext())
            {
                sbMessage.AppendFormat("{0} ",strEnum.Current);
            }
            return sbMessage.ToString().TrimEnd(' ');
        }

        /// <summary>
        /// Todo: Fix exporting a conversation in json format. - DONE
        /// Helper method to read the conversation from <paramref name="inputFilePath"/>.
        /// </summary>
        /// <param name="inputFilePath">
        /// The input file path.
        /// </param>
        /// <param name="obfuscate">
        /// Option for hiding userIds - Enabled/Disabled from the command line provided arguments.
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
        public Conversation ReadConversation(string inputFilePath, bool obfuscate)
        {
            try
            {
                var reader = new StreamReader(new FileStream(inputFilePath, FileMode.Open, FileAccess.Read),
                    Encoding.ASCII);

                string conversationName = reader.ReadLine();
                var messages = new List<Message>();

                string line;
                //Regex for validating input..
                Regex bracketChecker = new Regex(@"[(]|(\)\*)");

                Start:
                while ((line = reader.ReadLine()) != null)
                {
                    //This regex check is used for preformatting the output for JSON..
                    //This approach supports both the requested file format and plaintext 
                    //files such as chat.txt without the "( )*" separators..
                    var matchedres = bracketChecker.Match(line);
                    line = bracketChecker.IsMatch(line) ? line.Replace(matchedres.Value, "") : line;
                    if (string.IsNullOrEmpty(line))
                    {
                        //Ignore splitting empty strings..
                        goto Start;
                    }
                    var split = line.Split(' ');

                    //Determine whether to hide or show sendersId by means of alphanumeric obfuscation.
                    //With this method we keep the corrseponding value to the output of each message,
                    //and also each corresponding userId is obfuscated in the same manner. 
                    string changedId = obfuscate ? ObfuscateIds(split[1]) : split[1];
                    var msg = AppendMessageAsString(split.Skip(2).GetEnumerator());
                    messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(split[0])), changedId,
                        msg));
                }

                return new Conversation(conversationName, messages, new List<ActiveUser>());
            }

                //Additional exception handling with relevant error messages.
            catch (ArgumentException argEx)
            {
                Console.WriteLine("There was a problem with your input. - Error[{0}]", argEx.Message);
                Console.ReadLine();
            }
            catch (FormatException formEx)
            {
                Console.WriteLine("The file format was invalid. - Error[{0}]", formEx.Message);
                Console.ReadLine();
            }
            catch (IndexOutOfRangeException idxOrEx)
            {
                Console.WriteLine("This might be a different file type from what is supported. - Error[{0}]", idxOrEx.Message);
                Console.ReadLine();
            }
            catch (FileNotFoundException fnFEx)
            {
                Console.WriteLine("The file was not found. - Error[{0}]", fnFEx.Message);
                Console.ReadLine();
            }
            catch (IOException ioEx)
            {
                Console.WriteLine("Something went wrong in the IO. - Error[{0}]", ioEx.Message);
                Console.ReadLine();
            }
            return new Conversation("Empty", new List<Message>(), new List<ActiveUser>());
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

                //Always include the report in the end of the JSON data.
                IncludeActiveUsersReport(conversation);
                var serialized = JsonConvert.SerializeObject(conversation, Formatting.Indented);

                writer.Write(serialized);

                writer.Flush();

                writer.Close();
            }

            //Additional exception handling with relevant error messages.
            catch (UnauthorizedAccessException unAEx)
            {
                Console.WriteLine("Problem writing to outputfile. - Error[{0}]",unAEx.Message);
                Console.ReadLine();
            }

            catch (ArgumentException arghEx)
            {
                Console.WriteLine("Error in path of command. - Error[{0}]",arghEx.Message);
                Console.ReadLine();
            }

            catch (SecurityException secEx)
            {
                Console.WriteLine("No permission to file. - Error[{0}]", secEx.Message);
                Console.ReadLine();
            }
            catch (DirectoryNotFoundException dnFoundEx)
            {
                Console.WriteLine("Path invalid. - Error[{0}]", dnFoundEx.Message);
                Console.ReadLine();
            }
            catch (IOException ioExc)
            {
                Console.WriteLine("Something went wrong in the IO. - Error[{0}]", ioExc.Message);
                Console.ReadLine();
            }
        }

        /// <summary>
        /// Todo: Add reporting to the end of the JSON conversation file - DONE
        /// Method for attaching a conversation's report to the JSON output.
        /// </summary>
        /// <param name="convforReport"></param>
        /// <returns> A formatted instance of a <see cref="Conversation"/> document that includes a
        /// conversation's basic usage/statistics.
        /// </returns>
        public Conversation IncludeActiveUsersReport(Conversation convforReport)
        {

            //Count and create each user's messages with the corresponding senderId
            //This applies to obfuscated or normal Ids.
            foreach (var msg in convforReport.messages)
            {
                string num = convforReport.messages.Count(sid => sid.senderId == msg.senderId).ToString();

                ActiveUser au = new ActiveUser(msg.senderId, num);
                convforReport.activeusers.Add(au);
            }


            //Additional filtering and processing of results occurs here (with sorting).
            var temp =
                convforReport.activeusers.GroupBy(gByNum => gByNum.TotalOfMsgs, gByUid => gByUid.UserId)
                    .OrderByDescending(group => group.Key);

            var keys = temp.Select(group => group.Key).ToArray().GetEnumerator();
            var values = temp.SelectMany(group => group).Distinct().ToArray();
            int cnt = 0;
            List<ActiveUser> activeslist = new List<ActiveUser>();

            //Iterate over collections for retrieving appropriate report values.
            while (keys.MoveNext())
            {
                activeslist.Add(new ActiveUser(values[cnt], keys.Current.ToString()));
                cnt++;
            }

            convforReport.activeusers = activeslist;
            return convforReport;
        }
    }
}