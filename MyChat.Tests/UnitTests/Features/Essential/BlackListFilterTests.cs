namespace MindLink.Recruitment.MyChat.Tests.UnitTests.Features.Essential
{
    using MindLink.Recruitment.MyChat.Features.Essential;
    using MindLink.Recruitment.MyChat.Interfaces.FeatureInterfaces;
    using MyChatModel.ModelData;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Test class to test the <see cref="FilterByBlacklist"> class
    /// </summary>
    [TestFixture]
    public class BlackListFilterTests
    {
        // IStrategyFilter to be tested
        private IStrategyFilter blacklistFilter;
        // Conversation to test the filter with
        private Conversation conversation;

        /// <summary>
        /// Test to see if the Blacklist functions as expected 
        /// </summary>
        [Test]
        public void BlacklistFiltersFiltersBlacklist() 
        {
            // Filter to be tested 
            blacklistFilter = new FilterByBlacklist("pie");
            // GENERATE conversation to be filtered
            MakeConverSation();
            // INSTANTIATE new conversation called filteredConversation,
            // SET it to the returned conversation from the filter
            Conversation filteredConversation = blacklistFilter.ApplyFilter(conversation);
            // INSTANTIATE a list as the conversation msgs IEnumerable converted to list
            IList<Message> msgs = filteredConversation.Messages.ToList<Message>();

            Assert.That(filteredConversation.Name, Is.EqualTo("Test conversation"));

            Assert.That(filteredConversation.FilterMessage[0], Is.EqualTo("No filter errors"));

            Assert.That(msgs[0].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470901)));
            Assert.That(msgs[0].SenderId, Is.EqualTo("bob"));
            Assert.That(msgs[0].Content, Is.EqualTo("Hello there!"));

            Assert.That(msgs[1].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470905)));
            Assert.That(msgs[1].SenderId, Is.EqualTo("mike"));
            Assert.That(msgs[1].Content, Is.EqualTo("how are you?"));

            Assert.That(msgs[2].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470906)));
            Assert.That(msgs[2].SenderId, Is.EqualTo("bob"));
            Assert.That(msgs[2].Content, Is.EqualTo("I'm good thanks, do you like \\*redacted\\*?"));

            Assert.That(msgs[3].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470910)));
            Assert.That(msgs[3].SenderId, Is.EqualTo("mike"));
            Assert.That(msgs[3].Content, Is.EqualTo("no, let me ask Angus..."));

            Assert.That(msgs[4].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470912)));
            Assert.That(msgs[4].SenderId, Is.EqualTo("angus"));
            Assert.That(msgs[4].Content, Is.EqualTo("Hell yes! Are we buying some \\*redacted\\*?"));

            Assert.That(msgs[5].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470914)));
            Assert.That(msgs[5].SenderId, Is.EqualTo("bob"));
            Assert.That(msgs[5].Content, Is.EqualTo("No, just want to know if there's anybody else in the \\*redacted\\* society..."));

            Assert.That(msgs[6].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470915)));
            Assert.That(msgs[6].SenderId, Is.EqualTo("angus"));
            Assert.That(msgs[6].Content, Is.EqualTo("YES! I'm the head \\*redacted\\* eater there..."));
        }

        /// <summary>
        /// Test to see if the Blacklist functions as expected 
        /// </summary>
        [Test]
        public void BlacklistFiltersFiltersBlacklistMultipleWords()
        {
            string[] words = new string[] { "pie", "society" };

            // Filter to be tested 
            blacklistFilter = new FilterByBlacklist(words);
            // GENERATE conversation to be filtered
            MakeConverSation();
            // INSTANTIATE new conversation called filteredConversation,
            // SET it to the returned conversation from the filter
            Conversation filteredConversation = blacklistFilter.ApplyFilter(conversation);
            // INSTANTIATE a list as the conversation msgs IEnumerable converted to list
            IList<Message> msgs = filteredConversation.Messages.ToList<Message>();

            Assert.That(filteredConversation.Name, Is.EqualTo("Test conversation"));

            Assert.That(filteredConversation.FilterMessage[0], Is.EqualTo("No filter errors"));

            Assert.That(msgs[0].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470901)));
            Assert.That(msgs[0].SenderId, Is.EqualTo("bob"));
            Assert.That(msgs[0].Content, Is.EqualTo("Hello there!"));

            Assert.That(msgs[1].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470905)));
            Assert.That(msgs[1].SenderId, Is.EqualTo("mike"));
            Assert.That(msgs[1].Content, Is.EqualTo("how are you?"));

            Assert.That(msgs[2].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470906)));
            Assert.That(msgs[2].SenderId, Is.EqualTo("bob"));
            Assert.That(msgs[2].Content, Is.EqualTo("I'm good thanks, do you like \\*redacted\\*?"));

            Assert.That(msgs[3].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470910)));
            Assert.That(msgs[3].SenderId, Is.EqualTo("mike"));
            Assert.That(msgs[3].Content, Is.EqualTo("no, let me ask Angus..."));

            Assert.That(msgs[4].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470912)));
            Assert.That(msgs[4].SenderId, Is.EqualTo("angus"));
            Assert.That(msgs[4].Content, Is.EqualTo("Hell yes! Are we buying some \\*redacted\\*?"));

            Assert.That(msgs[5].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470914)));
            Assert.That(msgs[5].SenderId, Is.EqualTo("bob"));
            Assert.That(msgs[5].Content, Is.EqualTo("No, just want to know if there's anybody else in the \\*redacted\\* \\*redacted\\*..."));

            Assert.That(msgs[6].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470915)));
            Assert.That(msgs[6].SenderId, Is.EqualTo("angus"));
            Assert.That(msgs[6].Content, Is.EqualTo("YES! I'm the head \\*redacted\\* eater there..."));
        }

        /// <summary>
        /// Test whether the word filter works as intended and when the word is not found in the conversation
        /// it prints a message into the conversation to say as much. The list of messages should not throw any exceptions
        /// unlike the previous tests, as messages are to be kept intact except for a single word
        /// being replaced
        /// </summary>
        [Test]
        public void BlacklistFilterWordNotFound()
        {
            // Filter to be tested 
            blacklistFilter = new FilterByBlacklist("ball");
            // GENERATE conversation to be filtered
            MakeConverSation();
            // INSTANTIATE new conversation called filteredConversation,
            // SET it to the returned conversation from the filter
            Conversation filteredConversation = blacklistFilter.ApplyFilter(conversation);
            // INSTANTIATE a list as the conversation messages IEnumerable converted to list
            IList<Message> msgs = filteredConversation.Messages.ToList<Message>();

            Assert.That(filteredConversation.Name, Is.EqualTo("Test conversation"));

            Assert.That(filteredConversation.FilterMessage[0], Is.EqualTo("The word ball to blacklist was not found in the conversation"));

            Assert.That(msgs[0].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470901)));
            Assert.That(msgs[0].SenderId, Is.EqualTo("bob"));
            Assert.That(msgs[0].Content, Is.EqualTo("Hello there!"));

            Assert.That(msgs[1].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470905)));
            Assert.That(msgs[1].SenderId, Is.EqualTo("mike"));
            Assert.That(msgs[1].Content, Is.EqualTo("how are you?"));

            Assert.That(msgs[2].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470906)));
            Assert.That(msgs[2].SenderId, Is.EqualTo("bob"));
            Assert.That(msgs[2].Content, Is.EqualTo("I'm good thanks, do you like pie?"));

            Assert.That(msgs[3].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470910)));
            Assert.That(msgs[3].SenderId, Is.EqualTo("mike"));
            Assert.That(msgs[3].Content, Is.EqualTo("no, let me ask Angus..."));

            Assert.That(msgs[4].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470912)));
            Assert.That(msgs[4].SenderId, Is.EqualTo("angus"));
            Assert.That(msgs[4].Content, Is.EqualTo("Hell yes! Are we buying some pie?"));

            Assert.That(msgs[5].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470914)));
            Assert.That(msgs[5].SenderId, Is.EqualTo("bob"));
            Assert.That(msgs[5].Content, Is.EqualTo("No, just want to know if there's anybody else in the pie society..."));

            Assert.That(msgs[6].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470915)));
            Assert.That(msgs[6].SenderId, Is.EqualTo("angus"));
            Assert.That(msgs[6].Content, Is.EqualTo("YES! I'm the head pie eater there..."));
        }

        /// <summary>
        /// Test whether the blaclist filter works as intended and throws an exception when the user has
        /// not supplied an argument for the filter to filter with
        /// </summary>
        [Test]
        public void BlacklistFilterThrowsArgumentNullException()
        {

            Assert.That(() => new FilterByBlacklist(""), Throws.Exception.
                TypeOf<ArgumentNullException>());


            // SET the value of this variable to the return value of Asser throws, when supplied with the 
            // a delegate in the form of calling the filters ApplyFilter method
            var exception = Assert.Throws<ArgumentNullException>(() => new FilterByBlacklist(""));

            Assert.That(exception.ParamName, Is.EqualTo("No word was supplied for the blacklist filter to use"));

            Assert.That(() => new FilterByBlacklist(" "), Throws.Exception.
                TypeOf<ArgumentNullException>());


            // SET the value of this variable to the return value of Asser throws, when supplied with the 
            // a delegate in the form of calling the filters ApplyFilter method
            var exception2 = Assert.Throws<ArgumentNullException>(() => new FilterByBlacklist(" "));

            Assert.That(exception2.ParamName, Is.EqualTo("No word was supplied for the blacklist filter to use"));
        }

        private void MakeConverSation()
        {
            // IList of type Message called msgs
            IList<Message> messages = new List<Message>();

            messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(1448470901)), "bob", "Hello there!"));
            messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(1448470905)), "mike", "how are you?"));
            messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(1448470906)), "bob", "I'm good thanks, do you like pie?"));
            messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(1448470910)), "mike", "no, let me ask Angus..."));
            messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(1448470912)), "angus", "Hell yes! Are we buying some pie?"));
            messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(1448470914)), "bob", "No, just want to know if there's anybody else in the pie society..."));
            messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(1448470915)), "angus", "YES! I'm the head pie eater there..."));

            conversation = new Conversation("Test conversation", messages);

        }
    }
}
