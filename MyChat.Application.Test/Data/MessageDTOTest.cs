namespace MindLink.Recruitment.MyChat.Application.Test.Data
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Application.Data;
    using System;
    using System.Collections.Generic;

    [TestClass]
    public class MessageDTOTest
    {
        [TestMethod]
        public void It_performs_structural_equality()
        {
            var testCases = new List<dynamic>
            {
                new {
                    First = new MessageDTO { SenderId = "SenderId", Content = "Content", Timestamp = new DateTime(2000, 1, 1) },
                    Second = new MessageDTO { SenderId = "SenderId", Content = "Content", Timestamp = new DateTime(2000, 1, 1) },
                    Expected = true
                },
                new {
                    First = new MessageDTO { SenderId = null, Content = null, Timestamp = new DateTime(2000, 1, 1) },
                    Second = new MessageDTO { SenderId = null, Content = null, Timestamp = new DateTime(2000, 1, 1) },
                    Expected = true
                },
                new {
                    First = new MessageDTO { SenderId = "SenderId1", Content = "Content", Timestamp = new DateTime(2000, 1, 1) },
                    Second = new MessageDTO { SenderId = "SenderId", Content = "Content", Timestamp = new DateTime(2000, 1, 1) },
                    Expected = false
                },
                new {
                    First = new MessageDTO { SenderId = "SenderId", Content = "Content1", Timestamp = new DateTime(2000, 1, 1) },
                    Second = new MessageDTO { SenderId = "SenderId", Content = "Content", Timestamp = new DateTime(2000, 1, 1) },
                    Expected = false
                },
                new {
                    First = new MessageDTO { SenderId = "SenderId", Content = "Content", Timestamp = new DateTime(2000, 1, 1) },
                    Second = new MessageDTO { SenderId = "SenderId", Content = "Content", Timestamp = new DateTime(2000, 1, 2) },
                    Expected = false
                },
            };

            foreach (var tc in testCases)
                Assert.AreEqual(tc.Expected, EqualityComparer<MessageDTO>.Default.Equals(tc.First, tc.Second));
        }
    }
}
