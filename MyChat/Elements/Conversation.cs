using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat.Elements
{
   public class Conversation
    {
        public Conversation() {
        }
        public Conversation(string name) {
            this.name = name;
        }

      public class SenderInfo
        {
            public Sender sender;
            public int count=1;

            public SenderInfo(Sender sender) {
                this.sender = sender;
            }
            
        }

        /// <summary>
        /// The name of the conversation.
        /// </summary>
        private string name;

        private List<Message> messages = new List<Message>();
        private bool hideSenders = false;
        private Dictionary<String, SenderInfo> sendersInfos = new Dictionary<String, SenderInfo>();
        private List<SenderInfo> sendersInfoSorted = null;
        private Sender mostActiveSender = null;

        public void addMessage(DateTimeOffset timestamp, string senderId, string content) {
            Message myMsg;
            if (sendersInfos.ContainsKey(senderId))
            {

                (sendersInfos[senderId]).count++;
            }
            else
            {
                sendersInfos.Add(senderId, new SenderInfo(new Sender(senderId)));
            }

            myMsg = new Message(timestamp, sendersInfos[senderId].sender, content);
            messages.Add(myMsg);
        }


        /// <summary>
        /// Return the mostActiveSender or set the mostActiveSender
        /// </summary>
        public Sender MostActiveSender
        {
            get
            {
                return mostActiveSender;
            }
            set
            {
                mostActiveSender = value;
            }
        }


        /// <summary>
        /// Return the SenderInfo or set the SenderInfo dictionary
        /// </summary>
        public Dictionary<String, SenderInfo> SendersInfos
        {
            get
            {
                return sendersInfos;
            }
            set
            {
                sendersInfos = value;
            }
        }

        /// <summary>
        /// Return the sendersInfo sorded by amount of messages or set the SendersInfoSorted
        /// </summary>
        public List<SenderInfo> SendersInfoSorted
        {
            get
            {
                return sendersInfoSorted;
            }
            set
            {
                sendersInfoSorted = value;
            }
        }

        /// <summary>
        /// Return the Messages or set the Messages
        /// </summary>
        public List<Message> Messages
        {
            get
            {
                return messages;
            }
            set
            {
                messages = value;
            }
        }

        /// <summary>
        /// Return the HideSenders or set the HideSenders
        /// </summary>
        public Boolean HideSenders
        {
            get
            {
                return hideSenders;
            }
            set
            {
                hideSenders = value;
            }
        }


        /// <summary>
        /// Return the name
        /// </summary>
        public String Name
        {
            get
            {
                return name;
            }
        }
    }
}
