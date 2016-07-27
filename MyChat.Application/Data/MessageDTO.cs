namespace MindLink.Recruitment.MyChat.Application.Data
{
    using Domain.Conversations;
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class MessageDTO: IEquatable<MessageDTO>
    {
        public MessageDTO()
        { }

        public MessageDTO(Message message)
        {
            SenderId = message.SenderId;
            Timestamp = message.Timestamp;
            Content = message.Content;
        }

        [DataMember(Name="senderId")]
        public string SenderId { get; set; }

        [DataMember(Name="timestap")]
        public DateTimeOffset Timestamp { get; set; }

        [DataMember(Name="content")]
        public string Content { get; set; }

        public bool Equals(MessageDTO other)
        {
            if (other == this)
                return true;

            if (other == null)
                return false;

            return (
                   SenderId == other.SenderId
                && Timestamp == other.Timestamp
                && Content == other.Content
            );
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as MessageDTO);
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 17) + (SenderId?.GetHashCode() ?? 0);
            hash = (hash * 17) + Timestamp.GetHashCode();
            hash = (hash * 17) + (Content?.GetHashCode() ?? 0);
            return hash;
        }
    }
}