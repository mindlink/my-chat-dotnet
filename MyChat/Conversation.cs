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
        public List<Message> messages;

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
            this.messages = messages;
        }
        
        public Conversation blackListWords()
        {
            
        }
        
        public Conversation filterByUser()
        {
            
        }
        
        public Conversation filterByKeyword()
        {
            
        }
        
        public Conversation obfuscateUserID()
        {
            
            Dictionary<string, string> obfUserIDs = new Dictionary<string, string>();
            
            for each(var message in this.messages)  { 
                
                int hiddenUsername = 0; 
                     
                if(!obfUserIDs.ContainsKey(message.senderId))
                {
                    hiddenUsername++; 
                    obfUserIDs.Add(message.senderId, hiddenUsername); 
                }
                message.senderId = obfUserIDs[message.senderId]; 
            }
   
        }
    }
}
