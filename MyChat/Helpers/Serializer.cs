using MyChat.Core.Abstract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat.Helpers
{
    public class Serializer: ISerialize
    {
        public string SerializeObect(object input)
        {
            return JsonConvert.SerializeObject(input, Formatting.Indented);
        }
    }
}
