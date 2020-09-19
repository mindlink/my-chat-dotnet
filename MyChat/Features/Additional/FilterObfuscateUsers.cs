using MindLink.Recruitment.MyChat.Interfaces.FeatureInterfaces;
using MyChatModel.ModelData;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MindLink.Recruitment.MyChat.Features.Additional
{
    public sealed class FilterObfuscateUsers : IStrategyFilter
    {
        /// <summary>
        /// Dictionary to store original ID and an obfuscated ID
        /// original ID is key and obfuscated is value
        /// </summary>
        private IDictionary<string, string> userObfuscated;
        // int to keep count of obfusacted IDs and to make sure they're all unique
        private int countObfuscated;

        /// <summary>
        /// Constructor for FilterObfuscateUsers
        /// </summary>
        public FilterObfuscateUsers() 
        {
            userObfuscated = new Dictionary<string, string>();

            countObfuscated = 1;
        }

        public Conversation ApplyFilter(Conversation conversation) 
        {
            // FOREACH through the messages in the chat
            foreach (Message msg in conversation.Messages) 
            {
                // IF the dictionary already contains the user ID
                if (userObfuscated.ContainsKey(msg.SenderId))
                {
                    // SET the senderID of the message to the value contained at the user ID
                    msg.SenderId = userObfuscated[msg.SenderId];
                }
                else 
                {
                    // ELSE
                    // INITIALISE an int called parsed
                    int parsed = 0;
                    // FOREACH through the characters
                    // in the SenderID string
                    foreach (char c in msg.SenderId) 
                    {
                        // INCREMENT the parsed integer
                        // by the amount of the character converted to int32
                        parsed += Convert.ToInt32(c);
                    }
                    // ADD the SenderID string to the dictionary and add 
                    // the string User + <parsed value> + <count of users>
                    userObfuscated.Add(msg.SenderId, "User" + parsed.ToString() + countObfuscated.ToString());
                    // SET the SenderID to the value found at the SenderID in the dictionary
                    msg.SenderId = userObfuscated[msg.SenderId];
                    // INCREMENT the count by 1 
                    countObfuscated += 1;
                }
            }


            return conversation;
        }
    }
}
