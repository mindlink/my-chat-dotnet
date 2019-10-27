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

            configuration = argumentparser.ParseCommandLineArguments("chat.txt chat.json -u cian -k hello".Split(' '));
            Assert.AreEqual("hello", configuration.keywordToFilter);
            Assert.AreEqual("cian", configuration.userToFilter);
            Assert.IsFalse(configuration.hideNumbers);
            Assert.IsFalse(configuration.obfuscateUserID);

            configuration = argumentparser.ParseCommandLineArguments("chat.txt chat.json -k goodbye -hn -ouid".Split(' '));
            Assert.AreEqual("goodbye", configuration.keywordToFilter);
            Assert.AreEqual(null, configuration.userToFilter);
            Assert.IsTrue(configuration.hideNumbers);
            Assert.IsTrue(configuration.obfuscateUserID);
        }

        [TestMethod]
        [ExpectedException (typeof(ArgumentException), "The program did not throw an ArgumentException when no input or output file is specified.")]
        public void ArgumentExceptionWhenNoInputOrOutputFilesSpecified ()
        {
            CommandLineArgumentParser argumentParser = new CommandLineArgumentParser();
            ConversationExporterConfiguration configuration = argumentParser.ParseCommandLineArguments(new string[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "The program did not throw an ArgumentException when an input file but no output file is specified.")]
        public void ArgumentExceptionWhenOnlyInputFilesSpecified()
        {
            CommandLineArgumentParser argumentParser = new CommandLineArgumentParser();
            ConversationExporterConfiguration configuration = argumentParser.ParseCommandLineArguments(new string[1] { "input.txt" });
        }
    }
}
