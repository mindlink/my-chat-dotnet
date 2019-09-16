namespace MindLink.Recruitment.MyChat
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents the model of a conversation.
    /// </summary>
    public sealed class Conversation
    {
        /// <summary>
        /// The name of the conversation.
        /// </summary>
        public string name;

        /// <summary>
        /// The messages in the conversation.
        /// </summary>
        public IEnumerable<Message> messages;

        /// <summary>
        /// Initializes a new instance of the <see cref="Conversation"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the conversation.
        /// </param>
        /// <param name="messages">
        /// The messages in the conversation.
        /// </param>
        public Conversation(string name, IEnumerable<Message> messages)
        {
            this.name = name;
            this.messages = messages;
        }

        public Conversation FilterByUser(string senderId)
        {
            IEnumerable<Message> filteredMessages = new List<Message>();
            foreach (Message message in messages)
            {
                if (message.senderId == senderId)
                {
                    ((List<Message>)filteredMessages).Add(message);
                }
            }

            return new Conversation(name + "-senderId:" + senderId, filteredMessages);
        }

        public Conversation FilterByKeyword(string keyword)
        {
            IEnumerable<Message> filteredMessages = new List<Message>();
            foreach (Message message in messages)
            {
                if (message.content.Contains(keyword))
                {
                    ((List<Message>)filteredMessages).Add(message);
                }
            }

            return new Conversation(name + "-keyword:" + keyword, filteredMessages);
        }

        public void BlacklistWord(string word)
        {
            foreach (Message message in messages)
            {
                string newContent = "";

                string[] split = message.content.Split(' ');

                for (int i = 0; i < split.Length; i++)
                {
                    //Remove symbols from the end of words
                    string trimmedWord = split[i].TrimEnd("!?,.:;".ToCharArray());
                    string punctuation = split[i].Substring(trimmedWord.Length);

                    if (trimmedWord != word)
                    {
                        newContent += split[i];
                    }
                    else
                    {
                        newContent += "*redacted*" + punctuation;
                    }
                    if (i < split.Length - 1)
                    {
                        newContent += " ";
                    }
                }

                message.content = newContent;
            }
        }
    }
}
