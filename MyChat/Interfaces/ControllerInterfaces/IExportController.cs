using System;
using System.Collections.Generic;
using System.Text;

namespace MindLink.Recruitment.MyChat.Interfaces.ControllerInterfaces
{
    public interface IExportController
    {

        /// <summary>
        /// Exports the conversation at <paramref name="inputFilePath"/> as JSON to <paramref name="outputFilePath"/>.
        /// </summary>
        /// <param name="inputFilePath">
        /// The input file path.
        /// </param>
        /// <param name="outputFilePath">
        /// The output file path.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when a path is invalid.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown when something bad happens.
        /// </exception>
        void ExportConversation(string inputFilePath, string outputFilePath);
    }
}
