using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyChat.Core.Managers;
using MyChat.Core.Abstract;
using MindLink.Recruitment.MyChat.Helpers;
using MindLink.Recruitment.MyChat.Core;

namespace MindLink.Recruitment.MyChat.Tests
{
    [TestClass]
    public class DataManagerTest
    {
        [TestMethod]
        public void DataManagerTestReadConversation()
        {
            IOCManager.Register<ISerialize>(() => new Serializer());         
            IOCManager.Register<IIOHelper>(() => new IOHelper());

            var dm = DataManager.Instance;


            Conversation cov = dm.ReadConversation("chat.txt");

            Assert.IsNotNull(cov);
        }

    }
}
