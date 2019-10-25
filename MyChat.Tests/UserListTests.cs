using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindLink.Recruitment.MyChat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat.Tests
{
    [TestClass]
    public class UserListTests
    {
        UserList test_list;

        [TestInitialize]
        public void SetupTests ()
        {
            this.test_list = new UserList();
        }

        [TestMethod]
        public void AddingUsersAndRetrievingUsersTests ()
        {
            this.test_list.Add(new User("cian", 1));

            List<User> results_list = this.test_list.ToList();

            Assert.AreEqual("cian", results_list[0].userID);
            Assert.AreEqual(1, results_list[0].numberOfMessages);
            Assert.AreEqual(results_list.ToArray().Length, 1);
            Assert.AreEqual(this.test_list["cian"].userID, "cian");
        }

        [TestMethod]
        public void ContainsTests ()
        {
            this.test_list.Add(new User("cian", 1));

            Assert.IsTrue(test_list.Contains("cian"));
            Assert.IsFalse(test_list.Contains("bob"));
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException), "Found user with ID 'cian' in an empty list")]
        public void AccessingUserNotInListFails ()
        {
            UserList test_list = new UserList();
            Console.WriteLine(test_list["cian"]);
        }

        [TestMethod]
        public void ToListTests ()
        {
            this.test_list.Add(new User("cian", 1));
            this.test_list.Add(new User("jane", 2));

            List<User> results_list = this.test_list.ToList();

            Assert.AreEqual("cian", results_list[0].userID);
            Assert.AreEqual(1, results_list[0].numberOfMessages);
            Assert.AreEqual("jane", results_list[1].userID);
            Assert.AreEqual(2, results_list[1].numberOfMessages);
        }
    }
}