using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace MindLink.Recruitment.MyChat
{
    /// <summary>
    /// Stores the functions to edit the exported JSON
    /// </summary>
    public sealed class ConversationEditor
    {
        public string namefilter{ get; }
        public string keywordfilter{ get; }
        public string[] blacklisted{ get; }
        public ConversationEditor(EditingConfiguration config)
        {
            this.namefilter = config.filterByUser;
            this.keywordfilter = config.filterByKeyword;
            if (config.blacklist != null) {
                this.blacklisted = config.blacklist.Split(',');
            } else {
                this.blacklisted = new string[] {null};
            }
        }
       public void EditConversation(Conversation conversation)
        {
            this.FilterConversationByUsername(conversation);
            this.FilterConversationByKeyword(conversation);
            this.RedactBlacklistedWords(conversation);
        }

        public void FilterConversationByUsername(Conversation conversation)
        {
            if (this.namefilter != null)
            {
                var editedMessages = new List<Message>();
                foreach(Message message in conversation.messages)
                {
                    if (this.namefilter == message.senderId)
                    {
                        editedMessages.Add(message);
                    }
                };
                conversation.messages = editedMessages;
            }
        }

        public void FilterConversationByKeyword(Conversation conversation)
        {
            if (this.keywordfilter != null)
            {
                var editedMessages = new List<Message>();
                foreach(Message message in conversation.messages)
                {
                    if (message.content.Contains(this.keywordfilter))
                    {
                        editedMessages.Add(message);
                    }
                };
                conversation.messages = editedMessages;
            }
        }

        public void RedactBlacklistedWords(Conversation conversation)
        {
            if (this.blacklisted[0] != null)
            {
                var editedMessages = new List<Message>();
                foreach(Message message in conversation.messages)
                {
                    this.ApplyRegexRedaction(message);
                };
            }
        }

        public void ApplyRegexRedaction(Message message)
        {
            foreach(string blacklistedWord in this.blacklisted)
            {
                string input = message.content;
                string pattern = blacklistedWord;
                string replacement = "*redacted*";
                Regex rgx = new Regex("\\b"+pattern+"\\b");
                message.content = rgx.Replace(input, replacement);
            }
        }
    }
}