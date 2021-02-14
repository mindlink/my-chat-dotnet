using System.Linq;

namespace MindLink.Recruitment.MyChat.Filters
{
    public class Report : IFilter
    {
        /// <summary>
        /// Displays how many messages each user sent in descending order.
        /// </summary>
        /// <inheritdoc />
        public Conversation Filter(Conversation conversation)
        {
            conversation.activity = conversation.messages
                .Select(message => { return message.senderId; })
                .Distinct()
                .Select(senderId => { return new Activity(senderId, conversation.messages.Count(message => { return message.senderId == senderId; })); })
                .OrderByDescending(activity => { return activity.count; });

            return conversation;
        }
    }
}
