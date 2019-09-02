namespace MindLink.Recruitment.MyChat
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Linq;
    using System.Text.RegularExpressions;
    using MindLink.Recruitment.MyChat;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a message filter than can perform redactions and obfuscations on converstaion content.
    /// </summary>
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
            var filteredConversation = conversation;

            foreach (var message in filteredConversation.messages)
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
            }

            return filteredConversation;
        }

        public static Conversation ObfuscateIdentity(Conversation conversation, bool obfuscationState)
        {
            if (obfuscationState == true)
            {
                var filteredConversation = conversation;
                var encryptedIds = new Dictionary<string, int>() { };
                var count = 0;
                foreach (var message in filteredConversation.messages)
                {
                    if (encryptedIds.ContainsKey(message.userId) == false)
                    {
                        encryptedIds.Add(message.userId, count);
                        count++;
                    }
                }
                foreach (var message in filteredConversation.messages)
                {
                    message.userId = "User#" + encryptedIds[message.userId];
                }
                return filteredConversation;
            }
            else
            {
                return conversation;
            }
        }
    }
}
