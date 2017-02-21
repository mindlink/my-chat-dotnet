using System.IO;
using System.Linq;
using MyChat;
using Newtonsoft.Json;

namespace MindLink.Recruitment.MyChat.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for the <see cref="CommandLineArgumentParser"/> and its implemented methods of <see cref="ConversationExporterConfiguration"/>.
    /// </summary>
    [TestClass]
    public class CommandLineArgumentParserTests
    {
        
        /// <summary>
        /// Takes currents users Dsktop folder path.
        /// </summary>
        private string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\";

        /// <summary>
        /// Tests that the command line parser throws an exception when the arguments array is null with the specified message.
        /// </summary>
        [TestMethod]
        public void ParseCommandLineArgumentsExceptionArgumentArrayNull()
        {
            try
            {
                CommandLineArgumentParser parser = new CommandLineArgumentParser();
                parser.ParseCommandLineArguments(null);
            }
            catch (NullReferenceException e)
            {
                Assert.AreEqual("The Input and Output file arguments were not specified.", e.Message);
            }
        }

        /// <summary>
        /// Tests that the command line parser throws an exception when user filter argument is null with the specified message.
        /// </summary>
        [TestMethod]
        public void ParseCommandLineArgumentsExceptionUserFilterNull()
        {
            try
            {
                CommandLineArgumentParser parser = new CommandLineArgumentParser();
                parser.ParseCommandLineArguments(new string[] { path + "chat.txt", path + "chat_output.json" });
                parser.ParseCommandLineUserFilter(null);
            }
            catch (NullReferenceException e)
            {
                Assert.AreEqual("The user filter argument was not specified.", e.Message);
            }
        }

        /// <summary>
        /// Tests that the command line parser throws an exception when keyword filter argument is null with the specified message.
        /// </summary>
        [TestMethod]
        public void ParseCommandLineArgumentsExceptionKeywordFilterNull()
        {
            try
            {
                CommandLineArgumentParser parser = new CommandLineArgumentParser();
                parser.ParseCommandLineArguments(new string[] { path + "chat.txt", path + "chat_output.json" });
                parser.ParseCommandLineKeywordFilter(null);
            }
            catch (NullReferenceException e)
            {
                Assert.AreEqual("The keyword filter argument was not specified.", e.Message);
            }

        }
        
    }
}


