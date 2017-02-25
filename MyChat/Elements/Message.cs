using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat.Elements
{
   public class Message
    {
        /// <summary>
        /// The message content.
        /// </summary>
        private String content;

        /// <summary>
        /// The message timestamp.
        /// </summary>
        private DateTimeOffset timestamp;

        /// <summary>
        /// The message sender.
        /// </summary>
        private Sender sender;

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
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
