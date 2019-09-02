namespace MindLink.Recruitment.MyChat
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Linq;
    using MindLink.Recruitment.MyChat;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a conversation reader that can read a conversation text file and return a conversation.
    /// </summary>
    public static class ConversationReader
    {
        public static Conversation TextToConversation(string inputFilePath)
        {
            try
            {
                var reader = new StreamReader(new FileStream(@"..\..\" + inputFilePath, FileMode.Open, FileAccess.Read),
                    Encoding.ASCII);

                var messages = new List<Message>();

                string conversationName = reader.ReadLine();
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var split = line.Split(' ');
                    var timestamp = split[0];
                    var userId = split[1];
                    var message = string.Join(" ", split.Skip(2));
                    messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(timestamp)), userId, message));
                }

                return new Conversation(conversationName, messages);
            }
            catch (FileNotFoundException exception)
            {
                throw new ArgumentNullException("Input file could not be located / does not exist.", exception);
            }
        }
    }
}
