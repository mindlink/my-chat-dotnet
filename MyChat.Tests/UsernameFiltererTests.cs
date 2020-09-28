using NUnit.Framework;

namespace MindLink.Recruitment.MyChat.Tests
{
    [TestFixture]
    public class UsernameFiltererTests
    {
        [Test]
         public void name_in_sender_and_filter_match()
         {
             IFilterer unf = new UserNameFilterer("david");
             IMessage m = new Message(new [] {"1234", "david", "a message"});
             Assert.That(unf.Filter(m), Is.EqualTo(true));
         }
         
         [Test]
         public void single_letter_user_name_found()
         {
             IFilterer unf = new UserNameFilterer("d");
             IMessage m = new Message(new [] {"1234", "d", "a message"});
             Assert.That(unf.Filter(m), Is.EqualTo(true));
         }

         [Test]
         public void shorter_name_to_be_found_NOT_found_in_longer_name()
         {
             // If someone searches for "davide" and we find the name is "david"
             // that should fail. 
             IFilterer unf = new UserNameFilterer("davide");
             IMessage m = new Message(new [] {"1234", "david", "a message"});
             Assert.That(unf.Filter(m), Is.EqualTo(false));
         }

         [Test]
         public void name_to_be_found_not_in_sender_ID()
         {
             IFilterer unf = new UserNameFilterer("not there");
             IMessage m = new Message(new []{"1234", "david", "a message"});
             Assert.That(unf.Filter(m), Is.EqualTo(false));
         }
    }
}