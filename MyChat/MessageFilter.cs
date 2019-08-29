using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using MindLink.Recruitment.MyChat;
using Newtonsoft.Json;

namespace MindLink.Recruitment.MyChat
{
    public static class MessageFilter
    {
        public static Conversation FilterMessageByUsername(Conversation conversation, string usernameFilter)
        {
            var filteredMessageList = new List<Message>() { };
            var filteredConversation = conversation;

            foreach (var message in conversation.messages)
            {
                if (usernameFilter == null)
                {
                    filteredMessageList.Add(message);
                }
                else if (usernameFilter == message.userId)
                {
                    filteredMessageList.Add(message);
                }
            }

            filteredConversation.messages = filteredMessageList;

            return filteredConversation;
        }

        public static Conversation FilterMessageByKeyword(Conversation conversation, string keywordFilter)
        {
            var filteredMessageList = new List<Message>() { };
            var filteredConversation = conversation;

            foreach (var message in conversation.messages)
            {
                if (keywordFilter == null)
                {
                    filteredMessageList.Add(message);
                }
                else if (message.content.Contains(keywordFilter) == true)
                {
                    filteredMessageList.Add(message);
                }
            }

            filteredConversation.messages = filteredMessageList;

            return filteredConversation;
        }

        public static Conversation FilterMessageByBlacklist(Conversation conversation, Blacklist blacklist)
        {
            var filteredMessageList = new List<Message>() { };
            var filteredConversation = conversation;

            foreach (var message in conversation.messages)
            {
                var split = message.content.Split(' ');
                for (int i = 0; i < split.Count(); i++)
                {
                        if (blacklist.content.Any(new string(split[i].Where(c => !char.IsPunctuation(c)).ToArray()).ToLower().Contains))
                        {
                            split[i] = "*redacted*";
                        }
                        
                }
                message.content = string.Join(" ", split);
                filteredMessageList.Add(message);
            }

            filteredConversation.messages = filteredMessageList;

            return filteredConversation;
        }
    }
}
