namespace MindLink.Recruitment.MyChat.Tests.UnitTests.Features.Additional
{
    using MindLink.Recruitment.MyChat.Features.Additional;
    using MindLink.Recruitment.MyChat.Interfaces.FeatureInterfaces;
    using MyChatModel.ModelData;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Class to test the FilterCardDetails filter
    /// </summary>
    [TestFixture]
    public class FilterCardDetailsTests
    {
        // IStrategyFilter to be tested
        private IStrategyFilter cardDetailsFilter;
        // Conversation to test the filter with
        private Conversation conversation;

        /// <summary>
        /// Tests whether the FilterCardDetails filter filters the card details out of conversation
        /// and replaces with \*redacted\*
        /// </summary>
        [Test]
        public void FilterCardDetailsFiltersCardDetails() 
        {
            // Filter to be tested 
            cardDetailsFilter = new FilterCardDetails();
            // GENERATE conversation to be filtered
            MakeConverSation();
            // INSTANTIATE new conversation called filteredConversation,
            // SET it to the returned conversation from the filter
            Conversation filteredConversation = cardDetailsFilter.ApplyFilter(conversation);
            // INSTANTIATE a list as the conversation msgs IEnumerable converted to list
            IList<Message> msgs = filteredConversation.Messages.ToList<Message>();

            Assert.That(filteredConversation.Name, Is.EqualTo("Test conversation"));

            Assert.That(filteredConversation.FilterMessage[0], Is.EqualTo("No filter errors"));

            Assert.That(msgs[7].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470916)));
            Assert.That(msgs[7].SenderId, Is.EqualTo("angus"));
            Assert.That(msgs[7].Content, Is.EqualTo("My visa card is \\*redacted\\*"));

            Assert.That(msgs[8].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470916)));
            Assert.That(msgs[8].SenderId, Is.EqualTo("angus"));
            Assert.That(msgs[8].Content, Is.EqualTo("My JCB card is \\*redacted\\*"));

            Assert.That(msgs[9].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470916)));
            Assert.That(msgs[9].SenderId, Is.EqualTo("angus"));
            Assert.That(msgs[9].Content, Is.EqualTo("My Master card is \\*redacted\\*"));

            Assert.That(msgs[10].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470916)));
            Assert.That(msgs[10].SenderId, Is.EqualTo("angus"));
            Assert.That(msgs[10].Content, Is.EqualTo("My American Express card is \\*redacted\\*"));


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
            messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(1448470916)), "angus", "My visa card is 4587852252415674"));
            messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(1448470916)), "angus", "My JCB card is 3530111333300000"));
            messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(1448470916)), "angus", "My Master card is 5555555555554444"));
            messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(1448470916)), "angus", "My American Express card is 371449635398431"));
            

            conversation = new Conversation("Test conversation", messages);

        }
    }
}
