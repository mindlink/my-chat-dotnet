namespace MindLink.Recruitment.MyChat.Domain.Reporting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Displays the most active users based on their activity.
    /// </summary>
    public sealed class MostActiveUsersReport
    {
        private List<UserActivity> _userActivities;

        /// <summary>
        /// Initializes a <see cref="MostActiveUsersReport"/>
        /// </summary>
        public MostActiveUsersReport()
        {
            _userActivities = new List<UserActivity>();
        }

        /// <summary>
        /// All user activities.
        /// </summary>
        public IEnumerable<UserActivity> Activities
        {
            get { return _userActivities.AsReadOnly(); }
        }

        /// <summary>
        /// Adds a user activity in the report.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="numberOfMessages">The number of messages sent by the user.</param>
        public void AddUserActivity(string userId, int numberOfMessages = 1)
        {
            if (userId == null)
                throw new ArgumentNullException($"The value of {nameof(userId)} cannot be null.");

            if (numberOfMessages <= 0)
                throw new ArgumentOutOfRangeException($"The value of {nameof(numberOfMessages)} must be a positive value.");

            _userActivities.Add(new UserActivity(userId, numberOfMessages));
        }

        /// <summary>
        /// Generates the user activity collection which is sorted in descending order of the
        /// most active users.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserActivity> Generate()
        {
            return _userActivities
                .GroupBy(a => a.UserId, (userId, activities) => new UserActivity(userId, activities.Sum(a => a.NumberOfMessages)))
                .OrderByDescending(a => a.NumberOfMessages);
        }
    }
}
