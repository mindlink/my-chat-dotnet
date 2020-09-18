using System;

namespace MindLink.Recruitment.MyChat
{
    
    public sealed class CommandLineArgumentParser
    {
        public string ParseCommandLineArguments(string[] arguments)
        {
            if(arguments.Length != 1){
                throw new ArgumentException($"You provided {arguments.Length} arguments when this tool needs 1 argument, a json config file");
            }
            return arguments[0];
        }
    }
}
