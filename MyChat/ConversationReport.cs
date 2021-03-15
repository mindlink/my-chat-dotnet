using System;
using System.Collections.Generic;

namespace MindLink.Recruitment.MyChat
{
    public sealed class ConversationReport : Conversation
    {
        public List<SenderCounter> activity { get; set; }

        public ConversationReport(string name, IEnumerable<Message> messages) : base(name, messages)
        {
            activity = new List<SenderCounter>();
        }

        public void GenerateReportData()
        {
            foreach (var item in messages)
            {
                bool logged = false;
                for (int i = 0; i < activity.Count; i++)
                {
                    if (activity[i].sender.Equals(item.senderId))
                    {
                        activity[i].IncrementCounter();
                        logged = true;
                        break;
                    }
                }

                if (!logged)
                {
                    activity.Add(new SenderCounter(item.senderId, 1));
                }
            }

        }

        public void SortDataAscending()
        {
            activity.Sort((x, y) => y.count.CompareTo(x.count));
        }
    }

    [Serializable]
    public class SenderCounter
    {

        public string sender;
        public int count;

        public SenderCounter(string sender, int count)
        {
            this.sender = sender;
            this.count = count;
        }

        public void IncrementCounter()
        {
            count++;
        }
    }
}
