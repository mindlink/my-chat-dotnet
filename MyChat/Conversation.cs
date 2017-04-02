using System.Collections.Generic;

namespace MindLink.Recruitment.MyChat
{
    /// <summary>
    /// Represents the model of a conversation.
    /// </summary>
    public sealed class Conversation
    {
        /// <summary>
        /// The name of the conversation.
        /// </summary>
        string name;

        /// <summary>
        /// The messages in the conversation.
        /// </summary>
        IEnumerable<Message> messages;

        /// <summary>
        /// Sets or returns the name of the conversation
        /// </summary>
        public string Name 
        {
            get { return this.name; }
            set { this.name = value;}
        }       

        /// <summary>
        /// Sets or returns the messages of the conversation
        /// </summary>
        public IEnumerable<Message> Messages 
        {
            get { return this.messages; }
            set { this.messages = value; }
        }      

        /// Initializes a new instance of the <see cref="Conversation"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the conversation.
        /// </param>
        /// <param name="messages">
        /// The messages in the conversation.
        /// </param>
        /// 
        public Conversation(string name, IEnumerable<Message> messages)
        {
            Name = name;
            Messages = messages;
        }
    }
}
