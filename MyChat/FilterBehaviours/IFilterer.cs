using System;

namespace MindLink.Recruitment.MyChat
{
    public interface IFilterer
    {
        // So we can eventually setup a chain of handlers that we go through. 
        public IFilterer NextFilterer { get; set; }

        public bool Filter(IMessage m);
    }
}