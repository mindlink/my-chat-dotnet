using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using MyChat;
using Newtonsoft.Json;


namespace MindLink.Recruitment.MyChat.Tests
{

    /// <summary>
    /// Tests for the <see cref="ConversationExporter"/>.
    /// </summary>
    [TestClass]
    public class ConversationExporterTests
    {

        private const string defaultdir = @"C:\Users\standarduser\Source\Repos\my-chat-dotnet\MyChat\";

        /// <summary>
        /// Testing that exporting the conversation gives the proper output.
        /// </summary>
        [TestMethod]
        [TestCategory("1 - Testing that exporting the conversation gives the proper output.")]
        public void ExportingConversationExportsConversation()
        {
            ConversationExporter exporter = new ConversationExporter();

            exporter.ExportConversation(defaultdir+"chat.txt", "chatformat1.json");
            exporter.ExportConversation(defaultdir+"chatvalid2.txt", "chatformat2.json");

            //Verify that both input file formats are supported by checking their corrsponding conversations.
            var serializedConversation = new StreamReader(new FileStream("chatformat1.json", FileMode.Open)).ReadToEnd();
            var serializedConversationformat2 = new StreamReader(new FileStream("chatformat2.json", FileMode.Open)).ReadToEnd();

            Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);
            Conversation savedConversationformat2 = JsonConvert.DeserializeObject<Conversation>(serializedConversationformat2);

            Assert.AreEqual(savedConversation.messages.Count(),savedConversationformat2.messages.Count());
            Assert.AreEqual("My Conversation", savedConversation.name);

