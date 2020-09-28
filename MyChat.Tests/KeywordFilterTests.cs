using NUnit.Framework;

namespace MindLink.Recruitment.MyChat.Tests
{
    [TestFixture]
    public class KeywordFilterTests
    {
        [Test]
        public void keyword_in_message_returns_true()
        {
            IFilterer kf = new KeywordFilterer("message");
            IMessage msg = new Message(new []{"1234","david","this is a message"});
            Assert.That(kf.Filter(msg), Is.EqualTo(true));
        }

        [Test]
        public void keyword_not_in_message_returns_false()
        {
            IFilterer kf = new KeywordFilterer("nonexistent");
            IMessage msg = new Message(new []{"1234","david","this is a message"});
            Assert.That(kf.Filter(msg), Is.EqualTo(false));
        }
        
        [Test]
        public void word_in_message_that_contains_a_keyword_but_is_different_returns_false()
        {
            IFilterer kf = new KeywordFilterer("with");
            IMessage msg = new Message(new []{"1234","david","the word within will not be found"});
            Assert.That(kf.Filter(msg), Is.EqualTo(false));
        }
        
        [Test]
        public void capital_keyword_will_find_lowercase_word_in_a_message_if_they_are_technically_the_same()
        {
            IFilterer kf = new KeywordFilterer("Wonderful");
            IMessage msg = new Message(new []{"1234","david","words are wonderful sometimes!"});
            Assert.That(kf.Filter(msg), Is.EqualTo(true));
            
        }
        
        [Test]
        public void upper_case_keyword_finds_lowercase_word_in_message()
        {
            IFilterer kf = new KeywordFilterer("Big");
            IMessage msg = new Message(new []{"1234","david","big words"});
            Assert.That(kf.Filter(msg), Is.EqualTo(true));
        }
        
        [Test]
        public void longer_keyword_found_in_shorter_word_returns_false()
        {
            IFilterer kf = new KeywordFilterer("hereisalongwordplusextra");
            IMessage msg = new Message(new []{"1234","david","hereisalongword is not a real word"});
            Assert.That(kf.Filter(msg), Is.EqualTo(false));
        }
        
        [Test]
        public void keyword_without_punctuation_finds_word_with_punctuation()
        {
            //hi! and hi are the same word.
            IFilterer kf = new KeywordFilterer("hi");
            IMessage msg = new Message(new []{"1234","david","hi! this is a message"});
            Assert.That(kf.Filter(msg), Is.EqualTo(true));
            
        }

        [Test]
        public void keyword_with_punctuation_finds_word_with_punctuation()
        {
            IFilterer kf = new KeywordFilterer("hi!");
            IMessage msg = new Message(new []{"1234","david","hi! this is a message"});
            Assert.That(kf.Filter(msg), Is.EqualTo(true));

        }
        
        [Test]
        public void keyword_and_a_word_with_discordant_capitalisation_and_punctuation_still_match()
        {
            IFilterer kf = new KeywordFilterer("hi");
            IMessage msg = new Message(new []{"1234","david","hI! this is a message"});
            Assert.That(kf.Filter(msg), Is.EqualTo(true));
        }
        
        
        [Test]
        public void I_with_apostrophe_is_not_found_when_keyword_is_I()
        {
            IFilterer kf = new KeywordFilterer("I");
            IMessage msg = new Message(new []{"1234","david","I'm like pie."});
            Assert.That(kf.Filter(msg), Is.EqualTo(false));
        }
    }
}