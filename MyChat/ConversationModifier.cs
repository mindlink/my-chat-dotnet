using MyChat;
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

        String redacted = @"\*redacted*\";

        public ConversationModifier(Conversation conversation)
        {
            this.conversation = conversation;
        }

        public Conversation ModifyByKey(string key, FilterType filterType, List<Message> messages)
        {

            List<Message> filteredMessages = new List<Message>();


            foreach(Message m in messages)
            {
                switch(filterType){

                    case FilterType.SENDER_ID:

                        if (m.senderId.Equals(key)) { filteredMessages.Add(m); }

                        break;

                    case FilterType.KEYWORD:

                        if (m.content.Contains(key)) { filteredMessages.Add(m); }

                        break;

                }

            }


            return new Conversation(conversation.name, filteredMessages);

        }

        public Conversation ModifyByBlacklist(List<string> blacklist, List<Message> messages)
        {
            List<Message> filteredMessages = new List<Message>();


            foreach(var x in messages)
            {

                var content = x.content;

               foreach(var badword in blacklist)
                {

                    if(content.Contains(badword))
                    {
                        x.content = content.Replace(badword, redacted);
                    }
                        
                }




            }


            return new Conversation(conversation.name, messages);
        }



        public Conversation ModifyByKeyWord(string key)
        {
            List<Message> messages = new List<Message>();

            foreach(Message m in conversation.messages)
            {
                if (m.content.Contains(key))
                {
                    messages.Add(m);
                }

            }

            return new Conversation(conversation.name, messages);


        }





    }
}
