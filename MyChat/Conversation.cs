namespace MindLink.Recruitment.MyChat
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Represents the model of a conversation and contains filtering methods.
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

        /// <summary>
        /// Filters conversation to only show messages from user specified by senderId.
        /// </summary>
        /// <param name="senderId">
        /// The id of the user.
        /// </param>
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

        /// <summary>
        /// Filters conversation to only show messages that contain a specific keyword.
        /// </summary>
        /// <param name="keyword">
        /// The keyword to be searched for.
        /// </param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns a conversation in which all ocurrences of a word are changed to *redacted*
        /// </summary>
        /// <param name="word">
        /// The word to be blacklisted.
        /// </param>
        /// <returns></returns>
        public Conversation BlacklistWord(string word)
        {
            IEnumerable<Message> newMessages = new List<Message>();

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

                Message newMessage = new Message(message.timestamp, message.senderId, newContent);
                ((List<Message>)newMessages).Add(newMessage);
            }
            return new Conversation(name, newMessages);
        }

        public Conversation BlacklistPhoneAndCC()
        {
            IEnumerable<Message> newMessages = new List<Message>();

            foreach (Message message in messages)
            {
                string newMessage = "";

                //Regex assumes no spaces in phone number
                string pattern = @"(\+44|0)\d{10}";
                newMessage = Regex.Replace(message.content, pattern, "*redacted*");

                //Regex assumes no spaces in credit card number
                pattern = @"\d{16}";
                newMessage = Regex.Replace(newMessage, pattern, "*redacted*");

                ((List<Message>)newMessages).Add(new Message(message.timestamp, message.senderId, newMessage));
            }

            return new Conversation(name, newMessages);
        }
    }
}
