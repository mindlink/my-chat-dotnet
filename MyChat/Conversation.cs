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
        public string name { get; }

        /// <summary>
        /// The messages in the conversation.
        /// </summary>
        private List<Message> _messages;
        public List<Message> messages
        {
            get
            {
                return _messages;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Conversation"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the conversation.
        /// </param>
        /// <param name="messages">
        /// The messages in the conversation.
        /// </param>
        public Conversation(string name, List<Message> messages)
        {
            this.name = name;
            _messages = messages;
        }

        /// <summary>
        /// Discards any messages not sent by <paramref name="user"/>.
        /// </summary>
        /// <param name="user">
        /// The user to filter messages by.
        /// </param>
        public void FilterByUser(string user)
        {
            if (user == "")
            {
                return;
            }
            var filteredMessages = new List<Message>();
            foreach (Message m in messages)
            {
                if (m.senderId == user)
                {
                    filteredMessages.Add(m);
                }
            }
            _messages = filteredMessages;
        }

        /// <summary>
        /// Discards any messages that do not contain <paramref name="keyword"/>.
        /// </summary>
        /// <param name="keyword">
        /// The keyword to filter message by.
        /// </param>
        public void FilterByKeyword(string keyword)
        {
            if (keyword == "")
            {
                return;
            }
            var filteredMessages = new List<Message>();
            foreach (Message m in messages)
            {
                if (IndexOfWord(m.content, keyword) != -1)
                {
                    filteredMessages.Add(m);
                }
            }
            _messages = filteredMessages;
        }

        /// <summary>
        /// Searches messages for instances of the words in <paramref name="blacklist"/> and replaces them with "*redacted*".
        /// </summary>
        /// <param name="blacklist">
        /// The list of strings to be replaced.
        /// </param>
        public void HideBlacklistWords(List<string> blacklist)
        {
            if (blacklist == null)
            {
                return;
            }
            var filteredMessages = new List<Message>();
            for (int i = 0; i < messages.Count; i++)
            {
                filteredMessages.Add(messages[i]);
                foreach (string hide in blacklist)
                {
                    int index;
                    while ((index = IndexOfWord(filteredMessages[i].content, hide)) != -1)
                    {
                        filteredMessages[i].ReplaceWord(index, hide.Length, "*redacted*");
                    }
                }
            }
            _messages = filteredMessages;
        }

        /// <summary>
        /// Finds the index where <paramref name="word"/> is located in <paramref name="message"/>.
        /// Ignoring cases where <paramref name="word"/> is a substring of a larger word in <paramref name="message"/>.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="word"></param>
        /// <returns>
        /// Returns the index of the first instance of <paramref name="word"/> in <paramref name="message"/>.
        /// Returns -1 if <paramref name="message"/> doesnt contain the word <paramref name="word"/>.
        /// </returns>
        private int IndexOfWord(string message, string word)
        {
            message = message.ToUpper();
            word = word.ToUpper();
            if (!message.Contains(word)) //First check if the substring is in the message at all.
            {
                return -1;
            }
            else //Ensure it is not a smaller part of a larger word.
            {
                int index;
                int start = 0;
                while ((index = message.IndexOf(word, start)) != -1) //While there are more instances of the substring.
                {
                    start = index + word.Length; //Increase the start index to avoid finding the same substring repeatedly.
                    //If the substring has non letter characters before and after it or is at the start or end of the string.
                    if ((index - 1 < 0 || !char.IsLetter(message[index - 1])) && (index + word.Length >= message.Length || !char.IsLetter(message[index + word.Length])))
                    {
                        return index;
                    }
                }
                return -1; //Return -1 if no individual instances of the word found.
            }
        }
    }
}
