namespace MindLink.Recruitment.MyChat.Controllers
{

    using MindLink.Recruitment.MyChat.Interfaces.ControllerInterfaces;
    using MyChatModel.ModelData;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Class, separated out the ReadConversation functionality of the ConversationExporter class
    /// to this class. 
    /// </summary>
    public sealed class ReadController : IReadController
    {
        /// <summary>
        /// CONSTRUCTOR for ReadController class, empty.
        /// </summary>
        public ReadController() 
        {

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
        public Conversation ReadConversation(string inputFilePath) 
        {
            try
            {
                var reader = new StreamReader(new FileStream(inputFilePath, FileMode.Open, FileAccess.Read),
                    Encoding.ASCII);

                string conversationName = reader.ReadLine();
                var messages = new List<Message>();

                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var split = line.Split(' ', 3);

                    messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(split[0])), split[1], split[2]));
                }

                return new Conversation(conversationName, messages);
            }
            catch (FileNotFoundException inner)
            {
                throw new ArgumentException("The file was not found.", inner);
            }
            catch (IOException inner)
            {
                throw new Exception("Something went wrong in the IO.", inner);
            }
        }
    }
}
