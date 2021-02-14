using System.Collections.Generic;
using System.Linq;

namespace MindLink.Recruitment.MyChat.Filters
{
    public class UserFilter : IFilter
    {
        private string[] _users;

        public UserFilter(string[] users)
        {
            _users = users;
        }

        /// <summary>
        /// Returns all messages by the specified user.
        /// </summary>
        /// <inheritdoc />
        public Conversation Filter(Conversation conversation)
        {
            var newMessages = new List<Message>();

            foreach(var message in conversation.messages)
            {
                foreach (var user in _users)
                {
                    if (message.senderId == user)
                    {
                        newMessages.Add(message);
                    }
                }
            }
            return new Conversation(conversation.name, newMessages);
        }
    }
}
