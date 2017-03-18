using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat
{
   public sealed class ActiveUser
   {
        /// <summary>
        /// The name of a user being printed on the JSON report.
        /// </summary>
       public string UserId;

        /// <summary>
        /// The number of total messages that a user has send or "Activity" of a user.
        /// </summary>
       public string TotalOfMsgs;

        /// <summary>
        /// Initializes a new instance of an user with its activity data.
        /// </summary>
        /// <param name="userId">
        /// The senderId of a chat user.
        /// </param>
        /// <param name="totalOfMsgs">
        /// The number of messages sent by that corresponding user.
        /// </param>
        public ActiveUser(string userId, string totalOfMsgs)
        {
            this.UserId = userId;
            this.TotalOfMsgs = totalOfMsgs;
        }
    }
}
