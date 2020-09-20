namespace MindLink.Recruitment.MyChat.Controllers
{
    using MindLink.Recruitment.MyChat.Interfaces.ControllerInterfaces;
    using MyChatModel.ModelData;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Class to generate the report in the conversation
    /// </summary>
    public sealed class ReportController : IReportController
    {
        private IDictionary<string, int> usersCount;

        /// <summary>
        /// Constructor for ReportController class
        /// </summary>
        public ReportController() 
        {
             usersCount = new Dictionary<string, int>();
        }

        /// <summary>
        /// GenerateReport from the conversation 
        /// </summary>
        /// <param name="conversation"> <see cref="Conversation"> variable used for the report </param>
        /// <returns> the conversation with the generated report </returns>
        public Conversation GenerateReport(Conversation conversation) 
        {
            // CHECK if theres any messages to be reported
            if (conversation.Messages.Count() == 0)
            {
                // No messages, report this where the rankings would be 

                // INITIALISE an IList of type string, called empty list
                IList<string> emptyList = new List<string>();
                emptyList.Add("No users to be ranked");
                // INSANTIATE a new ConversationReport, passing into the constructor
                // the returns of the MostActive method and the MostActiveList method
                ConversationReport conversationReportEmpty = new ConversationReport(
                    "No users to report",
                    emptyList);
                // SET the Conversation parameters report
                conversation.Report = conversationReportEmpty;

                return conversation;
            }
            else
            {
                // INSANTIATE a new ConversationReport, passing into the constructor
                // the returns of the MostActive method and the MostActiveList method
                ConversationReport conversationReport = new ConversationReport(
                    MostActive(conversation.Messages.ToList()),
                    MostActiveList(conversation.Messages.ToList()));
                // SET the Conversation parameters report
                conversation.Report = conversationReport;

                return conversation;
            }
        }

        /// <summary>
        /// Method to sort the users into the most active order
        /// </summary>
        /// <param name="messages">List of messages to sort</param>
        /// <returns> a list of strings, which represent the user rankings</returns>
        private IList<string> MostActiveList(IList<Message> messages) 
        {
            // INITIALISE a list of type string
            IList<string> activeList = new List<string>();
            // initialise an int array, set the value to dictionaries values converted to an Array
            int[] msgCount = usersCount.Values.ToArray();
            // Array.Sort the int array
            Array.Sort(msgCount);

            // INTIALISE a dictionary type int, string called rankCount. message count is the key, users are the value
            IDictionary<int, string> rankCount = new Dictionary<int, string>();

            // ITERATE backwards through the array to start from the most messages sent
            for (int i = msgCount.Length - 1; i >= 0; i--) 
            {
                // FOREACH through the users in the dictionary
                foreach (string user in usersCount.Keys) 
                {
                    // IF the number of messages sent matches the number in the dictionary
                    if (msgCount[i] == usersCount[user]) 
                    {
                        if (rankCount.ContainsKey(msgCount[i]))
                        {
                            string rankCountUser = rankCount[msgCount[i]] + " and " + usersCount[user];
                        }
                        else 
                        {
                            rankCount.Add(msgCount[i], user);
                        }

                        // add a new string to the active list stating the rank, the user and the number of messages sent. 
                        activeList.Add(("Rank " + ((msgCount.Length - i)) + " is " + rankCount[msgCount[i]] + " with " + msgCount[i] + " messages"));
                        // need to handle if two users have the same number of messages sent
                    }
                }
            }

            return activeList;
        }

        private string MostActive(IList<Message> messages)
        { 
            // FOREACH through the messages in the list
            foreach(Message msg in messages) 
            {
                // IF the user is already in the dictionary
                if (usersCount.ContainsKey(msg.SenderId))
                    // THEN increment the integer found at the senderID in the dictionary
                    usersCount[msg.SenderId] += 1;
                // IF the senderID is not already in the dictionary
                if (!(usersCount.ContainsKey(msg.SenderId))) 
                {
                    // ADD the senderID to the dictionary and initialise its int to 1
                    usersCount.Add(msg.SenderId, 1);
                }
            }

            string mostActive = "";
            int userMsgNum = 0;

            // foreach through the dictionary
            foreach (string u in usersCount.Keys) 
            {
                // IF the count found at the user is greater than the current count
                if (usersCount[u] > userMsgNum)
                {
                    // SET the count to the new highest count
                    userMsgNum = usersCount[u];
                    // SET the user to the new most active user
                    mostActive = u;
                }
                // IF the two values are equal, make string joint
                else if (usersCount[u] == userMsgNum) 
                {
                    mostActive += " " + u;
                }
            }

            return mostActive;
        }
    }
}
