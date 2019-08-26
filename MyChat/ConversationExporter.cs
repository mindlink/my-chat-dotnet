namespace MyChat
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Linq;
    using MindLink.Recruitment.MyChat;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a conversation exporter that can read a conversation and write it out in JSON.
    /// </summary>
    public sealed class ConversationExporter
    {
        #region Variables
        private string inputFilePath;
        private string outputFilePath;
        private string senderIdFilter;
        private string keywordFilter;
        private List<string> blacklist = new List<string>() { };
        private bool isNumberFilterActive;
        private bool isIdObfuscationActive;
        #endregion

        #region Constructor
        public ConversationExporter(CommandLineArgumentParser commandLineArgumentParser)
        {
            inputFilePath = commandLineArgumentParser.inputFilePath;
            outputFilePath = commandLineArgumentParser.outputFilePath;
            senderIdFilter = commandLineArgumentParser.senderIdFilter;
            keywordFilter = commandLineArgumentParser.keywordFilter;
            blacklist = commandLineArgumentParser.blacklist;
            isNumberFilterActive = commandLineArgumentParser.isNumberFilterActive;
            isIdObfuscationActive = commandLineArgumentParser.isIdObfuscationActive;
        }
        #endregion

        #region Methods
        /// <summary>
        /// The application entry point.
        /// </summary>
        /// <param name="arguments">
        /// The command line arguments.
        /// </param>
        static void Main(string[] arguments)
        {
            var commandLineArgumentParser = new CommandLineArgumentParser(arguments);
            var conversationExporter = new ConversationExporter(commandLineArgumentParser);
            conversationExporter.ExportConversation();
        }

        /// <summary>
        /// Exports the conversation at <paramref name="inputFilePath"/> as JSON to <paramref name="outputFilePath"/>.
        /// </summary>
        public void ExportConversation()
        {
            Conversation conversation = ReadConversation(inputFilePath);

            WriteConversation(conversation, outputFilePath);

            Console.WriteLine("Conversation exported from '{0}' to '{1}'", inputFilePath, outputFilePath);
        }

        /// <summary>
        /// Helper method to read the conversation from <paramref name="inputFilePath"/>.
        /// </summary>
        private Conversation ReadConversation(string inputFilePath)
        {
            var reader = new StreamReader(new FileStream(inputFilePath, FileMode.Open, FileAccess.Read),
            Encoding.ASCII);

            string conversationName = reader.ReadLine();
            var messages = new List<Message>();

            string line;

            while ((line = reader.ReadLine()) != null)
            {
                var split = line.Split(' ');

                split = ApplyNumberFilter(split, isNumberFilterActive);

                var timestamp = split[0];
                var senderId = split[1];
                var message = string.Join(" ", split.Skip(2));

                if (IsKeywordPresent(split, keywordFilter) == true)
                {
                    ///apply userId filter. Case insensitive.
                    if (senderIdFilter == null)
                    {
                        messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(timestamp)), senderId, message));
                    }
                    else if (senderIdFilter.Equals(senderId, StringComparison.InvariantCultureIgnoreCase))
                    {
                        messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(timestamp)), senderId, message));
                    }
                }
            }

            ApplyBlacklist(messages, blacklist);
            HideSenderId(messages, isIdObfuscationActive);
            var activityReport = ReportActivity(messages);

            return new Conversation(conversationName, messages, activityReport);
        }

        /// <summary>
        /// Helper method to write the <paramref name="conversation"/> as JSON to <paramref name="outputFilePath"/>.
        /// </summary>
        private void WriteConversation(Conversation conversation, string outputFilePath)
        {
            var writer = new StreamWriter(new FileStream(outputFilePath, FileMode.Create, FileAccess.ReadWrite));

            var serialized = JsonConvert.SerializeObject(conversation, Formatting.Indented);

            writer.Write(serialized);

            writer.Flush();

            writer.Close();
        }

        /// <summary>
        /// Checks if keyword present in split string array. Case insensitive.
        /// </summary>
        private bool IsKeywordPresent(string[] split, string keyword)
        {
            var allowMessages = false;
            if (keyword == null)
            {
                allowMessages = true;
            }
            else if (keyword != null)
            {
                for (int i = 2; i < split.Count(); i++)
                {
                    if (keyword.Equals(new string(split[i].Where(c => !char.IsPunctuation(c)).ToArray()), StringComparison.InvariantCultureIgnoreCase))
                    {
                        allowMessages = true;
                        break;
                    }
                }
            }
            return allowMessages;
        }

        /// <summary>
        /// Changes blacklisted words to *redacted*. Case insensitive.
        /// </summary>
        private void ApplyBlacklist(List<Message> messages, List<string> blacklist)
        {
            foreach (var message in messages)
            {
                var split = message.content.Split(' ');
                for (int i = 0; i < split.Count(); i++)
                {
                    for (int j = 0; j < blacklist.Count(); j++)
                    {
                        if (blacklist[j].Equals(new string(split[i].Where(c => !char.IsPunctuation(c)).ToArray()), StringComparison.InvariantCultureIgnoreCase))
                        {
                            split[i] = "*redacted*";
                        }
                    }
                }
                message.content = string.Join(" ", split);
            }
        }

        /// <summary>
        /// Applies credit card and phone number filters.
        /// </summary>
        private string[] ApplyNumberFilter(string[] split, bool isNumberFilterActive)
        {
            if (isNumberFilterActive == true)
            {
                split = ApplyCreditCardFilter(split);
                split = ApplyPhoneNumberFilter(split);
            }
            return split;
        }

        /// <summary>
        /// Applies credit card filter. A complete series of 16 numbers from 0-9 (excluding spaces) becomes *redacted*.
        /// </summary>
        private string[] ApplyCreditCardFilter(string[] split)
        {
            var filteredList = split.ToList<string>();
            var count = 0;
            var indexList = new List<int>() { };
            for (int i = 0; i < split.Count(); i++)
            {
                var noPuncString = new string(split[i].Where(c => !char.IsPunctuation(c)).ToArray());
                if (noPuncString.All(char.IsDigit))
                {
                    count = count + noPuncString.Length;
                    indexList.Add(i);
                    if (count == 16)
                    {
                        filteredList[indexList[0]] = "*redacted*";
                        for (int j = 1; j < indexList.Count(); j++)
                        {
                            filteredList.RemoveAt(indexList[1]);
                        }
                    }
                }
                else
                {
                    count = 0;
                    indexList.Clear();
                }
            }
            split = filteredList.ToArray();
            return split;
        }

        /// <summary>
        /// Applies phone number filter. A complete series of 11 numbers from 0-9 (excluding spaces) becomes *redacted*.
        /// </summary>
        private string[] ApplyPhoneNumberFilter(string[] split)
        {
            var filteredList = split.ToList<string>();
            var count = 0;
            var indexList = new List<int>() { };
            for (int i = 0; i < split.Count(); i++)
            {
                var noPuncString = new string(split[i].Where(c => !char.IsPunctuation(c)).ToArray());
                if (noPuncString.All(char.IsDigit))
                {
                    count = count + noPuncString.Length;
                    indexList.Add(i);
                    if (count == 11)
                    {
                        filteredList[indexList[0]] = "*redacted*";
                        for (int j = 1; j < indexList.Count(); j++)
                        {
                            filteredList.RemoveAt(indexList[1]);
                        }
                    }
                }
                else
                {
                    count = 0;
                    indexList.Clear();
                }
            }
            split = filteredList.ToArray();
            return split;
        }

        /// <summary>
        /// Changes senderId into generic 'User#n' format. Identity preserved via unique integer n.
        /// </summary>
        private void HideSenderId(List<Message> messages, bool isIdObfuscationActive)
        {
            if (isIdObfuscationActive == true)
            {
                var encryptedIds = new Dictionary<string, int>() { };
                var count = 0;
                foreach (var message in messages)
                {
                    if (encryptedIds.ContainsKey(message.senderId) == false)
                    {
                        encryptedIds.Add(message.senderId, count);
                        count++;
                    }
                }
                foreach (var message in messages)
                {
                    message.senderId = "User#" + encryptedIds[message.senderId];
                }
            }
        }

        /// <summary>
        /// Creates an activity report in the form of string array. Users ordered by number of messages sent.
        /// </summary>
        private string[] ReportActivity(List<Message> messages)
        {
            var nameList = new List<string>();
            foreach (var message in messages)
            {
                nameList.Add(message.senderId);
            }
            var frequency = nameList.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
            var frequencyList = frequency.ToList();

            frequencyList.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));
            string[] activityReport = new string[frequencyList.Count()];
            for (int i = 0; i < frequencyList.Count(); i++)
            {
                activityReport[i] = $"UserId: {frequencyList[i].Key}, Messages sent = {frequencyList[i].Value}";
            }
            return activityReport;
            
        }
        #endregion
    }
}
