using System.ComponentModel.Design;

namespace MindLink.Recruitment.MyChat
{
    public interface IAdjuster
    {
        public IAdjuster NextAdjuster { get; set; }

        public IMessage Adjust(IMessage m);
    }
}
