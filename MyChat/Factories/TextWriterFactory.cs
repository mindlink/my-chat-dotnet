using System.IO;
using System.Security;

namespace MindLink.Recruitment.MyChat
{
    public static class TextWriterFactory
    {
        public static TextWriter GetStreamWriter(string outputFilePath, FileMode mode, FileAccess access)
        {
            // GetStreamWriter takes in an output file path, file mode and access permission and returns a Writer
            // configured based on those options.
            try
            {
                return new StreamWriter(new FileStream(outputFilePath, mode, access));
            }
            catch (SecurityException ex)
            {
                throw new SecurityException($"No permission to access {outputFilePath}.", ex);
            }
            catch (DirectoryNotFoundException ex)
            {
                throw new DirectoryNotFoundException($"The path to {outputFilePath} is invalid and wasn't found.", ex);
            }
            catch (IOException ex)
            {
                throw new IOException("Something went wrong in the IO.", ex);
            }
        }
    }
}