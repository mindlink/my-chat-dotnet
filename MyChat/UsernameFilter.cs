using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using MindLink.Recruitment.MyChat;
using Newtonsoft.Json;

namespace MindLink.Recruitment.MyChat
{
    public static class UsernameFilter
    {
        public static Conversation FilterMessageByUsername(Conversation conversation, string usernameFilter)
        {
            var filteredMessageList = new List<Message>() { };
            var filteredConversation = conversation;

            foreach (var message in conversation.messages)
            {
                if (usernameFilter == message.userId)
                {
                    filteredMessageList.Add(message);
                }
                else if (usernameFilter == null)
                {
                    filteredMessageList.Add(message);
                }
            }

            filteredConversation.messages = filteredMessageList;

            return filteredConversation;
        }
    }
}
