namespace MindLink.Recruitment.MyChat
{
    using System.Collections.Generic;
    using System;
    using System.IO;
    using System.Security;
    using Newtonsoft.Json;

    /// <summary>
    /// model for the output, to allow report to be optional in children
    /// </summary>
    public class Log
    {
        /// <summary>
        /// The name of the conversation.
        /// </summary>
        public string name;

        /// <summary>
        /// The messages in the conversation.
        /// </summary>
        public IEnumerable<Message> messages;
        public Log(Conversation conversation)
        {
            this.name = conversation.name;
            this.messages = conversation.messages;
        }

        public void WriteLogToOutput(string outputFilePath)
        {
             try
            {
                var writer = new StreamWriter(new FileStream(outputFilePath, FileMode.Create, FileAccess.ReadWrite));

                var serialized = JsonConvert.SerializeObject(this, Formatting.Indented);

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

    public sealed class LogWithReport : Log
    {
        public IEnumerable<Activity> activity;
        public LogWithReport(Conversation conversation, List<Activity> report) : base(conversation)
        {
            this.name = conversation.name;
            this.messages = conversation.messages;
            this.activity = report; 
        }

    }
}