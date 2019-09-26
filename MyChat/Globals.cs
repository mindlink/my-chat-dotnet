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
    }
}
