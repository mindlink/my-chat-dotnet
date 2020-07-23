namespace MindLink.Recruitment.MyChat.ConversationWriters
{
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Security;
    using MindLink.Recruitment.MyChat.ConversationData;

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
            catch (SecurityException e)
            {
                throw new ArgumentException("No permission to output file", outputFilePath, e);
            }
            catch (DirectoryNotFoundException e)
            {
                throw new ArgumentException("The output file path is invalid", outputFilePath, e);
            }
            catch (PathTooLongException e)
            {
                throw new ArgumentException("The iutput file path is too long", outputFilePath, e);
            }
        }
    }
}