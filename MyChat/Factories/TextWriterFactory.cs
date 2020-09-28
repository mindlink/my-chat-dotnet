using System.IO;
using System.Security;

namespace MindLink.Recruitment.MyChat
{
    public class TextWriterFactory : IWriterFactory
    {
        public TextWriter GetStreamWriter(ConversationExporterConfiguration config)
        {
            // GetStreamWriter takes in an output file path, file mode and access permission and returns a Writer
            // configured based on those options.
            try
            {
                return new StreamWriter(new FileStream(config.outputFilePath, FileMode.Create, FileAccess.ReadWrite));
            }
            catch (SecurityException ex)
            {
                throw new SecurityException($"No permission to access {config.outputFilePath}.", ex);
            }
            catch (DirectoryNotFoundException ex)
            {
                throw new DirectoryNotFoundException($"The path to {config.outputFilePath} is invalid and wasn't found.", ex);
            }
            catch (IOException ex)
            {
                throw new IOException("Something went wrong in the IO.", ex);
            }
        }
    }

    public interface IWriterFactory
    {
        public TextWriter GetStreamWriter(ConversationExporterConfiguration config);
    }
}