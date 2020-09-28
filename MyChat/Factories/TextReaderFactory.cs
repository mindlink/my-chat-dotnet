using System.IO;
using System.Security;
using System.Text;

namespace MindLink.Recruitment.MyChat
{
    public class TextReaderFactory : IReaderFactory
    {
        public TextReader GetStreamReader(ConversationExporterConfiguration config)
        {
            // GetStreamReader takes in a file path, file mode, access permission and encoding style and returns a
            // StreamReader configured for that.
            try
            {
                return new StreamReader(new FileStream(config.inputFilePath, FileMode.Open, FileAccess.Read), Encoding.ASCII);
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException($"The file {config.inputFilePath} was not found.");
            }
            catch (IOException ex)
            {
                throw new IOException("Something went wrong in the IO.", ex);
            }
            catch (SecurityException ex)
            {
                throw new SecurityException(
                    $"Couldn't open the file {config.inputFilePath} because of a permissions issue", ex);
            }

        }        
    }

    public interface IReaderFactory
    {
        public TextReader GetStreamReader(ConversationExporterConfiguration config);
    }
}