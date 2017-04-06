namespace MindLink.Recruitment.MyChat
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Helper class responsible for initializing and setting the UserActivity object in Conversation.
    /// </summary>
    public static class Activity
    {
        /// <summary>
        /// Populates userActivity list within Conversation.
        /// </summary>
        /// <param name="conversation">
        /// The Conversation object to add the activity list to.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when conversation is null or the conversation messages is null.
        /// </exception>
        public static void SetUserActivityInConversation(Conversation conversation)
        {
            if (conversation == null)
            {
                throw new ArgumentNullException("conversation", "Conversation was null. Could not set User Activity");
            }

            if (conversation.Messages == null)
            {
                throw new ArgumentNullException("conversation", "Conversation did not contain any messages.");
            }

            // initialize UserActivity list.
            var activityList = new List<UserActivity>();

            // using linq to group messages by user and order them by message count.
            var activity = conversation.Messages.GroupBy(message => message.SenderId)
                                .OrderByDescending(group => group.Count());

            foreach (var user in activity)
            {
                // instanciate UserActivity object.
                UserActivity usrAct = new UserActivity(user.Key, user.Count());

                // add UserActivity object in List.
                activityList.Add(usrAct);
            }

            // Set the created list to the conversation's property.
            conversation.UserActivity = activityList;
        }
    }
}
