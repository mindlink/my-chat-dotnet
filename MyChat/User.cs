using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat
{
    /// <summary>
    /// Represents a Chat User.
    /// </summary>
    public sealed class User
    {
        /// <summary>
        /// User ID
        /// </summary>
        public int UserId;

        /// <summary>
        /// User Name
        /// </summary>
        public string UserName;

        /// <summary>
        /// User Chats
        /// </summary>
        public int UserChats;
    }



    /// <summary>
    /// Represents all Chat Users.
    /// </summary>
    public sealed class Users
    {
        /// <summary>
        /// User Data
        /// </summary>
        public List<User> UserList = new List<User>();

        /// <summary>
        /// Last ID
        /// </summary>
        private int lastId = 1;

        /// <summary>
        /// add a new user to the UserList
        /// </summary>
        /// <param name="username">
        /// The User Name.
        /// </param>
        public int addUser(string username)
        {
            var user = new User();
            user.UserName = username;
            int id = user.UserId = createUserId(username);

            UserList.Add(user);

            return id;
        }


        /// <summary>
        /// Returns the User ID. If the User exists, 
        /// If not, create the user and return the ID after
        /// </summary>
        /// <returns>
        /// a Unique ID
        /// </returns>
        private int createUserId(string username)
        {
            var res = UserList.Where(s => s.UserName == username);
            if (res.Count() == 0)
            {
                try
                {
                    return lastId++;
                }
                // first user will fail, so return ID == 1
                catch (InvalidOperationException)
                {
                    return lastId = 1;
                }
            }

            return res.Last().UserId++;
        }
    }
}
