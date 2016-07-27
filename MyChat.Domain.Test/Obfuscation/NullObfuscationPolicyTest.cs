namespace MindLink.Recruitment.MyChat.Domain.Test.Obfuscation
{
    using Domain.Obfuscation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class NullObfuscationPolicyTest
    {
        [TestMethod]
        public void It_should_always_preserve_the_original_input()
        {
            var policy = new NullObfuscationPolicy();

            foreach (string input in new string[] { null, "", "Test" })
                Assert.AreEqual(input, policy.Obfuscate(input));
        }
    }
}
