using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat
{
    public class TextConversationReader : IConversationReader
    {
        public Conversation ReadConversation(string inputFilePath)
        {
            try
            {
                List<Message> messages = new List<Message>();
                string conversationName = "";

                using (var reader = new StreamReader(inputFilePath))
                {
                    conversationName = reader.ReadLine();
                    string line = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] messageContents = line.Split(new char[] { ' ' }, 3);

                        DateTimeOffset messageTime;
                        long messageTimeValue;
                        if (long.TryParse(messageContents[0], out messageTimeValue))
                        {
                            messageTime = DateTimeOffset.FromUnixTimeMilliseconds(messageTimeValue);
                        }
                        else
                        {
                            throw new InvalidDataException("Expected timestamp wasn't parseable.");
                        }

                        string sender = messageContents[1];
                        string contents = messageContents[2];

                        messages.Add(new Message(messageTime, sender, contents));
                    }
                }

                return new Conversation(conversationName, messages, "", null);
            }
            catch (Exception ex) when (ex is IOException || ex is FileNotFoundException || ex is DirectoryNotFoundException)
            {
                throw;
            }
        }
    }
}
