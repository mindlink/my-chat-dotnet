using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindLink.Recruitment.MyChat.Elements;
using MindLink.Recruitment.MyChat.FilesProcessing;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat.Tests
{
    /// <summary>
    /// Tests for the Elements.
    /// </summary>
    [TestClass]
    public class FilesProcessingTest
    {

        #region FileExporter Test 

        /// <summary>
        /// Tests that WriteConversationToJson function trow exception when outputJsonObject parameter is empty
        /// </summary>
        [TestMethod]
        public void FileExporterOutputJsonObjectNull()
        {
            try
            {
                FileExporter exporter = new FileExporter();
                exporter.WriteConversationToJson(null, "test.json");
            }
            catch (ArgumentNullException e)
            {
                // assert  
                StringAssert.Contains(e.Message, "outputJsonObject can not be empty when exporting a conversation to a file");
                return;
            }
            Assert.Fail("No exception was thrown.");
        }

        /// <summary>
        /// Tests that WriteConversationToJson function trow exception when outputFilePath parameter is null
        /// </summary>
        [TestMethod]
        public void FileExporterOutputFilePathNull()
        {
            try
            {
                JObject conv = new JObject();
                FileExporter exporter = new FileExporter();
                exporter.WriteConversationToJson(conv,null);
            }
            catch (ArgumentNullException e)
            {
                // assert  
                StringAssert.Contains(e.Message, "outputFilePath can not be empty when exporting a conversation to a file");
                return;
            }
            Assert.Fail("No exception was thrown.");
        }

        /// <summary>
        /// Tests that WriteConversationToJson function trow exception when outputFilePath parameter is empty
        /// </summary>
        [TestMethod]
        public void FileExporterOutputFilePathEmpty()
        {
            try
            {
                JObject conv = new JObject();
                FileExporter exporter = new FileExporter();
                exporter.WriteConversationToJson(conv, " ");
            }
            catch (ArgumentNullException e)
            {
                // assert  
                StringAssert.Contains(e.Message, "outputFilePath can not be empty when exporting a conversation to a file");
                return;
            }
            Assert.Fail("No exception was thrown.");
        }

        #endregion

        #region FileImporter Test 

        /// <summary>
        /// Tests that ReadConversationFromTextFile function trow exception when inputFilePath parameter is null
        /// </summary>
        [TestMethod]
        public void ReadConversationFromTextInputFilePathFileNull()
        {
            try
            {
                FileImporter importer = new FileImporter();
                Conversation conv = importer.ReadConversationFromTextFile(null);
            }
            catch (ArgumentNullException e)
            {
                // assert  
                StringAssert.Contains(e.Message, "inputFilePath can not be empty when inporting a conversation from a file");
                return;
            }
            Assert.Fail("No exception was thrown.");
        }

        /// <summary>
        /// Tests that ReadConversationFromTextFile function trow exception when inputFilePath parameter is empty
        /// </summary>
        [TestMethod]
        public void ReadConversationFromTextInputFilePathFileEmpty()
        {
            try
            {
                FileImporter importer = new FileImporter();
                Conversation conv = importer.ReadConversationFromTextFile(" ");
            }
            catch (ArgumentNullException e)
            {
                // assert  
                StringAssert.Contains(e.Message, "inputFilePath can not be empty when inporting a conversation from a file");
                return;
            }
            Assert.Fail("No exception was thrown.");
        }

        /// <summary>
        /// Tests that ReadConversationFromTextFile function trow exception when the conversation name is empty in the chat file
        /// </summary>
        [TestMethod]
        public void ReadConversationFromTextConversationNameEmpty()
        {
            try
            {
                FileImporter importer = new FileImporter();
                Conversation conv = importer.ReadConversationFromTextFile("NoConversationName.txt");
            }
            catch (ArgumentException e)
            {
                // assert  
                StringAssert.Contains(e.Message, "is not in the corect format");
                return;
            }
            Assert.Fail("No exception was thrown.");
        }

        /// <summary>
        /// Tests that ReadConversationFromTextFile function trow exception when the timestamp is in wrong format in the chat file
        /// </summary>
        [TestMethod]
        public void ReadConversationFromTextConversationWrongTimestamp()
        {
            try
            {
                FileImporter importer = new FileImporter();
                Conversation conv = importer.ReadConversationFromTextFile("WrongTimestamp.txt");
            }
            catch (ArgumentException e)
            {
                // assert  
                StringAssert.Contains(e.Message, "is not in the corect format");
                return;
            }
            Assert.Fail("No exception was thrown.");
        }

        /// <summary>
        /// Tests that ReadConversationFromTextFile function trow exception when the sender id is empty in the chat file
        /// </summary>
        [TestMethod]
        public void ReadConversationFromTextConversationSendeIdEmpty()
        {
            try
            {
                FileImporter importer = new FileImporter();
                Conversation conv = importer.ReadConversationFromTextFile("EmptySendeId.txt");
            }
            catch (ArgumentException e)
            {
                // assert  
                StringAssert.Contains(e.Message, "is not in the corect format");
                return;
            }
            Assert.Fail("No exception was thrown.");
        }


        #endregion

    }
}
