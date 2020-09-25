using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MindLink.Recruitment.MyChat
{
    public class ConversationReader
    {
        public Conversation Conversation { get; private set; }

        public ConversationReader(string path)
        {
            Conversation = ReadConversation(path);
        }
        

        public Conversation ReadConversation(string inputFilePath)
        {
            try
            {
                var reader = new StreamReader(new FileStream(inputFilePath, FileMode.Open, FileAccess.Read), Encoding.ASCII);
                string conversationName = reader.ReadLine();
                var messages = new List<Message>();
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var split = line.Split(' ');
                    messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(split[0])), split[1], String.Join(" ", split.Skip(2))));
                }

                return new Conversation(conversationName, messages);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("The conversation could not be extracted " + ex.Message + ex.StackTrace);
                throw ex;
            }
            catch (Exception ex)
            {
                Console.WriteLine("The conversation could not be extracted " + ex.Message + ex.StackTrace);
                throw ex;
            }


        }

    }
}

