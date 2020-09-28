using System;

namespace MindLink.Recruitment.MyChat
{
    public class UserNameFilterer : IFilterer
    {
        public string UserNameToFind { set; get; }
        public IFilterer NextFilterer { get; set; }

        public UserNameFilterer(string user)
        {
            UserNameToFind = user;
        }
        
        public bool Filter(IMessage m)
        {
            var found = UsernameFound(m); 
            
            if (found && NextFilterer != null)
            {
                return NextFilterer.Filter(m);
            }

            return found;
        }

        public bool UsernameFound(IMessage m)
        {
            return m.senderId == UserNameToFind;
        }
    }
}