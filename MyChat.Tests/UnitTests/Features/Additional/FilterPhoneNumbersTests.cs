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
    /// Class to test whether phone number filter works, this class will either 
    /// redact the phone numbers or not. No errors should be thrown as no
    /// arguments are passed to this class
    /// </summary>
    [TestFixture]
    public class FilterPhoneNumbersTests
    {
        // IStrategyFilter to be tested
        private IStrategyFilter phoneNumbersFilter;
        // Conversation to test the filter with
        private Conversation conversation;

        /// <summary>
        /// Test to test whether the FilterPhoneNumbers filter works as intended
        /// </summary>
        [Test]
        public void FilterPhoneNumbersFiltersPhoneNumbers() 
        {
            // Filter to be tested 
            phoneNumbersFilter = new FilterPhoneNumbers();
            // GENERATE conversation to be filtered
            MakeConverSation();
            // INSTANTIATE new conversation called filteredConversation,
            // SET it to the returned conversation from the filter
            Conversation filteredConversation = phoneNumbersFilter.ApplyFilter(conversation);
            // INSTANTIATE a list as the conversation msgs IEnumerable converted to list
            IList<Message> msgs = filteredConversation.Messages.ToList<Message>();

            Assert.That(filteredConversation.Name, Is.EqualTo("Test conversation"));

            Assert.That(filteredConversation.FilterMessage[0], Is.EqualTo("No filter errors"));

            Assert.That(msgs[7].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470916)));
            Assert.That(msgs[7].SenderId, Is.EqualTo("angus"));
            Assert.That(msgs[7].Content, Is.EqualTo("My phone number is \\*redacted\\*"));
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
            messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(1448470916)), "angus", "My phone number is 07864275981"));

            conversation = new Conversation("Test conversation", messages);

        }
    }
}
