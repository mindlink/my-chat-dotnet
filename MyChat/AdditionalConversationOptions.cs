using System.Collections.Generic;

namespace MindLink.Recruitment.MyChat
{
    public sealed class AdditionalConversationOptions
    {
        private ConversationExporterConfiguration exporterConfiguration;

        /// <summary>
        /// Constructor of this class, initialise exporter configration
        /// </summary>
        /// <param name="exporterConfig">
        /// Contains configurations for filter/manipulation options
        /// </param>
        public AdditionalConversationOptions(ConversationExporterConfiguration exporterConfig)
        {
            exporterConfiguration = exporterConfig;
        }

        /// <summary>
        /// Apply all options to conversation passed from command-line argument
        /// </summary>
        /// <param name="conversationName">
        /// Name of conversation
        /// </param>
        /// <param name="messages">
        /// All messages sent by users from conversation
        /// </param>
        /// <returns></returns>
        public Conversation ApplyOptionsToMessages(string conversationName, List<Message> messages)
        {
            var filteredMessages = messages;

            var filterByName = new FilterByName();
            var filterByWord = new FilterByWord();

            if (exporterConfiguration.FilterByUser != null)
            {
                filteredMessages = filterByName.Filter(filteredMessages, exporterConfiguration.FilterByUser);
            }

            if (exporterConfiguration.FilterByWord != null)
            {
                filteredMessages = filterByWord.Filter(filteredMessages, exporterConfiguration.FilterByWord);
            }

            return new Conversation(conversationName, filteredMessages);
        }
    }
}

