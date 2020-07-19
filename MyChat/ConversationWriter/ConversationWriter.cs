namespace MindLink.Recruitment.MyChat
{
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Security;

    /// <summary>
    /// Responsible for writing conversation data to the configuration output path
    /// </summary>
    public sealed class ConversationWriter : IConversationWriter
    {
        /// <summary>
        /// Writes the <paramref name="conversation"/> as JSON to <paramref name="outputFilePath"/>.
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
        public void WriteConversation(Conversation conversation, string outputFilePath)
        {
            try
            {
                var writer = new StreamWriter(new FileStream(outputFilePath, FileMode.Create, FileAccess.ReadWrite));

                var serialized = JsonConvert.SerializeObject(conversation, Formatting.Indented);

                writer.Write(serialized);

                writer.Flush();

                writer.Close();
            }
            catch (SecurityException)
            {
                throw new SecurityException("No permission to output file");
            }
            catch (DirectoryNotFoundException)
            {
                throw new DirectoryNotFoundException("Output file path is invalid");
            }
            catch (PathTooLongException)
            {
                throw new PathTooLongException("Output file path is too long");
            }
        }
    }
}