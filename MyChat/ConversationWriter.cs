using System;
using System.IO;
using System.Security;
using Newtonsoft.Json;

namespace MindLink.Recruitment.MyChat
{
    /// <summary>
    /// Represents a helper to write to file.
    /// </summary>
    public sealed class ConversationWriter
    {
        public ConversationWriter()
        {
        }
        /// <summary>
        /// Wrie to file using a <paramref name="conversation"/> model and an <paramref name="outputFilePath"/>.
        /// </summary>
        public void WriteConversation(Conversation conversation, string outputFilePath)
        {
            try
            {
                using (var writer = new StreamWriter(new FileStream(outputFilePath, FileMode.Create, FileAccess.ReadWrite)))
                {
                    var serialized = JsonConvert.SerializeObject(conversation, Formatting.Indented);
                    writer.Write(serialized);
                }
            }
            catch (SecurityException ex)
            {
                throw new ArgumentException("No permission to file", ex);
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
                throw new ArgumentException("Part of a file or directory cannot be found.", ex);
            }
        }
    }
}
