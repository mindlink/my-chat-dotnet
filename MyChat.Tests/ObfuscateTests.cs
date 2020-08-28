namespace MindLink.Recruitment.MyChat.Tests
{
    using NUnit.Framework;
    using MindLink.Recruitment.MyChat;

    /// <summary>
    /// Tests for the <see cref="Obfuscate"/>.
    /// </summary>
    [TestFixture]
    class ObfuscateTests
    {
        /// <summary>
        /// Tests the obfuscate user ids.
        /// </summary>
        [Test]
        public void ObfuscateUserIdTests()
        {
            Assert.That(Obfuscate.ObfuscateString("q"), Is.EqualTo("*"));
            Assert.That(Obfuscate.ObfuscateString("angus"), Is.EqualTo("sngua"));
            Assert.That(Obfuscate.ObfuscateString("mike"), Is.EqualTo("eikm"));
        }
    }
}
