using System;

namespace MindLink.Recruitment.MyChat
{
    using System.Collections.Generic;
    
    public sealed class Conversation
    {
        public string Name { get;}
        public IEnumerable<IMessage> Messages { get; }

        public Conversation(string name, IEnumerable<IMessage> messages)
        {
            Name = name;
            Messages = messages;
        }
    }
    
     
}
