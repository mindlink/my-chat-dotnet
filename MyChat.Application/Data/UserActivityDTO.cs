namespace MindLink.Recruitment.MyChat.Application.Data
{
    using Domain.Reporting;
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class UserActivityDTO: IEquatable<UserActivityDTO>
    {
        public UserActivityDTO()
        { }

        public UserActivityDTO(UserActivity userActivity)
        {
            UserId = userActivity.UserId;
            NumberOfMessages = userActivity.NumberOfMessages;
        }

        [DataMember(Name="userId")]
        public string UserId { get; set; }

        [DataMember(Name="numberOfMessages")]
        public int NumberOfMessages { get; set; }

        public bool Equals(UserActivityDTO other)
        {
            if (other == this)
                return true;

            if (other == null)
                return false;

            return (
                   UserId == other.UserId
                && NumberOfMessages == other.NumberOfMessages
            );
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as UserActivityDTO);
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 17) + (UserId?.GetHashCode() ?? 0);
            hash = (hash * 17) + NumberOfMessages.GetHashCode();
            return hash;
        }
    }
}
