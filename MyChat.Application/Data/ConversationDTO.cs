namespace MindLink.Recruitment.MyChat.Application.Data
{
    using Domain.Conversations;
    using Domain.Reporting;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    [DataContract]
    public class ConversationDTO: IEquatable<ConversationDTO>
    {
        public ConversationDTO()
        { }

        public ConversationDTO(Conversation conversation)
            : this(conversation, null)
        { }

        public ConversationDTO(Conversation conversation, IEnumerable<UserActivity> mostActiveUsers)
        {
            Name = conversation.Name;
            Messages = conversation.Messages.Select(m => new MessageDTO(m));
            MostActiveUsers = mostActiveUsers?.Select(a => new UserActivityDTO(a));
        }

        [DataMember(Name="name")]
        public string Name { get; set; }

        [DataMember(Name="messages")]
        public IEnumerable<MessageDTO> Messages { get; set; }

        [DataMember(Name="mostActiveUsers", EmitDefaultValue = false)]
        public IEnumerable<UserActivityDTO> MostActiveUsers { get; set; }

        public bool Equals(ConversationDTO other)
        {
            if (other == this)
                return true;

            if (other == null)
                return false;

            if (Name != other.Name)
                return false;

            if (Messages != other.Messages)
            {
                if (Messages != null && other.Messages == null
                    || Messages == null && other.Messages != null
                    || !Enumerable.SequenceEqual(Messages, other.Messages))
                {
                    return false;
                }
            }

            if (MostActiveUsers != other.MostActiveUsers)
            {
                if (MostActiveUsers != null && other.MostActiveUsers == null
                    || MostActiveUsers == null && other.MostActiveUsers != null
                    || !Enumerable.SequenceEqual(MostActiveUsers, other.MostActiveUsers))
                {
                    return false;
                }
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ConversationDTO);
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 17) + (Name?.GetHashCode() ?? 0);

            foreach (var message in Messages)
                hash = (hash * 17) + (message?.GetHashCode() ?? 0);

            foreach (var activity in MostActiveUsers)
                hash = (hash * 17) + (activity?.GetHashCode() ?? 0);

            return hash;
        }
    }
}
