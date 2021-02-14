using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MindLink.Recruitment.MyChat.Data;

namespace MindLink.Recruitment.MyChat
{
    public sealed class ConversationReader
    {
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
            try
            {
                var reader = new StreamReader(new FileStream(inputFilePath, FileMode.Open, FileAccess.Read),
                    Encoding.ASCII);

                string conversationName = reader.ReadLine();
                var messages = new List<Message>();

                string line;
                const char separator = ' ';

                while ((line = reader.ReadLine()) != null)
                {
                    var split = line.Split(separator);

                    // Fixes bug by using array range operator
                    var content = string.Join(separator, split[2..split.Length]);

                    messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(split[0])), split[1], content));
                }

                return new Conversation(conversationName, messages);
            }
            catch (FileNotFoundException fileNotFoundEx)
            {
                throw new ArgumentException("The file was not found.", fileNotFoundEx);
            }
            catch (IOException inputOutputEx)
            {
                throw new IOException("Something went wrong in the IO.", inputOutputEx);
            }
        }
    }
}
