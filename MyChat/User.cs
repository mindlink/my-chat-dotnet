using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat
{
    public sealed class User
    {
        /// <summary>
        /// The ID of the user
        /// </summary>
        public string userID;

        /// <summary>
        /// The number of messages sent by the user in a given 
        /// conversation.
        /// </summary>
        public int numberOfMessages;

        /// <param name="userID">
        /// The ID of the user
        /// </param>
        /// <param name="numberOfMessages">
        /// Number of messages sent by the user in a given
        /// conversation.
        /// </param>
        public User (string userID, int numberOfMessages)
        {
            // Need to throw exceptions if these aren't specified
            this.userID = userID;
            this.numberOfMessages = numberOfMessages;
        }

        /// <summary>
        /// Increases the number of messages sent by the user by 1.
        /// </summary>
        public void IncrementMessageCount()
        {
            this.numberOfMessages++;
        }
    }
}
