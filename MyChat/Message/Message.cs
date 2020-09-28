namespace MindLink.Recruitment.MyChat
{
    using System;

    public sealed class Message : IMessage
    {
        public DateTimeOffset timestamp { get; }
        
        public string senderId { get; }
        
        public string content { get;}

        public Message(string[] msg)
        {
            timestamp = StringToUnixTimeStamp(msg[0]);
            senderId = msg[1];
            content = string.Join(" ", msg[2..]);
        }

        public Message(DateTimeOffset ts, string sId, string c)
        {
            // Provide second constructor in the case you already have the data types ready
            // to go and you just need to assign them in one swoop.
            timestamp = ts;
            senderId = sId;
            content = c;
        }

        private DateTimeOffset StringToUnixTimeStamp(string s)
        {
            // StringToUnixTimeStamp does as its name suggests: it takes in a string and parses it to datetimeoffset.
            return DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(s));
        }
    }
}
