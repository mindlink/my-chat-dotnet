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
    /// Class to test the obfuscate user IDs
    /// </summary>
    [TestFixture]
    public class FilterObfuscateUsersTests
    {
        // IStrategyFilter to be tested
        private IStrategyFilter obfuscateUsersFilter;
        // Conversation to test the filter with
        private Conversation conversation;

        /// <summary>
        /// Test whether the obfuscate users filter hides the user ID's as expected
        /// </summary>
        [Test]
        public void FilterObfuscateUsersObfuscatesUsers() 
        {
            // Filter to be tested 
            obfuscateUsersFilter = new FilterObfuscateUsers();
            // GENERATE conversation to be filtered
            MakeConverSation();
            // INSTANTIATE new conversation called filteredConversation,
            // SET it to the returned conversation from the filter
            Conversation filteredConversation = obfuscateUsersFilter.ApplyFilter(conversation);
            // INSTANTIATE a list as the conversation msgs IEnumerable converted to list
            IList<Message> msgs = filteredConversation.Messages.ToList<Message>();

            Assert.That(filteredConversation.Name, Is.EqualTo("Test conversation"));

            Assert.That(filteredConversation.FilterMessage[0], Is.EqualTo("No filter errors"));


            Assert.That(msgs[0].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470901)));
            Assert.That(msgs[0].SenderId, Is.EqualTo("User3071"));
            Assert.That(msgs[0].Content, Is.EqualTo("Hello there!"));

            Assert.That(msgs[1].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470905)));
            Assert.That(msgs[1].SenderId, Is.EqualTo("User4222"));
            Assert.That(msgs[1].Content, Is.EqualTo("how are you?"));

            Assert.That(msgs[2].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470906)));
            Assert.That(msgs[2].SenderId, Is.EqualTo("User3071"));
            Assert.That(msgs[2].Content, Is.EqualTo("I'm good thanks, do you like pie?"));

            Assert.That(msgs[3].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470910)));
            Assert.That(msgs[3].SenderId, Is.EqualTo("User4222"));
            Assert.That(msgs[3].Content, Is.EqualTo("no, let me ask Angus..."));

            Assert.That(msgs[4].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470912)));
            Assert.That(msgs[4].SenderId, Is.EqualTo("User5423"));
            Assert.That(msgs[4].Content, Is.EqualTo("Hell yes! Are we buying some pie?"));

            Assert.That(msgs[5].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470914)));
            Assert.That(msgs[5].SenderId, Is.EqualTo("User3071"));
            Assert.That(msgs[5].Content, Is.EqualTo("No, just want to know if there's anybody else in the pie society..."));

            Assert.That(msgs[6].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470915)));
            Assert.That(msgs[6].SenderId, Is.EqualTo("User5423"));
            Assert.That(msgs[6].Content, Is.EqualTo("YES! I'm the head pie eater there..."));

            Assert.That(msgs[7].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470916)));
            Assert.That(msgs[7].SenderId, Is.EqualTo("User5423"));
            Assert.That(msgs[7].Content, Is.EqualTo("4578075020647520"));

            Assert.That(msgs[8].Timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470916)));
            Assert.That(msgs[8].SenderId, Is.EqualTo("User5423"));
            Assert.That(msgs[8].Content, Is.EqualTo("My phone number is 07864275981"));

        }

        /// <summary>
        /// Test whether the obfuscate users filter prints out a message into the conversation
        /// when there are no users to obfuscate
        /// </summary>
        [Test]
        public void ObfuscateUsersObfuscatesNoUsers() 
        {
            // Filter to be tested 
            obfuscateUsersFilter = new FilterObfuscateUsers();
            // INSTANTIATE new conversation called filteredConversation,
            // pass in the name and an empty list
            Conversation filteredConversation = obfuscateUsersFilter.ApplyFilter(new Conversation("Test conversation", new List<Message>()));

            Assert.That(filteredConversation.Name, Is.EqualTo("Test conversation"));

            Assert.That(filteredConversation.FilterMessage[0], Is.EqualTo("No users to obfuscate from chat"));

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
            messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(1448470916)), "angus", "4578075020647520"));
            messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(1448470916)), "angus", "My phone number is 07864275981"));  


            conversation = new Conversation("Test conversation", messages);

        }
    }
}
