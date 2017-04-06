namespace MindLink.Recruitment.MyChat.Tests
{
    using global::MyChat;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Tests for the <see cref="Activity"/>.
    /// </summary>
    [TestClass]
    public class ActivityTests
    {
        /// <summary>
        /// Tests that the UserActivity list is correctly generated in Conversation object.
        /// </summary>
        [TestMethod]
        public void ActivityTest()
        {
            ConversationExporter exporter = new ConversationExporter();

            exporter.ExportConversation("chat.txt", "chat.json", null, null, null, false, false);

            FileStream stream = new FileStream("chat.json", FileMode.Open);

            // using statement to correctly dispose StreamReader
            using (StreamReader reader = new StreamReader(stream))
            {
                string serializedConversation = reader.ReadToEnd();

                Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

                // setting it to null to re-create.
                savedConversation.UserActivity = null;

                // recreating user activity.
                Activity.SetUserActivityInConversation(savedConversation);

                // checking if activity is null.
                Assert.IsNotNull(savedConversation.UserActivity);

                // checking if user activity is the correct type.
                Assert.IsInstanceOfType(savedConversation.UserActivity, typeof(List<UserActivity>));

                Assert.IsInstanceOfType(savedConversation.UserActivity.ToList()[0], typeof(UserActivity));
                Assert.AreEqual("bob", savedConversation.UserActivity.ToList()[0].UserName);
                Assert.AreEqual(4, savedConversation.UserActivity.ToList()[0].MessagesNo);

                Assert.IsInstanceOfType(savedConversation.UserActivity.ToList()[1], typeof(UserActivity));
                Assert.AreEqual("mike", savedConversation.UserActivity.ToList()[1].UserName);
                Assert.AreEqual(3, savedConversation.UserActivity.ToList()[1].MessagesNo);

                Assert.IsInstanceOfType(savedConversation.UserActivity.ToList()[2], typeof(UserActivity));
                Assert.AreEqual("angus", savedConversation.UserActivity.ToList()[2].UserName);
                Assert.AreEqual(3, savedConversation.UserActivity.ToList()[2].MessagesNo);
            }
        }
    }
}