            //Validate output of the conversation export here.
            var messages = savedConversation.messages.ToList();
            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470901), messages[0].timestamp);
            Assert.AreEqual("bob", messages[0].senderId);
            Assert.AreEqual("Hello there!", messages[0].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470905), messages[1].timestamp);
            Assert.AreEqual("mike", messages[1].senderId);
            Assert.AreEqual("how are you?", messages[1].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470906), messages[2].timestamp);
            Assert.AreEqual("bob", messages[2].senderId);
            Assert.AreEqual("I'm good thanks, do you like pie?", messages[2].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470910), messages[3].timestamp);
            Assert.AreEqual("mike", messages[3].senderId);
            Assert.AreEqual("no, let me ask Angus...", messages[3].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470912), messages[4].timestamp);
            Assert.AreEqual("angus", messages[4].senderId);
            Assert.AreEqual("Hell yes! Are we buying some pie?", messages[4].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470914), messages[5].timestamp);
            Assert.AreEqual("bob", messages[5].senderId);
            Assert.AreEqual("No, just want to know if there's anybody else in the pie society...", messages[5].content);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1448470915), messages[6].timestamp);
            Assert.AreEqual("angus", messages[6].senderId);
            Assert.AreEqual("YES! I'm the head pie eater there...", messages[6].content);

        }


        /// <summary>
        /// Testing for filtering by conversation sender's name.
        /// </summary>
        [TestMethod]
        [TestCategory("2 - Testing for filtering by conversation sender's name.")]
        public void FilteringConversationBySenderId()
        {
            ConversationExporter exporter = new ConversationExporter();

            //Check for json exports when filtering by senderId.
            exporter.ExportConversation(defaultdir+"chat.txt", "chatfilteredCorrect.json", "bob");
            exporter.ExportConversation(defaultdir+"chat.txt", "chatfilteredWrong.json","wasdy");

            var serializedConversation = new StreamReader(new FileStream("chatfilteredCorrect.json", FileMode.Open)).ReadToEnd();
            var serializConvBlank = new StreamReader(new FileStream("chatfilteredWrong.json", FileMode.Open)).ReadToEnd();

            Conversation filtered1 = JsonConvert.DeserializeObject<Conversation>(serializedConversation);
            Conversation filtered2 = JsonConvert.DeserializeObject<Conversation>(serializConvBlank);

            var filteredConversation = filtered1.messages.ToList();
            var filtered2ndConversation = filtered2.messages.ToList();

            Assert.IsTrue(filtered1.name.Equals(filtered2.name));

            //Assert.AreNotEqual(filtered2ndConversation[0].content,filteredConversation[0].content);
            Assert.IsTrue(filteredConversation.Any() && !filtered2ndConversation.Any());

            //Additional checks on exported content of Json files..
            Assert.AreEqual(3, filteredConversation.Count());
            Assert.AreNotEqual(filteredConversation[1].timestamp, DateTimeOffset.FromUnixTimeSeconds(1448470905));

            foreach (var msg in filteredConversation)
                Assert.AreEqual("bob", msg.senderId);

        }

        /// <summary>
        /// Testing for filtering by specific keyword in conversation messages.
        /// </summary>
        [TestMethod]
        [TestCategory("3 - Testing for filtering by specific keyword in conversation messages.")]
        public void FilteringConversationByKeyword()
        {
            ConversationExporter exp = new ConversationExporter();

            //Check for json exports with the appropriate keyword filtering on message content.
            exp.ExportConversation(defaultdir+"chat.txt", "chatfilteredNoCase.json", "hello");
            exp.ExportConversation(defaultdir+"chat.txt", "chatfilteredCase.json", "PIE");

            //Testing for filtering by content using the specified keywords. Test includes UPpercase and lowercase keywords.
            var filtOutput1 = new StreamReader(new FileStream("chatfilteredNoCase.json", FileMode.Open)).ReadToEnd();
            var filtOutput2 = new StreamReader(new FileStream("chatfilteredCase.json", FileMode.Open)).ReadToEnd();

            Conversation conversatOutput1 = JsonConvert.DeserializeObject<Conversation>(filtOutput1);
            Conversation conversatOutput2 = JsonConvert.DeserializeObject<Conversation>(filtOutput2);

            var filtKeywordsConversation1 = conversatOutput1.messages.ToList();
            var filtKeywordsConversation2 = conversatOutput2.messages.ToList();

            Assert.IsTrue(conversatOutput1.name.Equals(conversatOutput2.name));

            //Testing for proper results on content filtering with caps, lowercase and content type.
            Assert.IsTrue(filtKeywordsConversation1.Any() || filtKeywordsConversation2.Any());

            Assert.IsTrue(filtKeywordsConversation1.Count().Equals(1) && filtKeywordsConversation2.Count().Equals(4));

            Assert.IsTrue(filtKeywordsConversation1[0].content.Contains("Hello") ||
                         !filtKeywordsConversation2[0].content.Contains("Hello") ||
                          filtKeywordsConversation1[0].content.Contains("pie"));

            Assert.IsFalse(filtKeywordsConversation1[0].content.Contains("pie") &&
               filtKeywordsConversation2[0].content.Contains("Hello"));
        }

        /// <summary>
        /// Testing for redacting blacklisted keywords from message content.
        /// </summary>
        [TestMethod]
        [TestCategory("4 - Testing for redacting blacklisted keywords from message content.")]
        public void RedactingConversationContentByBlacklist()
        {
            ConversationExporter exp = new ConversationExporter();

            //Check for json exports with the blacklisted words redacted from message content.
            exp.ExportConversation(defaultdir+"chat.txt", "chatRedacted.json", "hello|pie|the".Split('|'), false);

            var redactedByBlacklist = new StreamReader(new FileStream("chatRedacted.json", FileMode.Open,FileAccess.ReadWrite)).ReadToEnd();
            Conversation conversatOutRed = JsonConvert.DeserializeObject<Conversation>(redactedByBlacklist);

            //Validate properly redacted output per conversation/per message.
            //Find all occurences of blacklisted keywords and verify output of the redacted conversation.
            var allredactions = conversatOutRed.messages.Where(msg => msg.content.Contains("*redacted*")).ToList();
            Assert.AreEqual(5, allredactions.Count());

            foreach (var redact in allredactions)
            {
                var allcontentWords = redact.content.Split(' ');
                Assert.IsTrue(!allcontentWords[0].Equals("") || allcontentWords.Count(cnt => cnt.Contains("*redacted*")) >= 1);
            }
        }

        /// <summary>
        /// Testing for redacting credit cards and phone numbers from message content.
        /// </summary>
        [TestMethod]
        [TestCategory("5 - Testing for redacting credit cards and phone numbers from message content.")]
        public void RedactingConversationContentCreditCards()
        {
            ConversationExporter convExporter = new ConversationExporter();

            //Check for redacted content on the json exported file - Enabling or Disabling Sensitive Information Flag.
            //Using: extended conversation file to include credit card and phone numbers.

            //Filtering only credit card and phone numbers - Include UNUSED words in blacklist and the "-Sensitive" command option.
            convExporter.ExportConversation(defaultdir+"chatextended.txt","chatExtRedacted.json","hello|test".Split('|'), true);

            //Filtering both sensitive info AND blacklisted words - Include USED words and the "-Sensitive" command option.
            convExporter.ExportConversation(defaultdir+"chatextended.txt","chatExtRedacted2.json","pie|Hello".Split('|'),true);


            var redactedBySensitiveInfo = new StreamReader(new FileStream("chatExtRedacted.json", FileMode.Open)).ReadToEnd();
            Conversation convOutRedactSensitive = JsonConvert.DeserializeObject<Conversation>(redactedBySensitiveInfo);

            var redactedBySensitiveInfoBoth = new StreamReader(new FileStream("chatExtRedacted2.json", FileMode.Open)).ReadToEnd();
            Conversation convOutRedactSensitiveBoth = JsonConvert.DeserializeObject<Conversation>(redactedBySensitiveInfoBoth);

            //Validate all redacted output per conversation/per message.
            //Find all occurences of blacklisted keywords and verify output of the redacted conversation.
            var allsensitiveInfo = convOutRedactSensitive.messages.Where(msg => msg.content.Contains("*redacted*")).ToList();
            Assert.AreEqual(4, allsensitiveInfo.Count());

            var allsensitiveInfoBoth = convOutRedactSensitiveBoth.messages.Where(msg => msg.content.Contains("*redacted*")).ToList();
            Assert.AreEqual(10, allsensitiveInfoBoth.Count());

            Assert.IsFalse(allsensitiveInfo.SequenceEqual(allsensitiveInfoBoth));

            foreach (var redact in allsensitiveInfo)
            {
                var allsensWords = redact.content.Split(' ');
                Assert.IsTrue(!allsensWords[0].Equals("") || allsensWords.Count(sentence => sentence.Contains("*redacted*")) >= 1);
            }
        }

        /// <summary>
        /// Testing for obfuscation procedure of sender names in conversation.
        /// </summary>
        [TestMethod]
        [TestCategory("6 - Testing for obfuscation procedure of sender names in conversation.")]
        public void ObfuscationOfSenderIds()
        {
            ConversationExporter exportConv = new ConversationExporter();

            //Check the obfuscation method for different case of names.
            string[] testStrings = {"Bob", "bob", "Sherry", "Terry", "SavvaS", "Anton", "Anders"};

            Assert.AreNotEqual(testStrings[0], exportConv.ObfuscateIds(testStrings[0]));
            Assert.AreNotEqual(testStrings[1], exportConv.ObfuscateIds(testStrings[1]));
            Assert.AreNotEqual(testStrings[2], exportConv.ObfuscateIds(testStrings[2]));
            Assert.AreNotEqual(testStrings[3], exportConv.ObfuscateIds(testStrings[3]));
            Assert.AreNotEqual(testStrings[4], exportConv.ObfuscateIds(testStrings[4]));
            Assert.AreNotEqual(testStrings[5], exportConv.ObfuscateIds(testStrings[5]));
            Assert.AreNotEqual(testStrings[6], exportConv.ObfuscateIds(testStrings[6]));

            //Additional checks for further randomization of namings..
            Assert.AreNotEqual(exportConv.ObfuscateIds(testStrings[0]), exportConv.ObfuscateIds(testStrings[1]));
            Assert.AreNotEqual(exportConv.ObfuscateIds(testStrings[3]), exportConv.ObfuscateIds(testStrings[2]));


            StringBuilder sb = new StringBuilder();
            foreach (char  C in testStrings[4].Reverse())
                sb.Append(C);
            Assert.AreEqual(exportConv.ObfuscateIds(testStrings[4]), exportConv.ObfuscateIds(sb.ToString()));
            sb.Clear();

            foreach (char C in testStrings[4].ToLower().Reverse())
                sb.Append(C);
            Assert.AreNotEqual(exportConv.ObfuscateIds(testStrings[4]), exportConv.ObfuscateIds(sb.ToString()));

            //Validate that all senderIds in exported content is in its proper form (alphanumeric strings).
            exportConv.ExportConversation(defaultdir+"chat.txt", "chatOut.json", true);

            var obfuscatedOutput = new StreamReader(new FileStream("chatOut.json", FileMode.Open)).ReadToEnd();
            Conversation conversatOutOBf = JsonConvert.DeserializeObject<Conversation>(obfuscatedOutput);

            Regex alphaNumReg = new Regex("[a-z0-9]{10}");
            foreach (var msg in conversatOutOBf.messages)
                Assert.IsTrue(alphaNumReg.IsMatch(msg.senderId));
        }

        /// <summary>
        /// Testing for creation of report appended to the exported conversation.
        /// </summary>
        [TestMethod]
        [TestCategory("7 - Testing for creation of report appended to the exported conversation.")]
        public void CreateReportAndAppendToOutput()
        {
            ConversationExporter convExport = new ConversationExporter();

            //Check for the content of the most active users report.
            //Using: extended conversation file for the reporting.
            //NOTE: The report is appended in every exported file by default.
            convExport.ExportConversation(defaultdir+"chatextended.txt", "chatReport.json");

            var standardOutputWithReport = new StreamReader(new FileStream("chatReport.json", FileMode.Open, FileAccess.Read)).ReadToEnd();
            Conversation convReport = JsonConvert.DeserializeObject<Conversation>(standardOutputWithReport);
            
            Assert.IsTrue(convReport.activeusers.Any());

            Assert.AreEqual(8, convReport.messages.Count(user => user.senderId == "bob"));
            Assert.AreNotEqual(6, convReport.messages.Count( user => user.senderId == "mike"));
            Assert.AreEqual(4, convReport.messages.Count(user => user.senderId == "mike"));
            Assert.AreEqual(6, convReport.messages.Count(user => user.senderId == "angus"));

            Assert.AreEqual(convReport.messages.Count(u => u.senderId == "bob").ToString(),
                convReport.activeusers[0].TotalOfMsgs);

            Assert.AreEqual(convReport.messages.Count(u => u.senderId == "mike").ToString(),
                convReport.activeusers[2].TotalOfMsgs);

            Assert.AreEqual(convReport.messages.Count(u => u.senderId == "angus").ToString(),
                convReport.activeusers[1].TotalOfMsgs);
        }


    }
}
