using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat
{
    class ConversationModifier
    {

        public Conversation conversation;

        public ConversationModifier(Conversation conversation)
        {
            this.conversation = conversation;
        }

        public Conversation ModifyByUser(string key)
        {
            List<Message> messages = new List<Message>();

            foreach(Message m in conversation.messages)
            {
                if (m.senderId.Equals(key))
                {
                    messages.Add(m);
                }

            }

            return new Conversation(conversation.name, messages);

        }

 


    }
}
