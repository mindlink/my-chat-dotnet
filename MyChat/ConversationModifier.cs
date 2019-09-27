using MyChat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace MindLink.Recruitment.MyChat
{
    public class ConversationModifier
    {

        public Conversation conversation;

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

            try
            {
                foreach (var x in messages)
                {
                    foreach (var badword in blacklist)
                    {
                        if (x.content.Contains(badword)) { x.content = x.content.Replace(badword, Globals.REDACTED_WORD); }
                    }
                }
            }
            catch(ArgumentException e)
            {
                Console.WriteLine(e);
                Console.WriteLine(Globals.EXCEPTION_ARGUMENT_BLACKLIST);
            }

            return new Conversation(conversation.name, messages);
        }

        public Conversation ModifyBySensitiveData(List<Message> messages)
        {
          
            foreach(var x in messages)
            {
                x.content = Regex.Replace(x.content, Globals.REGEX_CREDIT_CARD, Globals.REDACTED_WORD);
                x.content = Regex.Replace(x.content, Globals.REGEX_UK_PHONE_NUMBER, Globals.REDACTED_WORD);
            }

            return new Conversation(conversation.name, messages);
        }

        public Conversation ModifyByObfiscatingIDs(List<Message> messages)
        {
            Dictionary<string,string> uniqueUsers = new Dictionary<string, string>();

            int uniqueIDs = 0;

            foreach(var message in messages)
            {
                if(!uniqueUsers.ContainsKey(message.senderId))
                {
                    uniqueIDs++;
                    uniqueUsers.Add(message.senderId, Globals.OBFUSCATION_KEY + uniqueIDs);
                }

                message.senderId = uniqueUsers[message.senderId];

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
                if (action == FilterType.HIDE_SENSITIVE_DATA) { conversation = ModifyBySensitiveData(conversation.messages); }
                if (action == FilterType.OBFUSCATE_IDS) { conversation = ModifyByObfiscatingIDs(conversation.messages);  }
            }

            return conversation;

        }         

    }
}
