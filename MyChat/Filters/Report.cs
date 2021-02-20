using System;
using System.Linq;
using MindLink.Recruitment.MyChat.Data;
using MindLink.Recruitment.MyChat.Exceptions;

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
            if (conversation == null)
            {
                throw new ArgumentNullException("There must be a conversation to report on.");
            }

            if (conversation.messages.Count() == 0)
            {
                throw new NoMessagesException("There must be at least one message to issue a report.");
            }

            conversation.activity = conversation.messages
                    .Select(message => { return message.senderId; })
                    .Distinct()
                    .Select(senderId => { return new Activity(senderId, conversation.messages.Count(message => { return message.senderId == senderId; })); })
                    .OrderByDescending(activity => { return activity.count; });

            var newConversation = conversation;

            return newConversation;
        }
    }
}
