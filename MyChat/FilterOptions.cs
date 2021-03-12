using System.Collections.Generic;

namespace MindLink.Recruitment.MyChat
{
    public abstract class FilterOptions
    {
        public abstract List<Message> Filter(List<Message> messages, string filterTarget);

    }

    public class FilterByName : FilterOptions
    {
        public override List<Message> Filter(List<Message> messages, string nameToFilter)
        {
            var filteredMessages = new List<Message>();

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