namespace MindLink.Recruitment.MyChat.Features.Essential
{
    using MindLink.Recruitment.MyChat.Interfaces.FeatureInterfaces;
    using MyChatModel.ModelData;
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;

    /// <summary>
    /// Class to filter a conversation and show only the messages which contain the word
    /// specified
    /// </summary>
    public sealed class FilterByWord : IStrategyFilter
    {
        /// <summary>
        /// string to store the word to find in the conversation
        /// </summary>
        private string word;
        
        /// <summary>
        /// Constructor for FilterByWord, takes 1 param, which will be the word to
        /// search for in the conversation
        /// </summary>
        /// <param name="word"> the word to find in the conversation</param>
        public FilterByWord(string word) 
        {
            this.word = word;
        }

        public Conversation ApplyFilter(Conversation conversation) 
        {
            // INTIIALISE a Conversation called filteredConversation
            Conversation filteredConversation;
            // INITIALISE a bool to say whether the word was found in the conversation
            bool validInput = false;
            // INITIALISE a list of type Message called filteredMessages
            IList<Message> filteredMessages = new List<Message>();
            // FOREACH through the messages in the conversation
            foreach (Message msg in conversation.Messages) 
            {
                // IF the messag content contains the word
                if (msg.Content.Contains(word)) 
                {
                    // ADD the message to the list of filteredMessages
                    filteredMessages.Add(msg);
                }
            }

            filteredConversation = new Conversation(conversation.Name, filteredMessages);

            if (!validInput) 
            {
                if (word == "" || word == " ")
                {
                    throw new ArgumentNullException("No word was supplied for the filter to use");
                }
                else
                {
                    string conversationMessage = "The word " + word + " was not found in the conversation";

                    filteredConversation.AddFilterMessage(conversationMessage);
                }
            }

            return new Conversation(conversation.Name, filteredMessages);
        }
    }
}
