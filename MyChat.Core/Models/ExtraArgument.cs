using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat.Core
{
    public class ExtraArgument
    {
        public enum FilterType
        {        
            Empty,
            User,
            Keyword,
            HideWord
        }

        public FilterType Filter { get; set; }
        public List<String> Value = new List<string>();

    }
}
