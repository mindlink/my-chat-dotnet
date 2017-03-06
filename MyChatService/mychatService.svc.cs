using MyChatLibrary;
using System;
using System.Collections.Generic;
using System.IO;

namespace MyChatService
{
    public class mychatService : IService1
    {
        public string GetChartData(string file)
        {
            return new Statistics().getPieChartData(file);
        }

        public string TriggerConversation(string[] args)
        {
            ConversationExporter ce = new ConversationExporter();
            ce.Trigger(args);

            return "It's done :-)";
        }


        public void UploadFile(RemoteFileInfo request)
        {
            FileStream targetStream = null;
            Stream sourceStream = request.FileByteStream;
            string filePath = request.FileName;

            using (targetStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                const int bufferLen = 65000;
                byte[] buffer = new byte[bufferLen];
                int count = 0;
                while ((count = sourceStream.Read(buffer, 0, bufferLen)) > 0)
                {
                    // save to output stream
                    targetStream.Write(buffer, 0, count);
                }
            }
        }

        public RemoteFileInfo DownloadFile(DownloadRequest request)
        {
            RemoteFileInfo result = new RemoteFileInfo();
            try
            {
                //string filePath = System.IO.Path.Combine(@"c:\Uploadfiles", request.FileName);
                FileInfo fileInfo = new FileInfo(request.FileName);

                // check if exists
                if (!fileInfo.Exists)
                    throw new System.IO.FileNotFoundException("File not found", request.FileName);

                // open stream
                FileStream stream = new FileStream(request.FileName, FileMode.Open, FileAccess.Read);

                // return result 
                result.FileName = request.FileName;
                    result.Length = fileInfo.Length;
                    result.FileByteStream = stream;

            }
            catch (IOException ex)
            {
                Console.WriteLine("File not found\n{0}", ex.Message);
            }
            return result;
        }
    }
}
