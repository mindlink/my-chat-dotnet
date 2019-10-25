using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindLink.Recruitment.MyChat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindLink;

namespace MindLink.Recruitment.MyChat.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CommandLineArgumentParserTests
    {
        [TestMethod]
        public void ParseArgumentValueTests()
        {
            CommandLineArgumentParser argumentparser = new CommandLineArgumentParser();

            ConversationExporterConfiguration configuration = argumentparser.ParseCommandLineArguments("chat.txt chat.json -u cian".Split(' '));
            Assert.AreEqual("cian", configuration.userToFilter);

            configuration = argumentparser.ParseCommandLineArguments("chat.txt chat.json".Split(' '));
            Assert.AreEqual(null, configuration.userToFilter);
            Assert.AreEqual(null, configuration.keywordToFilter);

            configuration = argumentparser.ParseCommandLineArguments("chat.txt chat.json -u cian -k hello -mau".Split(' '));
            Assert.AreEqual("hello", configuration.keywordToFilter);
            Assert.AreEqual("cian", configuration.userToFilter);
            Assert.IsFalse(configuration.hideNumbers);
            Assert.IsFalse(configuration.obfuscateUserID);
            Assert.IsTrue(configuration.mostActiveUsersReport);

            configuration = argumentparser.ParseCommandLineArguments("chat.txt chat.json -k goodbye -hn -ouid".Split(' '));
            Assert.AreEqual("goodbye", configuration.keywordToFilter);
            Assert.AreEqual(null, configuration.userToFilter);
            Assert.IsTrue(configuration.hideNumbers);
            Assert.IsTrue(configuration.obfuscateUserID);
            Assert.IsFalse(configuration.mostActiveUsersReport);
        }
    }
}
