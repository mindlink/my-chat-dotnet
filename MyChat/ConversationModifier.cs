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
            Conversation newConversation = new Conversation(conversation.name, new List<Message>());

            foreach(Message m in conversation.messages)
            {
                if (m.senderId.Equals(key))
                {
                    newConversation.messages.Add()
                }


            }
  

        }

        return 


    }
}
