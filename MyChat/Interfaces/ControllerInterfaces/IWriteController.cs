using System;
using System.Collections.Generic;
using System.Text;

namespace MindLink.Recruitment.MyChat.Interfaces.ControllerInterfaces
{
    public interface IWriteController
    {

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
        void WriteConversation(Conversation conversation, string outputFilePath);
    }
}
