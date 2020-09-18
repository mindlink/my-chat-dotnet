namespace MindLink.Recruitment.MyChat.Features.Essential
{

    using MindLink.Recruitment.MyChat.Interfaces.FeatureInterfaces;
    using MyChatModel.ModelData;
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Class to filter a conversation by a specified user name
    /// </summary>
    public sealed class FilterByUser : IStrategyFilter
    {
        /// <summary>
        /// string to store the user name, called user
        /// </summary>
        private string user;

        /// <summary>
        /// Constructor for FilterByUser, takes 1 string param
        /// </summary>
        /// <param name="user"> the name of the of user to be filtered </param>
        public FilterByUser(string user) 
        {
            // SET
            this.user = user;
        }

        public Conversation ApplyFilter(Conversation conversation) 
        {
            // INITIALISE a local list of type Message called filteredMessages
            IList<Message> filteredMessages = new List<Message>();

            // FOREACH through the messages in the conversation
            foreach (Message msg in conversation.Messages) 
            {
                // IF the message sender ID matches the id of the user
                if (msg.SenderId == user) 
                {
                    // ADD that message to the list of messages
                    filteredMessages.Add(msg);
                }
            }

            // RETURN a new conversation, passing the conversation name to the constructor and the filtered
            // list of messages
            return new Conversation(conversation.Name, filteredMessages);
        }
    }
}
