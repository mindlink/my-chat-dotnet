using System.Collections.Generic;

namespace MindLink.Recruitment.MyChat
{
    public abstract class FilterOptions
    {
        public abstract List<Message> Filter(List<Message> messages, ConversationExporterConfiguration exporterConfiguration);

    }

    public class FilterByName : FilterOptions
    {
        public override List<Message> Filter(List<Message> messages, ConversationExporterConfiguration exporterConfiguration)
        {
            var filteredMessages = new List<Message>();

            var nameToFilter = exporterConfiguration.FilterByUser;

            foreach (var message in messages)
            {
                if (message.senderId.Equals(nameToFilter))
                {
                    filteredMessages.Add(message);
                }
            }

            return filteredMessages;
        }
    }
}