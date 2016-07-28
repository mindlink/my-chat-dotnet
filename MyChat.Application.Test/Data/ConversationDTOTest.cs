using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using MindLink.Recruitment.MyChat.Application.Data;

namespace MindLink.Recruitment.MyChat.Application.Test.Data
{
    [TestClass]
    public class ConversationDTOTest
    {
        [TestMethod]
        public void It_performs_structural_equality()
        {
            var testCases = new List<dynamic>
            {
                // same
                new
                {
                    First = new ConversationDTO
                    {
                        Name = "Name",
                        Messages = new List<MessageDTO>
                        {
                            new MessageDTO { SenderId = "SenderId", Content = "Content", Timestamp = new DateTime(2000, 1, 1) },
                        },
                        MostActiveUsers = new List<UserActivityDTO>
                        {
                            new UserActivityDTO { UserId = "SenderId", NumberOfMessages = 1 }
                        }
                    },
                    Second = new ConversationDTO
                    {
                        Name = "Name",
                        Messages = new List<MessageDTO>
                        {
                            new MessageDTO { SenderId = "SenderId", Content = "Content", Timestamp = new DateTime(2000, 1, 1) },
                        },
                        MostActiveUsers = new List<UserActivityDTO>
                        {
                            new UserActivityDTO { UserId = "SenderId", NumberOfMessages = 1 }
                        }
                    },
                    Expected = true
                },
                // different name
                new
                {
                    First = new ConversationDTO
                    {
                        Name = "Name1",
                        Messages = new List<MessageDTO>
                        {
                            new MessageDTO { SenderId = "SenderId", Content = "Content", Timestamp = new DateTime(2000, 1, 1) },
                        },
                        MostActiveUsers = new List<UserActivityDTO>
                        {
                            new UserActivityDTO { UserId = "SenderId", NumberOfMessages = 1 }
                        }
                    },
                    Second = new ConversationDTO
                    {
                        Name = "Name",
                        Messages = new List<MessageDTO>
                        {
                            new MessageDTO { SenderId = "SenderId", Content = "Content", Timestamp = new DateTime(2000, 1, 1) },
                        },
                        MostActiveUsers = new List<UserActivityDTO>
                        {
                            new UserActivityDTO { UserId = "SenderId", NumberOfMessages = 1 }
                        }
                    },
                    Expected = false
                },
                // different message
                new
                {
                    First = new ConversationDTO
                    {
                        Name = "Name",
                        Messages = new List<MessageDTO>
                        {
                            new MessageDTO { SenderId = "SenderId1", Content = "Content", Timestamp = new DateTime(2000, 1, 1) },
                        },
                        MostActiveUsers = new List<UserActivityDTO>
                        {
                            new UserActivityDTO { UserId = "SenderId", NumberOfMessages = 1 }
                        }
                    },
                    Second = new ConversationDTO
                    {
                        Name = "Name",
                        Messages = new List<MessageDTO>
                        {
                            new MessageDTO { SenderId = "SenderId", Content = "Content", Timestamp = new DateTime(2000, 1, 1) },
                        },
                        MostActiveUsers = new List<UserActivityDTO>
                        {
                            new UserActivityDTO { UserId = "SenderId", NumberOfMessages = 1 }
                        }
                    },
                    Expected = false
                },
                // different number of messages
                new
                {
                    First = new ConversationDTO
                    {
                        Name = "Name",
                        Messages = new List<MessageDTO>
                        {
                            new MessageDTO { SenderId = "SenderId", Content = "Content", Timestamp = new DateTime(2000, 1, 1) },
                            new MessageDTO { SenderId = "SenderId", Content = "Content", Timestamp = new DateTime(2000, 1, 1) },
                        },
                        MostActiveUsers = new List<UserActivityDTO>
                        {
                            new UserActivityDTO { UserId = "SenderId", NumberOfMessages = 1 }
                        }
                    },
                    Second = new ConversationDTO
                    {
                        Name = "Name",
                        Messages = new List<MessageDTO>
                        {
                            new MessageDTO { SenderId = "SenderId", Content = "Content", Timestamp = new DateTime(2000, 1, 1) },
                        },
                        MostActiveUsers = new List<UserActivityDTO>
                        {
                            new UserActivityDTO { UserId = "SenderId", NumberOfMessages = 1 }
                        }
                    },
                    Expected = false
                },
                // different user activity
                new
                {
                    First = new ConversationDTO
                    {
                        Name = "Name",
                        Messages = new List<MessageDTO>
                        {
                            new MessageDTO { SenderId = "SenderId", Content = "Content", Timestamp = new DateTime(2000, 1, 1) },
                        },
                        MostActiveUsers = new List<UserActivityDTO>
                        {
                            new UserActivityDTO { UserId = "SenderId1", NumberOfMessages = 1 }
                        }
                    },
                    Second = new ConversationDTO
                    {
                        Name = "Name",
                        Messages = new List<MessageDTO>
                        {
                            new MessageDTO { SenderId = "SenderId", Content = "Content", Timestamp = new DateTime(2000, 1, 1) },
                        },
                        MostActiveUsers = new List<UserActivityDTO>
                        {
                            new UserActivityDTO { UserId = "SenderId", NumberOfMessages = 1 }
                        }
                    },
                    Expected = false
                },
                // different number of user activities
                new
                {
                    First = new ConversationDTO
                    {
                        Name = "Name",
                        Messages = new List<MessageDTO>
                        {
                            new MessageDTO { SenderId = "SenderId", Content = "Content", Timestamp = new DateTime(2000, 1, 1) },
                        },
                        MostActiveUsers = new List<UserActivityDTO>
                        {
                            new UserActivityDTO { UserId = "SenderId1", NumberOfMessages = 1 },
                            new UserActivityDTO { UserId = "SenderId1", NumberOfMessages = 1 }
                        }
                    },
                    Second = new ConversationDTO
                    {
                        Name = "Name",
                        Messages = new List<MessageDTO>
                        {
                            new MessageDTO { SenderId = "SenderId", Content = "Content", Timestamp = new DateTime(2000, 1, 1) },
                        },
                        MostActiveUsers = new List<UserActivityDTO>
                        {
                            new UserActivityDTO { UserId = "SenderId", NumberOfMessages = 1 }
                        }
                    },
                    Expected = false
                }
            };

            foreach (var tc in testCases)
                Assert.AreEqual(tc.Expected, EqualityComparer<ConversationDTO>.Default.Equals(tc.First, tc.Second));
        }
    }
}
