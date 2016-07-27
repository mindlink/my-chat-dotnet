namespace MindLink.Recruitment.MyChat.Domain.Test.Obfuscation
{
    using Domain.Obfuscation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ReversedBase64ObfuscationPolicyTest
    {
        [TestMethod]
        public void It_should_preserve_original_value_for_empty_strings()
        {
            var policy = new ReversedBase64ObfuscationPolicy();

            string obfuscated = policy.Obfuscate(null);
            Assert.IsNull(obfuscated);

            obfuscated = policy.Obfuscate(string.Empty);
            Assert.AreEqual(string.Empty, obfuscated);
        }

        [TestMethod]
        public void It_should_generate_the_same_obfuscated_text_for_the_same_input()
        {
            var policy = new ReversedBase64ObfuscationPolicy();

            string obfuscated1 = policy.Obfuscate("a boring message");
            string obfuscated2 = policy.Obfuscate("a boring message");

            Assert.AreEqual(obfuscated2, obfuscated1);
        }
    }
}
