using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyChat.Core.Managers;
using MindLink.Recruitment.MyChat.Helpers;
using MyChat.Core.Abstract;
using MyChat;
using MindLink.Recruitment.MyChat.Core;
using System.Linq;

namespace MindLink.Recruitment.MyChat.Tests
{
    [TestClass]
    public class FilteringTest
    {


        [TestMethod]
        Conversation getConversation(String[] args)
        {
            IOCManager.Register<IIOHelper>(() => new IOHelper());
            IOCManager.Register<ISerialize>(() => new Serializer());

             return new ConversationExporter().Export(args);            
        }

        [TestMethod]
        public void FilterTestWrongUser()
        {
            var ret = getConversation(new String[] {"chat.txt", "chat.json" , "-u" , "peter"});

            Assert.IsNotNull(ret.messages);
            Assert.IsTrue(!ret.messages.Any());
        }

        [TestMethod]
        public void FilterTestCorrectUser()
        {
            var ret = getConversation(new String[] { "chat.txt", "chat.json", "-u", "angus" });

            Assert.IsNotNull(ret.messages);
            Assert.IsTrue(ret.messages.Any());
        }

        [TestMethod]
        public void FilterTestIncorrectUserInput()
        {
            var ret = getConversation(new String[] { "chat.txt", "chat.json", "-u", null });

            Assert.IsNotNull(ret.messages);
            Assert.IsTrue(!ret.messages.Any());
        }


        [TestMethod]
        public void FilterTestExistingKeyword()
        {
            var ret = getConversation(new String[] { "chat.txt", "chat.json", "-k", "hello" });

            Assert.IsNotNull(ret.messages);
            Assert.IsTrue(ret.messages.Any());
        }

        [TestMethod]
        public void FilterTestMissingKeyword()
        {
            var ret = getConversation(new String[] { "chat.txt", "chat.json", "-k", "234asdfv345" });

            Assert.IsNotNull(ret.messages);
            Assert.IsTrue(!ret.messages.Any());
        }


        [TestMethod]
        public void FilterTestInvalidKeyword()
        {
            var ret = getConversation(new String[] { "chat.txt", "chat.json", "-k", null });

            Assert.IsNotNull(ret.messages);
            Assert.IsTrue(!ret.messages.Any());
        }

        [TestMethod]
        public void FilterTestExistingKeywordToHideWord()
        {
            string keyword = "hello";
            String hideTag = "*redacted*";

            var ret = getConversation(new String[] { "chat.txt", "chat.json", "-h", keyword });

            Assert.IsNotNull(ret.messages);
            Assert.IsTrue(ret.messages.Any());

            ret.messages.ToList().ForEach(curMessage=>Assert.IsTrue((!curMessage.content.Contains(hideTag) && !curMessage.content.Contains(keyword)) || (curMessage.content.Contains(hideTag) && !curMessage.content.Contains(keyword))));
                           
        }

        [TestMethod]
        public void FilterTestExistingKeywordToHideWordThatDoesNotExist()
        {
            string keyword = "646843548";
            String hideTag = "*redacted*";

            var ret = getConversation(new String[] { "chat.txt", "chat.json", "-h", keyword });

            Assert.IsNotNull(ret.messages);
            Assert.IsTrue(ret.messages.Any());

            ret.messages.ToList().ForEach(curMessage => Assert.IsTrue((!curMessage.content.Contains(hideTag) && !curMessage.content.Contains(keyword)) || (curMessage.content.Contains(hideTag) && !curMessage.content.Contains(keyword))));

        }

        [TestMethod]
        public void FilterTestExistingKeywordToHideWordThatIsInvalid()
        {
            string keyword = null;
            String hideTag = "*redacted*";

            var ret = getConversation(new String[] { "chat.txt", "chat.json", "-h", keyword });

            Assert.IsNotNull(ret.messages);
            Assert.IsTrue(!ret.messages.Any());

            ret.messages.ToList().ForEach(curMessage => Assert.IsTrue((!curMessage.content.Contains(hideTag) && !curMessage.content.Contains(keyword)) || (curMessage.content.Contains(hideTag) && !curMessage.content.Contains(keyword))));
        }
    }
}
