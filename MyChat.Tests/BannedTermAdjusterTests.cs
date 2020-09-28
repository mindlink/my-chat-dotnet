using NUnit.Framework;

namespace MindLink.Recruitment.MyChat.Tests
{
    [TestFixture]
    public class BannedTermAdjusterTests
    {
        public KeywordFilterer GetKeywordFilterer(string term)
        {
            return new KeywordFilterer(term);
        }
        [Test]
        public void banned_term_is_redacted()
        {
            IAdjuster ad = new BannedTermRedactor("banned", GetKeywordFilterer("banned"));
            IMessage m = new Message(new[] {"1234", "david", "banned word should be redacted"});
            Assert.That(ad.Adjust(m).content, Is.EqualTo("*redacted* word should be redacted"));
        }

        [Test]
        public void non_existent_banned_term_not_removed_from_message()
        {
            IAdjuster ad = new BannedTermRedactor("nothing", GetKeywordFilterer("nothing"));
            IMessage m = new Message(new[] {"1234", "david", "banned term is not in message so no changes"});
            Assert.That(ad.Adjust(m).content, Is.EqualTo("banned term is not in message so no changes"));
        }
        
        [Test]
        public void banned_word_contained_within_a_longer_word_is_not_redacted()
        {
            IAdjuster ad = new BannedTermRedactor("with", GetKeywordFilterer("with"));
            IMessage m = new Message(new[] {"1234", "david", "a longer term within is not redacted"});
            Assert.That(ad.Adjust(m).content, Is.EqualTo("a longer term within is not redacted"));
        }
        
        [Test]
        public void upper_case_banned_word_still_redacts_lowercase_word_in_message()
        {
            IAdjuster ad = new BannedTermRedactor("Casing", GetKeywordFilterer("Casing"));
            IMessage m = new Message(new[] {"1234", "david", "the casing of the word shouldn't matter"});
            Assert.That(ad.Adjust(m).content, Is.EqualTo("the *redacted* of the word shouldn't matter"));
        }
        
        [Test]
        public void multiple_occurrences_of_a_banned_term_in_a_message_are_all_removed()
        {
            IAdjuster ad = new BannedTermRedactor("pie", GetKeywordFilterer("pie"));
            IMessage m = new Message(new[] {"1234", "david", "I really want pie and I want pie now. pie."});
            Assert.That(ad.Adjust(m).content,
                Is.EqualTo("I really want *redacted* and I want *redacted* now. *redacted*."));
        }
        
        [Test]
        public void multiple_occurrences_of_a_banned_term_with_punctuation_and_capitalisation_muckups_all_removed()
        {
            IAdjuster ad = new BannedTermRedactor("pie", GetKeywordFilterer("pie"));
            IMessage m = new Message(new[] {"1234", "david", "I really want Pie and I want piE now. pie!"});
            Assert.That(ad.Adjust(m).content,
                Is.EqualTo("I really want *redacted* and I want *redacted* now. *redacted*!"));
        }
        
        [Test]
        public void chain_of_redacted_words_adjusts_message_properly()
        {
            IMessage m = new Message(new[] {"1234", "david", "I'm called david!"});
            IAdjuster bt = new BannedTermRedactor("I'm", GetKeywordFilterer("I'm"));
            IAdjuster nextBt = new BannedTermRedactor("david", GetKeywordFilterer("david"));
            bt.NextAdjuster = nextBt;
            Assert.That(bt.Adjust(m).content, Is.EqualTo("*redacted* called *redacted*!"));
        }
        
        [Test]
        public void single_letter_redacted_item_does_not_catch_apostrophe_items()
        {
            IMessage m = new Message(new[] {"1234", "david", "I I'm I I'm I I'm called david!"});
            IAdjuster bt = new BannedTermRedactor("I", GetKeywordFilterer("I"));
            Assert.That(bt.Adjust(m).content, Is.EqualTo("*redacted* I'm *redacted* I'm *redacted* I'm called david!"));
        }
        
        [Test]
        public void word_with_non_terminal_punctuation_is_not_redacted()
        {
            //there != there's
            IMessage m = new Message(new[] {"1234", "david", "there's got to be more to eat"});
            IAdjuster bt = new BannedTermRedactor("there", GetKeywordFilterer("there"));
            Assert.That(bt.Adjust(m).content, Is.EqualTo("there's got to be more to eat"));
        }
        
    }
}