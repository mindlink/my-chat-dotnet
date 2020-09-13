using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
    
namespace MindLink.Recruitment.MyChat
{
    /// <summary>
    /// Represents a helper to read a text file.
    /// </summary>
    public sealed class ConversationReader
    {
        public List<Message> messages;
        string line;
        string username;

        public ConversationReader()
        {
        }

        /// <summary>
        /// Creates a conversation object from <paramref name="configuration"/>.
        /// </summary>
        /// <param name="arguments">
        /// The command line arguments.
        /// </param>
        /// <returns>
        /// A <see cref="Conversation"/> representing the read text file.
        /// </returns>
        public Conversation ReadConversation(ConversationExporterConfiguration configuration)
        {
            try
            {
                using (var reader = new StreamReader(new FileStream(configuration.InputFilePath, FileMode.Open, FileAccess.Read),
                    Encoding.ASCII))
                {
                    string conversationName = reader.ReadLine();
                    messages = new List<Message>();

                    while ((line = reader.ReadLine()) != null)
                    {
                        var split = line.Split(' ');
                        username = split[1];
                        var content = "";
                        var count = 2;
                        while (count < split.Length)
                        {
                            if (count - split.Length != -1)
                            {
                                content += split[count] + " ";
                            }
                            else
                            {
                                content += split[count];
                            }
                            count++;
                        }
                        Message message = new Message { Timestamp = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(split[0])), SenderId = username, Content = content };
                        messages.Add(message);
                    }
                    Conversation conversation = new Conversation { Name = conversationName, Messages = messages };
                    return conversation;
                }
            }
            catch (FileNotFoundException ex)
            {
                throw new ArgumentException("Input file not found.", ex);
            }
            catch (PathTooLongException ex)
            {
                throw new ArgumentException("Specified path too long.", ex);
            }
            catch (FileLoadException ex)
            {
                throw new ArgumentException("File could not be loaded.", ex);
            }
            catch (EndOfStreamException ex)
            {
                throw new ArgumentException("Reading attempted past end of stream.", ex);
            }
            catch (DirectoryNotFoundException ex)
            {
                throw new ArgumentException("Part of a file or directory cannot be found", ex);
            }
        }
    }
}