namespace MindLink.Recruitment.MyChat.Application.Test.Data
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Generic;
    using Application.Data;

    [TestClass]
    public class UserActivityDTOTest
    {
        [TestMethod]
        public void It_performs_structural_equality()
        {

            var testCases = new List<dynamic>
            {
                new {
                    First = new UserActivityDTO { UserId = "UserId", NumberOfMessages = 1 },
                    Second = new UserActivityDTO { UserId = "UserId", NumberOfMessages = 1 },
                    Expected = true
                },
                new {
                    First = new UserActivityDTO { UserId = null, NumberOfMessages = 1 },
                    Second = new UserActivityDTO { UserId = null, NumberOfMessages = 1 },
                    Expected = true
                },
                new {
                    First = new UserActivityDTO { UserId = "UserId1", NumberOfMessages = 1 },
                    Second = new UserActivityDTO { UserId = "UserId", NumberOfMessages = 1 },
                    Expected = false
                },
                new {
                    First = new UserActivityDTO { UserId = "UserId", NumberOfMessages = 2 },
                    Second = new UserActivityDTO { UserId = "UserId", NumberOfMessages = 1 },
                    Expected = false
                },
            };

            foreach (var tc in testCases)
                Assert.AreEqual(tc.Expected, EqualityComparer<UserActivityDTO>.Default.Equals(tc.First, tc.Second));
        }
    }
}
