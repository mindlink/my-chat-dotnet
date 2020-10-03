using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq; 

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
        public bool isReportNeeded{ get; }
        public ConversationEditor(EditingConfiguration config)
        {
            this.namefilter = config.filterByUser;
            this.keywordfilter = config.filterByKeyword;
            if (config.blacklist != null) {
                this.blacklisted = config.blacklist.Split(',');
            } else {
                this.blacklisted = new string[] {null};
            }
            this.isReportNeeded = config.isReportNeeded;
        }
       public void EditConversation(Conversation conversation)
        {
            this.FilterConversationByUsername(conversation);
            this.FilterConversationByKeyword(conversation);
            this.RedactBlacklistedWords(conversation);
            this.AddReport(conversation);
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

        public void AddReport(Conversation conversation)
        {
            if (this.isReportNeeded)
            {
                var report = new List<Activity>();
                foreach(Message message in conversation.messages)
                {
                    if (report.Any(record=>record.sender == message.senderId) == false)
                    {
                        int count = conversation.messages.Count(count => count.senderId == message.senderId);
                        var record = new Activity(message.senderId, count);
                        report.Add(record);
                    }
                }
                conversation.addReport(report.OrderByDescending(record => record.count));
            }
        }
    }
}