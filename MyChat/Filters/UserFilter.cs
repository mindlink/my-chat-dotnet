using System;
using System.Collections.Generic;
using System.Linq;
using MindLink.Recruitment.MyChat.Data;
using MindLink.Recruitment.MyChat.Exceptions;

namespace MindLink.Recruitment.MyChat.Filters
{
    public class UserFilter : IFilter
    {
        private readonly string[] _users;

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
            if (conversation == null)
            {
                throw new ArgumentNullException("There must be a conversation to filter.");
            }

            if (conversation.messages.Count() == 0)
            {
                throw new NoMessagesException("There must be at least one message to filter.");
            }

            if (_users.Length ==  0)
            {
                throw new NoUsersException("You must specify at least one user to filter.");
            }

            var newMessages = new List<Message>();

            foreach (var message in conversation.messages)
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
