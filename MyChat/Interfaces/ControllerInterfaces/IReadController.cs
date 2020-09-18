using System;
using System.Collections.Generic;
using System.Text;

namespace MindLink.Recruitment.MyChat.Interfaces.ControllerInterfaces
{
    public interface IReadController
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
        Conversation ReadConversation(string inputFilePath);
    }
}
