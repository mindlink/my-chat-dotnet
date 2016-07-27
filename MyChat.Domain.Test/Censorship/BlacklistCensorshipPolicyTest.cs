namespace MindLink.Recruitment.MyChat.Domain.Test.Sensorship
{
    using Censorship;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BlacklistCensorshipPolicyTest
    {
        [TestMethod]
        public void It_can_censor_blacklisted_keywords_which_appear_at_word_boundaries()
        {
            var blacklist = new string[] { "bad1", "bad2", "bad3" };
            var replacement = "good";
            var input = "bad1 bad2 bad3.";
            var policy = new BlacklistCensorshipPolicy(blacklist, replacement);
            var censoredInput = policy.Censor(input);
            Assert.AreEqual("good good good.", censoredInput);
        }

        [TestMethod]
        public void It_can_censor_blacklisted_keywords_regardless_of_case()
        {
            var blacklist = new string[] { "bad" };
            var replacement = "good";
            var input = "BaD";
            var policy = new BlacklistCensorshipPolicy(blacklist, replacement);
            var censoredInput = policy.Censor(input);
            Assert.AreEqual("good", censoredInput);
        }

        [TestMethod]
        public void It_cannot_censor_blacklisted_keywords_in_the_middle_of_a_word()
        {
            var blacklist = new string[] { "bad" };
            var replacement = "good";
            var input = "badass";
            var policy = new BlacklistCensorshipPolicy(blacklist, replacement);
            var censoredInput = policy.Censor(input);
            Assert.AreEqual("badass", censoredInput);
        }
    }
}
