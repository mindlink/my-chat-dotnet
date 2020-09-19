namespace MindLink.Recruitment.MyChat.Features.Essential
{
    using MindLink.Recruitment.MyChat.Interfaces.FeatureInterfaces;
    using MyChatModel.ModelData;
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Text.RegularExpressions;

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

            // IF the word passed in is empty
            if (word == "" || word == " ")
            {
                // THROW an ArgumentNullException, to notify the user they have specifed the filter but
                // not supplied any arguments
                throw new ArgumentNullException("No word was supplied for the word filter to use");
            }
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
                if (msg.Content.Contains(word))
                {
                    validInput = true;

                    filteredMessages.Add(msg);
                }
            }
            // SET the filterConversation to a new Conversation, passing in the conversation name
            // and the filteredMessages
            filteredConversation = new Conversation(conversation.Name, filteredMessages);

            // IF there is no valid input
            if (!validInput) 
            {
                // THEN the word was not found in the conversation, and we would like to tell the user this
                    string conversationMessage = "The word " + word + " was not found in the conversation";
                // CALL to the conversations AddFilterMessage and pass in the message
                filteredConversation.AddFilterMessage(conversationMessage);
            }

            return filteredConversation;
        }
    }
}
