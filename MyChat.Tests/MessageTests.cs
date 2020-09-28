using System;
using NUnit.Framework;

namespace MindLink.Recruitment.MyChat.Tests
{
    [TestFixture]
    public class MessageTests
    {
        [Test]
        public void SenderIDCorrectAfterConversionIntoMessage()
                 {
                     IMessage m = new Message(new []{"1234", "david", "a message"});
                     Assert.That(m.senderId, Is.EqualTo("david"));
                 }
        
                 [Test]
                 public void MessageContentCorrectAfterConversion()
                 {
                     IMessage m = new Message(new []{"1234", "david", "a message"});
                     Assert.That(m.content, Is.EqualTo("a message"));
                 }
        
                 [Test]
                 public void TimestampCorrectWhenCreatingNewMessage()
                 {
                     IMessage m = new Message(new []{"1448470901", "david", "a message"});
                     DateTimeOffset want = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470901"));

                     Assert.That(m.timestamp, Is.EqualTo(want));
                 }
        
    }
}