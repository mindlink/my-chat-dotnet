namespace MyChatModel.ModelData
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class Conversation
    {
        /// <summary>
        /// The name of the conversation.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// A list of strings for the filters to write any messages into
        /// in case of certain scenarios
        /// </summary>
        public IList<string> FilterMessage { get; set; }
        /// <summary>
        /// The messages in the conversation.
        /// </summary>
        public IEnumerable<Message> Messages { get; set; }
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
            this.Name = name;
            this.Messages = messages;
        }

        public void AddFilterMessage(string convoMsg) 
        {
            // IF FilterMessage is null, then initialise it
            if (FilterMessage == null)
                FilterMessage = new List<string>();
            // ADD the message to the conversations message
            FilterMessage.Add(convoMsg);
        }
    }
}
