using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat
{
    public class Globals
    {
        public static readonly string REDACTED_WORD = @"\*redacted*\";

        public static readonly int MINIMUM_MESSAGE_LENGTH = 3;

        public static readonly string REGEX_CREDIT_CARD = @"[\d]+((-|\s)?[\d]+)+";

        public static readonly string REGEX_UK_PHONE_NUMBER = @"^(?:0|\+?44)(?:\d\s?){9,10}$";

        public static readonly string EXCEPTION_ARGUMENT_BLACKLIST = "Blacklist cannot empty. If -b flag is included, at least one element has to be in it.";

        public static readonly string EXCEPTION_FILE_NOT_FOUND = "File not found. Make sure your input path is correct";

        public static readonly string EXCEPTION_DIRECTORY_NOT_FOUND = "File or directory not found. Make sure your output path is correct";

        public static readonly string EXCEPTION_ARGUMENT_NOT_FOUND = "No arguments were entered.";

        public static readonly string EXCEPTION_ARGUMENT_NULL_NOT_FOUND = "Configuration is null, input and/or output path was entered incorrectly.";

        public static readonly string EXCEPTION_IO = "Something went wrong in the IO.";
        
        public static readonly string EXCEPTION_FILE_NOT_FOUND_GENERAL = "The file was not found.";

        public static readonly string EXCEPTION_DIRECTORY_NOT_FOUND_GENERAL = "Path invalid.";

        public static readonly string EXCEPTION_SECURITY = "No permission to file.";

        public static readonly string OBFUSCATION_KEY = "user";




    }
}
