using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindLink.Recruitment.MyChat.Actions;
using MindLink.Recruitment.MyChat.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat.Tests
{
    /// <summary>
    /// Tests for the <see cref="ConversationExporter"/>.
    /// </summary>
    [TestClass]
    public class ActionsTest
    {

     #region FilterUser Test 
        /// <summary>
        /// Tests that FilterUser trow exception when userid is empty
        /// </summary>
        [TestMethod]
        public void FilterUserUserIdNull()
        {
            try
            {
                ConversationAction action = new FilterUser(" ");
            }
            catch (ArgumentNullException e)
            {
                // assert  
                StringAssert.Contains(e.Message, "User id can not be empty when filering by user");
                return;
            }
            Assert.Fail("No exception was thrown.");


        }

        /// <summary>
        /// Tests that FilterUser PerformOn function throw exception when Conversation is null
        /// </summary>
        [TestMethod]
        public void FilterUserConversationNull()
        {
            try
            {
                ConversationAction action = new FilterUser("bob");
                action.PerformOn(null);
            }
            catch (ArgumentNullException e)
            {
                // assert  
                StringAssert.Contains(e.Message, "PerfromOn received a null argument!");
                return;
            }
            Assert.Fail("No exception was thrown.");


        }

        /// <summary>
        /// Tests that FilterUser filter correctly, with a user that is define by the input
        /// </summary>
        [TestMethod]
        public void FilterUserUserWeArefiltering()
        {

            ConversationAction action = new FilterUser("bob");

            Conversation conversation = new Conversation();

            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470901")), "bob", "Hello there!");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470905")), "mike", "how are you?");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470906")), "bob", "I'm good thanks, do you 14 like pie?");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470910")), "mike", "no, let me ask Angus...");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470912")), "angus", "Hell yes! Are we buying some pie?");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470914")), "bob", "No, just want to know if there's anybody else in the pie society...");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470915")), "angus", "YES! I'm the head pie eater there...");
            action.PerformOn(conversation);


            if (conversation.Messages.Count(x => x.msgSender.senderID == "bob") <= 0)
            {
                Assert.Fail("Messages of the user we were filtering were not found");
            }

           

        }

        // <summary>
        /// Tests that FilterUser filter correctly, with a user that is not the user we are filtering
        /// </summary>
        [TestMethod]
        public void FilterUserUserWeAreNotFiltering()
        {

            ConversationAction action = new FilterUser("bob");

            Conversation conversation = new Conversation();

            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470901")), "bob", "Hello there!");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470905")), "mike", "how are you?");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470906")), "bob", "I'm good thanks, do you 14 like pie?");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470910")), "mike", "no, let me ask Angus...");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470912")), "angus", "Hell yes! Are we buying some pie?");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470914")), "bob", "No, just want to know if there's anybody else in the pie society...");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470915")), "angus", "YES! I'm the head pie eater there...");
            action.PerformOn(conversation);

            Assert.AreEqual(conversation.Messages.Count(x => x.msgSender.senderID == "mike"), 0, "Messages were found of a user that was not the user we were filtering");

        }
        #endregion

     #region FilterWord Test 
        /// <summary>
        /// Tests that FilterWord trow exception when word is empty
        /// </summary>
        [TestMethod]
        public void FilterWordWordNull()
        {
            try
            {
                ConversationAction action = new FilterWord(" ");
            }
            catch (ArgumentNullException e)
            {
                // assert  
                StringAssert.Contains(e.Message, "Word can not be empty when filering using a word");
                return;
            }
            Assert.Fail("No exception was thrown.");


        }

        /// <summary>
        /// Tests that FilterWord PerformOn function throw exception when Conversation is null
        /// </summary>
        [TestMethod]
        public void FilterWordConversationNull()
        {
            try
            {
                ConversationAction action = new FilterWord("pie");
                action.PerformOn(null);
            }
            catch (ArgumentNullException e)
            {
                // assert  
                StringAssert.Contains(e.Message, "PerfromOn received a null argument!");
                return;
            }
            Assert.Fail("No exception was thrown.");


        }

        /// <summary>
        /// Tests that FilterWord filter correctly, with a word that is define by the input
        /// </summary>
        [TestMethod]
        public void FilterWordWordWeArefiltering()
        {

            ConversationAction action = new FilterWord("pie");

            Conversation conversation = new Conversation();

            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470901")), "bob", "Hello there!");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470905")), "mike", "how are you?");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470906")), "bob", "I'm good thanks, do you 14 like pie?");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470910")), "mike", "no, let me ask Angus...");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470912")), "angus", "Hell yes! Are we buying some pie?");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470914")), "bob", "No, just want to know if there's anybody else in the pie society...");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470915")), "angus", "YES! I'm the head pie eater there...");
            action.PerformOn(conversation);
           

            if (conversation.Messages.Count(x => x.Content.Contains("pie")) <= 0)
            {
                Assert.Fail("Messages conatainig the word we were filtering were not found");
            }



        }

        // <summary>
        /// Tests that FilterWord filter correctly, with a word that is not cotained in the filered result but is contained on the conversation
        /// </summary>
        [TestMethod]
        public void FilterWordWordWeAreNotFiltering()
        {

            ConversationAction action = new FilterWord("pie");

            Conversation conversation = new Conversation();

            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470901")), "bob", "Hello there!");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470905")), "mike", "how are you?");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470906")), "bob", "I'm good thanks, do you 14 like pie?");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470910")), "mike", "no, let me ask Angus...");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470912")), "angus", "Hell yes! Are we buying some pie?");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470914")), "bob", "No, just want to know if there's anybody else in the pie society...");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470915")), "angus", "YES! I'm the head pie eater there...");
            action.PerformOn(conversation);

            Assert.AreEqual(conversation.Messages.Count(x => x.Content.Contains("ask")), 0, "Messages were found of a user that was not the user we were filtering");

        }
        #endregion

     #region HideBlackList Test 

        /// <summary>
        /// Tests that HideBlackList trow exception when word is empty
        /// </summary>
        [TestMethod]
        public void HideBlackListAddNullWord()
        {
            HideBlackList action = new HideBlackList();
            try
            {
                action.addBlackListWord(null);
            }
            catch (ArgumentNullException e)
            {
                // assert  
                StringAssert.Contains(e.Message, "addBlackListWord received a null argument!");
                return;
            }
            Assert.Fail("No exception was thrown.");


        }

        /// <summary>
        /// Tests that HideBlackList trow exception when word is empty string
        /// </summary>
        [TestMethod]
        public void HideBlackListAddEmptyStringWord()
        {
            HideBlackList action = new HideBlackList();
            try
            {
                action.addBlackListWord("");
            }
            catch (ArgumentNullException e)
            {
                // assert  
                StringAssert.Contains(e.Message, "addBlackListWord received a null argument!");
                return;
            }
            Assert.Fail("No exception was thrown.");


        }

        /// <summary>
        /// Tests that HideBlackList PerformOn function throw exception when Conversation is null
        /// </summary>
        [TestMethod]
        public void HideBlackListConversationNull()
        {
            try
            {
                ConversationAction action = new HideBlackList(); 
                action.PerformOn(null);
            }
            catch (ArgumentNullException e)
            {
                // assert  
                StringAssert.Contains(e.Message, "PerfromOn received a null argument!");
                return;
            }
            Assert.Fail("No exception was thrown.");


        }

        /// <summary>
        /// Tests that HideBlackList removes the blacklisted words
        /// </summary>
        [TestMethod]
        public void HideBlackListBlacklistedWordDontExist()
        {

            HideBlackList action = new HideBlackList();
            action.addBlackListWord("buying");
            action.addBlackListWord("hello");

            Conversation conversation = new Conversation();

            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470901")), "bob", "Hello there!");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470905")), "mike", "how are you?");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470906")), "bob", "I'm good thanks, do you 14 like pie?");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470910")), "mike", "no, let me ask Angus...");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470912")), "angus", "Hell yes! Are we buying some pie?");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470914")), "bob", "No, just want to know if there's anybody else in the pie society...");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470915")), "angus", "YES! I'm the head pie eater there...");
            action.PerformOn(conversation);


            if (conversation.Messages.Count(x => x.Content.Contains("Hello")) > 0)
            {
                Assert.Fail("Messages conatainig the word we were blacklisting were found");
            }

            if (conversation.Messages.Count(x => x.Content.Contains("buying")) > 0)
            {
                Assert.Fail("Messages conatainig the word we were blacklisting were found");
            }

        }

        /// <summary>
        /// Tests that HideBlackList replace the blacklisted words with the word *redacted*
        /// </summary>
        [TestMethod]
        public void HideBlackListReplaceBlacklisedWord()
        {

            HideBlackList action = new HideBlackList();
            action.addBlackListWord("buying");
            action.addBlackListWord("hello");

            Conversation conversation = new Conversation();

            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470901")), "bob", "Hello there!");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470905")), "mike", "how are you?");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470906")), "bob", "I'm good thanks, do you 14 like pie?");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470910")), "mike", "no, let me ask Angus...");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470912")), "angus", "Hell yes! Are we buying some pie?");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470914")), "bob", "No, just want to know if there's anybody else in the pie society...");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470915")), "angus", "YES! I'm the head pie eater there...");
            action.PerformOn(conversation);


            if (conversation.Messages.Count(x => x.Content.Contains("*redacted*")) <= 0)
            {
                Assert.Fail("Black listed words were not replced by *redacted* word");
            }

        }
        #endregion

     #region HideCreditcartPhone Test 

        /// <summary>
        /// Tests that HideCreditcartPhone PerformOn function throw exception when Conversation is null
        /// </summary>
        [TestMethod]
        public void HideCreditcartPhoneConversationNull()
        {
            try
            {
                ConversationAction action = new HideCreditcartPhone();
                action.PerformOn(null);
            }
            catch (ArgumentNullException e)
            {
                // assert  
                StringAssert.Contains(e.Message, "PerfromOn received a null argument!");
                return;
            }
            Assert.Fail("No exception was thrown.");


        }

        /// <summary>
        /// Tests that HideCreditcartPhone removes the credicart numbers
        /// </summary>
        [TestMethod]
        public void HideCreditcartPhoneCreditcartNumberDontExist()
        {

            ConversationAction action = new HideCreditcartPhone();


            Conversation conversation = new Conversation();

            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470901")), "bob", "Hello there my land line 00357 22389618!");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470905")), "mike", "how are you?");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470906")), "bob", "I'm good thanks, do you like 25897 pie?");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470910")), "mike", "no, let me ask Angus... my mobile phone number +357 99355993");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470912")), "angus", "Hell yes! Are we buying some pie? my credit cart is 4012-8888-8888-1881 hehe");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470914")), "bob", "No, just want to know my phone 99355993 if there's anybody else in the pie society... my credit cart is 4111111111111111");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470915")), "angus", "YES! my mobile phone +35799355993 I'm the head pie eater there...");
            action.PerformOn(conversation);


            if (conversation.Messages.Count(x => x.Content.Contains("4111111111111111")) > 0)
            {
                Assert.Fail("Messages conatains a credit cart number that shoould have been removed");
            }

            if (conversation.Messages.Count(x => x.Content.Contains("4012-8888-8888-1881")) > 0)
            {
                Assert.Fail("Messages conatains a credit cart number that shoould have been removed");
            }

        }

        /// <summary>
        /// Tests that HideCreditcartPhone removes the phone numbers
        /// </summary>
        [TestMethod]
        public void HideCreditcartPhonePhoneNumberDontExist()
        {

            ConversationAction action = new HideCreditcartPhone();


            Conversation conversation = new Conversation();

            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470901")), "bob", "Hello there my land line 00357 22389618");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470905")), "mike", "how are you?");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470906")), "bob", "I'm good thanks, do you like 25897 pie?");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470910")), "mike", "no, let me ask Angus... my mobile phone number +357 99355993");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470912")), "angus", "Hell yes! Are we buying some pie? my credit cart is 4012-8888-8888-1881 hehe");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470914")), "bob", "No, just want to know my phone 99355993 if there's anybody else in the pie society... my credit cart is 4111111111111111");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470915")), "angus", "YES! my mobile phone +35799355993 I'm the head pie eater there...");
            action.PerformOn(conversation);


            if (conversation.Messages.Count(x => x.Content.Contains("+35799355993")) > 0)
            {
                Assert.Fail("Messages conatains a phone number that shoould have been removed");
            }

            if (conversation.Messages.Count(x => x.Content.Contains("+357 99355993")) > 0)
            {
                Assert.Fail("Messages conatains a phone number that shoould have been removed");
            }

            if (conversation.Messages.Count(x => x.Content.Contains("00357 22389618")) > 0)
            {
                Assert.Fail("Messages conatains a phone number that shoould have been removed");
            }

            if (conversation.Messages.Count(x => x.Content.Contains("99355993")) > 0)
            {
                Assert.Fail("Messages conatains a phone number that shoould have been removed");
            }

        }

        /// <summary>
        /// Tests that HideCreditcartPhone dont removes non phone and creditcart like numbers
        /// </summary>
        [TestMethod]
        public void HideCreditcartPhoneDontRemovesNumbers()
        {

            ConversationAction action = new HideCreditcartPhone();


            Conversation conversation = new Conversation();

            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470901")), "bob", "Hello there my land line 00357 22389618");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470905")), "mike", "how are you?");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470906")), "bob", "I'm good thanks, do you like 25897 pie?");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470910")), "mike", "no, let me ask Angus... my mobile phone number +357 99355993");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470912")), "angus", "Hell yes! Are we buying some pie? my credit cart is 4012-8888-8888-1881 hehe");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470914")), "bob", "No, just want to know my phone 99355993 if there's anybody 23-2-1928 else in the pie society... my credit cart is 4111111111111111");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470915")), "angus", "YES! my mobile phone +35799355993 I'm the head pie eater there...");
            action.PerformOn(conversation);


            if (conversation.Messages.Count(x => x.Content.Contains("25897")) <= 0)
            {
                Assert.Fail("A not phone or credit card like number was removed.The number 25897 should not be removed");
            }

         if (conversation.Messages.Count(x => x.Content.Contains("23-2-1928 ")) <= 0)
            {
                Assert.Fail("A not phone or credit card like number was removed.The date 23-2-1928 should not be removed");
            }

        }

        /// <summary>
        /// Tests that HideCreditcartPhone replace the phone and creditcart like numbers with the word *redacted*
        /// </summary>
        [TestMethod]
        public void HideCreditcartPhoneRplaceCreditcartPhoneNumbers()
        {

            ConversationAction action = new HideCreditcartPhone();


            Conversation conversation = new Conversation();

            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470901")), "bob", "Hello there my land line 00357 22389618");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470905")), "mike", "how are you?");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470906")), "bob", "I'm good thanks, do you like 25897 pie?");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470910")), "mike", "no, let me ask Angus... my mobile phone number +357 99355993");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470912")), "angus", "Hell yes! Are we buying some pie? my credit cart is 4012-8888-8888-1881 hehe");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470914")), "bob", "No, just want to know my phone 99355993 if there's anybody 23-2-1928 else in the pie society... my credit cart is 4111111111111111");
            conversation.addMessage(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470915")), "angus", "YES! my mobile phone +35799355993 I'm the head pie eater there...");
            action.PerformOn(conversation);


            if (conversation.Messages.Count(x => x.Content.Contains("*redacted*")) <= 0)
            {
                Assert.Fail("Phone and creditcart like numbers were not replced by *redacted* word");
            }

        }

        #endregion

    }
}
