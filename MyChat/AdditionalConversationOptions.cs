using System.Collections.Generic;

namespace MindLink.Recruitment.MyChat
{
    public sealed class AdditionalConversationOptions
    {
        private ConversationExporterConfiguration exporterConfiguration;

        public AdditionalConversationOptions(ConversationExporterConfiguration exporterConfig)
        {
            exporterConfiguration = exporterConfig;
        }

        public Conversation ApplyOptionsToMessages(string conversationName, List<Message> messages)
        {
            var filteredMessages = messages;

            var filterByName = new FilterByName();

            filteredMessages = filterByName.Filter(filteredMessages,exporterConfiguration);

            return new Conversation(conversationName, filteredMessages);
        }
    }
}

