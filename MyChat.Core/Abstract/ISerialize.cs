using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyChat.Core.Abstract
{
    public interface ISerialize
    {
        string SerializeObect(object input);
    }
}
