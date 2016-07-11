using MindLink.Recruitment.MyChat.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyChat.Core.Abstract
{
    public interface IIOHelper
    {
        void setupLogs();
        StreamReader ReadFile(string inputFilePath);

        void WriteFile(String data, string outputFilePath);

        void AppendTextToFile(String text);

        System.DateTimeOffset FromUnixTimeSeconds(String input);
    }
}
