using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat.Elements
{
   public class Message
    {

        private String content;
        private DateTimeOffset timestamp;
        private Sender sender;

        /// <summary>
        /// Initializes a new instance of the Message class.
        /// </summary>
        /// <param name="timestamp">
        /// The message timestamp.
        /// </param>
        /// <param name="senderId">
        /// The ID of the sender.
        /// </param>
        /// <param name="content">
        /// The message content.
        /// </param>
        public Message(DateTimeOffset timestamp, Sender sender, string content)
        {
            //if name is empty we throw an exception
            if (sender == null)
            {
                throw new ArgumentNullException("senderId", String.Format("Exception in {0}, Error message : {1}",
                        this.GetType().Name + "." +System.Reflection.MethodBase.GetCurrentMethod().Name, "Sender can not be null when creating a new message"));
            }

            // /if timestamp is null we throw an exception
            if (timestamp == null)
            {
                throw new ArgumentNullException("timestamp", String.Format("Exception in {0}, Error message : {1}",
                        this.GetType().Name + "." +System.Reflection.MethodBase.GetCurrentMethod().Name, "timestamp can not be null when creating a new message"));
            }

            // /if content is null we throw an exception
            if (content == null)
            {
                throw new ArgumentNullException("content", String.Format("Exception in {0}, Error message : {1}",
                        this.GetType().Name + "." +System.Reflection.MethodBase.GetCurrentMethod().Name, "content can not be null when creating a new message"));
            }
            this.content = content;
            this.timestamp = timestamp;
            this.sender = sender;
        }

        /// <summary>
        /// Return the sender
        /// </summary>
        public Sender msgSender  
        {
            get
            {
                return sender;
            }
        }

        /// <summary>
        /// Return the Timestamp
        /// </summary>
        public DateTimeOffset Timestamp
        {
            get
            {
                return timestamp;
            }
        }

        /// <summary>
        /// Return the content or set the content
        /// </summary>
        public String Content
        {
            get
            {
                return content;
            }
            set
            {
                content = value;
            }
        }


    }
}
