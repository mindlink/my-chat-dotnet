using System;
using System.IO;

namespace MindLink.Recruitment.MyChat
{
    
    public sealed class CommandLineArgumentParser
    {
        public string ParseCommandLineArguments(string[] arguments)
        {
            if(arguments.Length != 1){
                throw new ArgumentException($"You have provided the wrong number of arguments. This tool needs 1 argument, a json config file");
            }

            if (Path.GetExtension(arguments[0]) != ".json")
            {
                throw new ArgumentException("You need to provide a JSON configuration file for this tool to work.");
            }
             
            return arguments[0];
        }
    }
}
