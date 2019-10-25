using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat
{
    public sealed class UserList
    {
        private List<User> listOfUsers;

        public UserList ()
        {
            this.listOfUsers = new List<User>();
        }

        /// <summary>
        ///  Allow use of user_list_name["userID"] syntax to retrieve User 
        ///  elements from the list.
        /// </summary>
        /// <param name="userID">
        /// The user ID to retrieve
        /// </param>
        /// <exception cref="IndexOutOfRangeException">
        /// Thrown if the requested user is not found in the user list.
        /// </exception>
        /// <returns>
        /// The User object with a user ID matching the userID parameter
        /// </returns>
        public User this[string userID]
        {
            get
            {
                User found_user = this.FindUser(userID);

                if (found_user != null)
                {
                    return found_user;
                } 
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// Returns a List<User> object containing the User objects in the
        /// user list.
        /// </summary>
        public List<User> ToList ()
        {
            return this.listOfUsers;
        }

        /// <summary>
        /// Private method for finding a user with a given user ID in the user list.
        /// </summary>
        /// <param name="userID">
        /// The user ID to find.
        /// </param>
        /// <returns>
        /// If the user is found returns the corresponding User object,
        /// if not returns null.
        /// </returns>
        private User FindUser (string userID)
        {
            foreach (var user in this.listOfUsers)
            {
                if (user.userID == userID)
                {
                    return user;
                }
            }

            return null;
        }

        /// <summary>
        /// Checks if a user with a given user ID exists in the user list.
        /// </summary>
        /// <param name="userID">The user ID to find</param>
        /// <returns>
        /// True if the user is found, false if the user is not present within the 
        /// user list.
        /// </returns>
        public bool Contains (string userID)
        {
            if (this.FindUser(userID) != null)
            { 
                return true;
            } else {
                return false;
            }
        }

        /// <summary>
        /// Adds a user to the user list.
        /// </summary>
        /// <param name="user">
        /// The user object to add to the list.
        /// </param>
        public void Add (User user)
        {
            this.listOfUsers.Add(user);
        }

        /// <summary>
        /// Sorts the users in the user list by the number of messages they have sent
        /// in descending order. 
        /// </summary>
        public void SortByActivity ()
        {
            this.listOfUsers = this.listOfUsers.OrderByDescending(user => user.numberOfMessages).ToList();
        }
    }
}
