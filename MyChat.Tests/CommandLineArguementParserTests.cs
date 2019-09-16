using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MindLink.Recruitment.MyChat.Tests
{
    [TestClass]
    public class CommandLineArguementParserTests
    {
        [TestMethod]
        public void TestParser()
        {
            string[] args = { "chat.txt", "chat.json", "-fu", "user", "-fk", "keyword", "-bw", "word", "-bn", "-o" };

            ConversationExporterConfiguration config = CommandLineArgumentParser.ParseCommandLineArguments(args);

            Assert.AreEqual("chat.txt", config.inputFilePath);
            Assert.AreEqual("chat.json", config.outputFilePath);
            Assert.AreEqual("user", config.userToFilter);
            Assert.AreEqual("keyword", config.keywordToFilter);
            Assert.AreEqual("word", config.wordToBlacklist);
            Assert.IsTrue(config.blacklistNumbers);
            Assert.IsTrue(config.obfuscate);

            string[] args2 = { "chat.txt", "chat.json", "-bw", "word", "-o" };
            config = CommandLineArgumentParser.ParseCommandLineArguments(args2);

            Assert.AreEqual("chat.txt", config.inputFilePath);
            Assert.AreEqual("chat.json", config.outputFilePath);
            Assert.IsNull(config.userToFilter);
            Assert.IsNull(config.keywordToFilter);
            Assert.AreEqual("word", config.wordToBlacklist);
            Assert.IsFalse(config.blacklistNumbers);
            Assert.IsTrue(config.obfuscate);
        }
    }
}
