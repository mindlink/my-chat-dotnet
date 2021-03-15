using System.Collections.Generic;

namespace MindLink.Recruitment.MyChat
{
    public sealed class AdditionalConversationOptions
    {
        private ConversationExporterConfiguration exporterConfiguration;

        /// <summary>
        /// Initialise new instance of <see cref="AdditionalConversationOptions">
        /// </summary>
        /// <param name="exporterConfig">
        /// Configuration of commandline arguments
        /// </param>
        public AdditionalConversationOptions(ConversationExporterConfiguration exporterConfig)
        {
            exporterConfiguration = exporterConfig;
        }

        /// <summary>
        /// Apply all commandline argument options to conversation
        /// </summary>
        /// <param name="conversation">
        /// model representing the conversation
        /// </param>
        /// <param name="messages">
        /// All messages sent by users from conversation
        /// </param>
        /// <returns>
        /// A <see cref="Conversation"/> model representing the conversation. May return overloaded <see cref="Conversation"> instead
        /// </returns>
        public Conversation ApplyOptionsToConversation(Conversation conversation)
        {
            var messages = (List<Message>)conversation.messages;
            var conversationName = conversation.name;

            /// Filter by user name
            if (exporterConfiguration.FilterByUser != null)
            {
                messages = new FilterByName().Filter(messages, exporterConfiguration.FilterByUser);
            }

            /// Filter by keyword
            if (exporterConfiguration.FilterByWord != null)
            {
                messages = new FilterByWord().Filter(messages, exporterConfiguration.FilterByWord);
            }

            /// Blacklist words
            if (exporterConfiguration.Blacklist != null)
            {
                new ReplaceWithRedact().Replace(messages,exporterConfiguration.Blacklist.Split(','));
            }

            /// Generate Report
            if (exporterConfiguration.Report)
            {
                var messageCountPerUserReport = new MessageCountPerUserReportOption().GenerateReport(messages);
                return new Conversation(conversationName,messages,messageCountPerUserReport);
            }
            
            return new Conversation(conversationName, messages,null);
        }
    }
}

