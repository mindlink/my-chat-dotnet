using System;
using System.ServiceModel;
using MyChatLibrary;
using System.Collections.Generic;

namespace MyChatService
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        RemoteFileInfo DownloadFile(DownloadRequest request);

        [OperationContract]
        void UploadFile(RemoteFileInfo request);

        [OperationContract]
        string TriggerConversation(string[] args);

        [OperationContract]
        string  GetChartData(string file);
    }

    [MessageContract]
    public class Argumentlist
    {
        [MessageBodyMember]
        public string[] Args;
    }

    [MessageContract]
    public class DownloadRequest
    {
        [MessageBodyMember]
        public string FileName;
    }

    [MessageContract]
    public sealed class RemoteFileInfo : IDisposable
    {
        [MessageHeader(MustUnderstand = true)]
        public string FileName;

        [MessageHeader(MustUnderstand = true)]
        public long Length;

        [MessageBodyMember(Order = 1)]
        public System.IO.Stream FileByteStream;
        
        public void Dispose()
        {
            if (FileByteStream != null)
            {
                FileByteStream.Close();
                FileByteStream = null;
            }
        }
    }
}


