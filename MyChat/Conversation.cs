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
        //public string Name
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// The messages in the conversation.
        /// </summary>
        public IEnumerable<Message> messages;
//        public IEnumerable<Message> Messagez
//            {
//            get;
//            set;
//}

        ///// <summary>
        ///// Initializes a new instance of the <see cref="Conversation"/> class.
        ///// </summary>
        ///// <param name="name">
        ///// The name of the conversation.
        ///// </param>
        ///// <param name="messages">
        ///// The messages in the conversation.
        ///// </param>
        public Conversation(string name, IEnumerable<Message> messages)
        {
            this.name = name;
            this.messages = messages;
        }
    }
}
