using MyChat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat
{
    public class ConversationModifier
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
            foreach(var x in messages)
            {
               foreach(var badword in blacklist)
                {
                    if(x.content.Contains(badword)) { x.content = x.content.Replace(badword, redacted);}                                 
                }
            }

            return new Conversation(conversation.name, messages);
        }

        public Conversation PerformActions(List<FilterType> actionList,ConversationExporterConfiguration configuration)
        {
            foreach(FilterType action in actionList)
            {

                if(action == FilterType.KEYWORD) { conversation = ModifyByKey(configuration.keyword, action, conversation.messages); }
                if (action == FilterType.SENDER_ID) { conversation = ModifyByKey(configuration.user, action, conversation.messages); }
                if (action == FilterType.BLACKLIST) { conversation = ModifyByBlacklist(configuration.blacklist,conversation.messages); }


            }

            return conversation;



        }
             


    }
}
