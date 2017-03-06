using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindLink.Recruitment.MyChat;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using MyChatLibrary;

namespace MindLink.Recruitment.MyChat.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for the <see cref="ConversationExporter"/>.
    /// </summary>
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public void CreateAndReadNewUser()
        {
            int iMax = 51;
            Users _users = this.createUserlist();

            for (int i = 0; i < iMax; i++)
            {
                Assert.AreNotEqual(0, _users.UserList[i].UserId);
                Console.WriteLine("User Name == " + _users.UserList[i].UserName + "\tnew UserId == " + _users.UserList[i].UserId);
            }
        }


        [TestMethod()]
        public void addUsergetUserId()
        {
            var _users = new Users();

            Console.WriteLine("the new User ID is: {0}", _users.addUser("Sunshine"));
            Assert.AreNotEqual("0", _users.addUser("Sunshine"));
            Assert.AreNotEqual(null, _users.addUser("Sunshine"));
        }

        [TestMethod()]
        public void addUsergetUserIdInList()
        {
            var _users = this.createUserlist();

            Console.WriteLine("the new User ID is: {0}", _users.addUser("Maximilian"));
            Console.WriteLine("the new User Name is: {0}", _users.UserList.ElementAt(_users.UserList.Count - 1).UserName);
            Assert.AreEqual("Maximilian", _users.UserList.ElementAt(_users.UserList.Count - 1).UserName);

            Console.WriteLine("the new User ID is: {0}", _users.addUser("Nicos"));
            Console.WriteLine("the new User Name is: {0}", _users.UserList.ElementAt(_users.UserList.Count - 1).UserName);
            Assert.AreEqual("Nicos", _users.UserList.ElementAt(_users.UserList.Count - 1).UserName);
        }


        private Users createUserlist()
        {
            int iMax = 51;
            Random ra1 = new Random(DateTime.Now.Millisecond);
            Random ra2 = new Random((int)DateTime.Now.Ticks);
            Random ra3 = new Random(DateTime.Now.Millisecond + 2000);
            Random ra4 = new Random(DateTime.Now.Millisecond + 4000);
            Random ra5 = new Random(DateTime.Now.Millisecond + 1500);

            Users _users = new Users();

            for (int i = 0; i < iMax; i++)
            {
                if (i != ra1.Next(1, 49) && i != ra2.Next(1, 49) && i != ra3.Next(1, 49) && i != ra4.Next(1, 49) && i != ra5.Next(1, 49))
                    _users.addUser("snowflake_" + i);
                else  // add an existing user
                    _users.addUser("sunshine");
            }

            return _users;
        }
    }
}
