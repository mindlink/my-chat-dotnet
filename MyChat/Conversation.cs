namespace MindLink.Recruitment.MyChat
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions; 

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
            
            List<string> blkLstWords = new List<string>
            
            string replacement = "*redacted*";
            //string hiddenWord; 
                
            for each (var message in this.messages) {
                
                for each (var hiddenWord in blkLstWords){
                    
                    if (message.content.Contains(hiddenWord)){
                    //add replacement code here
                        message.content = message.content.Replace(hiddenWord, replacement);
                }
                    
                }
            }
            return new Conversation(conversation.name, messages); 
        }
        
        public Conversation filterByUser(string userFilter)
        {
            List<Message> filterUserMsg = new List<Message>();
                
            for each (var message in this.message){
                
                if (message.senderId == userFilter){
                    
                    filterUserMsg.Add(message); 
                }
                
            }
            return new Conversation(conversation.name, messages); 
        }
        
        public Conversation filterByKeyword(string userKey)
        {
            List<Message> filterUserKey = new List<Message>();
                
            for each (var message in this.message){
                
                if(message.content.Contains(userKey){
                    
                    filterUserKey.Add(message);
                    
                }
            }
                   
              return new Conversation(conversation.name, messages); 
        }
        
        public Conversation obfuscateUserID(string obfuscateID)
        {
            
            Dictionary<string, string> obfUserIDs = new Dictionary<string, string>();
            
            for each(var message in this.messages)  { 
                
                int hiddenUsername = 0; 
                     
                if(!obfUserIDs.ContainsKey(message.senderId))
                {
                    hiddenUsername++; 
                    obfUserIDs.Add(message.senderId, hiddenUsername.ToString()); 
                }
                message.senderId = obfUserIDs[message.senderId]; 
            }
                return new Conversation(conversation.name, messages); 
        }
    }
}
