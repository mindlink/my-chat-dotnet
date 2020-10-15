namespace MindLink.Recruitment.MyChat
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Interface holder for filters
    /// </summary>
    public class Filter
    {
        public virtual void ApplyFilter(Conversation conversation)
        {
        }
    }

    public sealed class SenderFilter : Filter
    {
        private string namefilter;

        public SenderFilter(String namefilter){
            this.namefilter = namefilter;
        }

        public override void ApplyFilter(Conversation conversation)
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

    public sealed class KeywordFilter : Filter
    {
        private string keywordfilter;

        public KeywordFilter(String keywordfilter){
            this.keywordfilter = keywordfilter;
        }

        public override void ApplyFilter(Conversation conversation)
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

    public sealed class BlacklistFilter : Filter
    {
        private string[] blacklist;

        public BlacklistFilter(String[] blacklist){
            this.blacklist = blacklist;
        }

        public override void ApplyFilter(Conversation conversation)
        {
            foreach(Message message in conversation.messages)
            {
                this.ApplyRegexRedaction(message);
            };
        }
        public void ApplyRegexRedaction(Message message)
        {
            foreach(string blacklistedWord in this.blacklist)
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