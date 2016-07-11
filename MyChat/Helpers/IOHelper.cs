using MindLink.Recruitment.MyChat.Core;
using MyChat.Core.Abstract;
using MyChat.Core.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat.Helpers
{
    public class IOHelper : IOHelperBase , IIOHelper
    {        
        public IOHelper()
        {
            setupLogs();
        }

        public void setupLogs()
        {
            //Create folder for logs 
            LogsFolder = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "_LOGS");

            // Create current logfile
            CurrentLogFile = Path.Combine(LogsFolder, DateTime.Now.Date.ToString("dd_MM_yyyy") + ".txt");


            if (!Directory.Exists(LogsFolder))
                Directory.CreateDirectory(LogsFolder);

            if (!File.Exists(CurrentLogFile))
                File.Create(CurrentLogFile).Close();

        }
        public StreamReader ReadFile(string inputFilePath)
        {
            try
            {
                var reader = new StreamReader(new FileStream(inputFilePath, FileMode.Open, FileAccess.Read), Encoding.ASCII);
                return reader;
            }
            catch (FileNotFoundException e)
            {
                Log(e);
            }
            catch (IOException e)
            {
                Log(e);
            }

            return null;


        }


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
        public void WriteFile(String data, string outputFilePath)
        {
            try
            {
                var writer = new StreamWriter(new FileStream(outputFilePath, FileMode.Create, FileAccess.ReadWrite));

                writer.Write(data);

                writer.Flush();

                writer.Close();
            }
            catch (SecurityException e)
            {
                Log(e);
            }
            catch (DirectoryNotFoundException e)
            {
                Log(e);
            }
            catch (IOException e)
            {
                Log(e);
            }
        }

        public void AppendTextToFile(String inputText)
        {
            try
            {
                var reader = ReadFile(CurrentLogFile);
                string buf = reader.ReadToEnd();
                reader.Close();

                String tag = "\r\n--------------\r\n";

                buf += tag + inputText + tag;

                WriteFile(buf, CurrentLogFile);
            }
            catch (Exception e)
            {
                Console.WriteLine(e); // Something when wrong and we cannot write log to file , atleast let the user know about it
            }
        }

        public System.DateTimeOffset FromUnixTimeSeconds(String input)
        {
            return DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(input));
        }

    }
}
