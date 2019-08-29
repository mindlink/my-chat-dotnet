using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat
{
    public class Blacklist
    {
        public HashSet<string> content;

        public Blacklist(HashSet<string> content)
        {
            this.content = content;
        }
    }
}
