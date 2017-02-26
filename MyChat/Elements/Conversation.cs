using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat.Elements
{
   public class Conversation
    {

       
        /// <summary>
        /// Constractor
        /// </summary>
        /// <param name="name"></param>
        public Conversation(string name) {
            //if name is empty we throw an exception
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name", String.Format("Exception in {0}, Error message : {1}",
                        this.GetType().Name + "." +System.Reflection.MethodBase.GetCurrentMethod().Name, "Conversation name can not be empty when creating new conversation"));
            }
            this.name = name;
        }

        /// <summary>
        /// store the number of messages a sender has in this conversation
        /// </summary>
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

        /// <summary>
        /// Adds new message to the conversation
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="senderId"></param>
        /// <param name="content"></param>
        public void addMessage(DateTimeOffset timestamp, string senderId, string content) {
            Message myMsg;

            //if name is empty we throw an exception
            if (String.IsNullOrWhiteSpace(senderId))
            {
                throw new ArgumentNullException("senderId", String.Format("Exception in {0}, Error message : {1}",
                        this.GetType().Name + "." +System.Reflection.MethodBase.GetCurrentMethod().Name, "SenderId can not be empty when adding a new message to the conversation"));
            }

            // /if timestamp is null we throw an exception
            if (timestamp==null)
            {
                throw new ArgumentNullException("timestamp", String.Format("Exception in {0}, Error message : {1}",
                        this.GetType().Name + "." +System.Reflection.MethodBase.GetCurrentMethod().Name, "timestamp can not be null when adding a new message to the conversation"));
            }

            // /if content is null we throw an exception
            if (content == null)
            {
                throw new ArgumentNullException("content", String.Format("Exception in {0}, Error message : {1}",
                        this.GetType().Name + "." +System.Reflection.MethodBase.GetCurrentMethod().Name, "content can not be null when adding a new message to the conversation"));
            }

            //check if a sender with this id exist in the conversation
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
