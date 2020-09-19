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
            if (user == "" || String.IsNullOrWhiteSpace(user))
            {
                throw new ArgumentNullException("No username was supplied for the filter to use");
            }
        }

        public Conversation ApplyFilter(Conversation conversation) 
        {
            // INTIIALISE a Conversation called filteredConversation
            Conversation filteredConversation;
            // INSTANTIATE a bool to say whether the user name was valid or not
            bool validUser = false;
            // INITIALISE a local list of type Message called filteredMessages
            IList<Message> filteredMessages = new List<Message>();
            // FOREACH through the messages in the conversation
            foreach (Message msg in conversation.Messages) 
            {
                // IF the message sender ID matches the id of the user
                if (msg.SenderId == user) 
                {
                    validUser = true;
                    // ADD that message to the list of messages
                    filteredMessages.Add(msg);
                }
            }

            filteredConversation = new Conversation(conversation.Name, filteredMessages);

            // IF the username is not valid
            if (!validUser)
            {
                // THEN the user was not found in the conversation, and we would like to tell the user this
                string conversationMessage = "No user by the name " + user + " was found";
                // AddFilterMessage conversationMessage to the conversation
                filteredConversation.AddFilterMessage(conversationMessage);

            }

            // RETURN a new conversation, passing the conversation name to the constructor and the filtered
            // list of messages
            return filteredConversation;
        }
    }
}
