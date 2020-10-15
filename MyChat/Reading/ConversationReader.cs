namespace MindLink.Recruitment.MyChat
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    /// <summary>
    /// model for the output, to allow report to be optional in children
    /// </summary>
    public class ConversationReader
    {
        /// <summary>
        /// The name of the conversation.
        /// </summary>
        public string inputFilePath;
        public ConversationReader(string inputFilePath)
        {
            this.inputFilePath = inputFilePath;
        }

        public Conversation ReadConversation()
        {
            try
            {
                var reader = new StreamReader(new FileStream(this.inputFilePath, FileMode.Open, FileAccess.Read),
                    Encoding.ASCII);

                string conversationName = reader.ReadLine();
                var messages = new List<Message>();

                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var split = line.Split(' ');

                    var message = new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(split[0])),
                        split[1], string.Join(" ",split[2..]));
                    messages.Add(message);
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
    }
}