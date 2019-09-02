namespace MindLink.Recruitment.MyChat
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents the model of a blacklist.
    /// </summary>
    public class Blacklist
    {
        public HashSet<string> content;

        public Blacklist(HashSet<string> content)
        {
            this.content = content;
        }
    }
}
