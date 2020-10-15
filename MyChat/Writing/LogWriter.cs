namespace MindLink.Recruitment.MyChat
{
    using System;
    using System.IO;
    using System.Security;
    using Newtonsoft.Json;

    /// <summary>
    /// writes logs to output
    /// </summary>

    public sealed class LogWriter
    {
       public void WriteToOutput(Log output, string outputFilePath)
        {
            try
            {
                var writer = new StreamWriter(new FileStream(outputFilePath, FileMode.Create, FileAccess.ReadWrite));

                var serialized = JsonConvert.SerializeObject(output, Formatting.Indented);

                writer.Write(serialized);

                writer.Flush();

                writer.Close();
            }
            catch (SecurityException)
            {
                throw new ArgumentException("No permission to file.");
            }
            catch (DirectoryNotFoundException)
            {
                throw new ArgumentException("Path invalid.");
            }
            catch (IOException)
            {
                throw new Exception("Something went wrong in the IO.");
            }
        }
    }
}