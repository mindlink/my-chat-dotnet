using System.IO;
using System.Security;
using System.Text;

namespace MindLink.Recruitment.MyChat
{
    public static class TextReaderFactory
    {
        public static TextReader GetStreamReader(string inputFilePath, FileMode mode, FileAccess access, Encoding encoding)
        {
            // GetStreamReader takes in a file path, file mode, access permission and encoding style and returns a
            // StreamReader configured for that.
            try
            {
                return new StreamReader(new FileStream(inputFilePath, mode, access), encoding);
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException($"The file {inputFilePath} was not found.");
            }
            catch (IOException ex)
            {
                throw new IOException("Something went wrong in the IO.", ex);
            }
            catch (SecurityException ex)
            {
                throw new SecurityException(
                    $"Couldn't open the file {inputFilePath} because of a permissions issue", ex);
            }

        }        
    }
}