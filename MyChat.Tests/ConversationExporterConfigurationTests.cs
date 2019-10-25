using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindLink.Recruitment.MyChat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat.Tests
{
    [TestClass()]
    public class ConversationExporterConfigurationTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Allowed Invalid File Path")]
        public void DoesntAllowInvalidFilePathsInputInvalidTest()
        {
            ConversationExporterConfiguration test_config_invalid_input_path = new ConversationExporterConfiguration("£#|||';;!$%.txt", "chat.json");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Allowd Invalid File Path")]
        public void DoesntAllowInvalidFilePathOutputInvalidTest()
        {
            ConversationExporterConfiguration test_config_invalid_output_path = new ConversationExporterConfiguration("chat.txt", "£$%^>&*.json");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Allowd Invalid File Path")]
        public void DoesntAllowInvalidFilePathBothInvalidTest()
        {
            ConversationExporterConfiguration test_config_both_invalid = new ConversationExporterConfiguration("£$%^&<*&^%$.txt", "^%$£$%^&*.json");
        }

        [TestMethod] 
        public void AllowsValidFilePaths ()
        {
            // Test will fail if exception thrown.
            ConversationExporterConfiguration test = new ConversationExporterConfiguration("folder/input.txt", "output.txt");
        }
    }
}