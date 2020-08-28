namespace MindLink.Recruitment.MyChat
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Security;
    using System.Text;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a conversation exporter that can read a conversation and write it out in JSON.
    /// </summary>
    public sealed class ConversationExporter
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
        /// <param name="censorCard">
        /// A flag to hide credit card numbers.
        /// </param>
        /// <param name="censorPhone">
        /// A flag to hide phone numbers.
        /// </param>
        /// <param name="obfuscateUser">
        /// A flag to obfuscate user ids.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when a path is invalid.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown when something bad happens.
        /// </exception>
        public void ExportConversation(string inputFilePath, string outputFilePath, string censorCard = "no", string censorPhone = "no", string obfuscateUser = "no")
        {
            Conversation conversation = this.ReadConversation(inputFilePath, censorCard, censorPhone, obfuscateUser);

            this.WriteConversation(conversation, outputFilePath);

            Console.WriteLine("Conversation exported from '{0}' to '{1}'", inputFilePath, outputFilePath);
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
        /// <param name="censorCard">
        /// A flag to hide credit card numbers.
        /// </param>
        /// <param name="censorPhone">
        /// A flag to hide phone numbers.
        /// </param>
        /// <param name="obfuscateUser">
        /// A flag to obfuscate user ids.
        /// </param>
        /// <param name="filterMethod">
        /// A delegate to pass filter methods into export conversation.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when a path is invalid.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown when something bad happens.
        /// </exception>
        public void ExportConversation(string inputFilePath, string outputFilePath, string filterWord, string censorCard, string censorPhone, string obfuscateUser, Func<string, string, string, string, string, List<Message>> filterMethod)
        {
            Conversation conversation = this.ReadConversation(inputFilePath, filterWord, censorCard, censorPhone, obfuscateUser, filterMethod);

            this.WriteConversation(conversation, outputFilePath);

            Console.WriteLine("Conversation exported from '{0}' to '{1}'", inputFilePath, outputFilePath);
        }

        /// <summary>
        /// Helper method to read the conversation from <paramref name="inputFilePath"/>.
        /// </summary>
        /// <param name="inputFilePath">
        /// The input file path.
        /// </param>
        /// <param name="searchWord">
        /// A keyword for filter methods.
        /// </param>
        /// <param name="censorCard">
        /// A flag to hide credit card numbers.
        /// </param>
        /// <param name="censorPhone">
        /// A flag to hide phone numbers.
        /// </param>
        /// <param name="obfuscateUser">
        /// A flag to obfuscate user ids.
        /// </param>
        /// <param name="filterMethod">
        /// A delegate to pass filter methods into export conversation.
        /// </param>
        /// <returns>
        /// A <see cref="Conversation"/> model representing the conversation.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when a path is invalid.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown when something bad happens.
        /// </exception>
        public Conversation ReadConversation(string inputFilePath, string searchWord, string censorCard, string censorPhone, string obfuscateUser, Func<string, string, string, string, string, List<Message>> filterMethod)
        {
            try
            {
                var reader = new StreamReader(new FileStream(inputFilePath, FileMode.Open, FileAccess.Read), Encoding.ASCII);

                var messages = filterMethod(inputFilePath, searchWord, censorCard, censorPhone, obfuscateUser);

                string conversationName = reader.ReadLine();

                return new Conversation(conversationName, messages);
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
        public Conversation ReadConversation(string inputFilePath, string censorCard, string censorPhone, string obfuscateUser)
        {
            try
            {
                var reader = new StreamReader(new FileStream(inputFilePath, FileMode.Open, FileAccess.Read), Encoding.ASCII);
                var messages = new List<Message>();
                string line, conversationName = reader.ReadLine();

                while ((line = reader.ReadLine()) != null)
                {
                    if (censorCard.Equals("yes", StringComparison.OrdinalIgnoreCase) || censorCard.Equals("y", StringComparison.OrdinalIgnoreCase))
                        line = ConversationCensor.CensorCardNumber(line);
                    if (censorPhone.Equals("yes", StringComparison.OrdinalIgnoreCase) || censorPhone.Equals("y", StringComparison.OrdinalIgnoreCase))
                        line = ConversationCensor.CensorPhoneNumber(line);

                    var split = line.Split(' ');
                    string content = "";

                    for (int i = 2; i < split.Length; i++)
                    {
                        if (obfuscateUser.Equals("yes", StringComparison.OrdinalIgnoreCase) || obfuscateUser.Equals("y", StringComparison.OrdinalIgnoreCase))
                            split[1] = Obfuscate.ObfuscateString(split[1]);

                        content += split[i] + " ";
                    }

                    messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(split[0])), split[1], content.TrimEnd()));
                }

                return new Conversation(conversationName, messages);
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
                ConversationReport conversationReport = new ConversationReport();
                
                var report = conversationReport.CreateReport(conversation.messages);

                ReportStatistics reportStatistics = new ReportStatistics(report);

                var writer = new StreamWriter(new FileStream(outputFilePath, FileMode.Create, FileAccess.ReadWrite));
                var serializedConversation = JsonConvert.SerializeObject(conversation, Formatting.Indented);
                //var serializedReport = JsonConvert.SerializeObject(reportStatistics, Formatting.Indented);

                writer.Write(serializedConversation);

                //writer.Write(serializedReport);

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
