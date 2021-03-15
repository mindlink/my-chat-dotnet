using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using MindLink.Recruitment.MyChat.Blacklisting;

namespace MindLink.Recruitment.MyChat.Tests.BlacklistTest
{
    class KeywordBlacklistTest
    {
        [Test]
        public void KeywordBlacklistFailTest()
        {
            var blacklistTest = new KeywordBlacklist(new string[] { "yello"}, "CENSORED!!");

            var result = blacklistTest.CheckInput("1448470905", "bobby", "Hi HELLO, Hey yello");

            Assert.That(result[2].Contains("CENSORED!!"));
            Assert.That(!result[2].Contains("yello"));

        }

        [Test]
        public void KeywordBlacklistPassTest()
        {
            var blacklistTest = new KeywordBlacklist(new string[] { "yello" }, "CENSORED!!");

            var result = blacklistTest.CheckInput("1448470905", "bobby", "Hi HELLO, Hey");

            Assert.That(!result[2].Contains("CENSORED!!"));

        }

        [Test]
        public void KeywordBlacklistPartialWordTest()
        {
            var blacklistTest = new KeywordBlacklist(new string[] { "ello" }, "CENSORED!!");

            var result = blacklistTest.CheckInput("1448470905", "bobby", "Hi HiLLO hello, Hey");

            Assert.That(!result[2].Contains("CENSORED!!"));
            Assert.That(result[2].Contains("hello"));


        }

        [Test]
        public void KeywordBlacklistPunctuationTest()
        {
            var blacklistTest = new KeywordBlacklist(new string[] { "i'm" }, "CENSORED!!");

            var result = blacklistTest.CheckInput("1448470905", "bobby", "Hi i'm hello, Hey");

            Assert.That(result[2].Contains("CENSORED!!"));
            Assert.That(!result[2].Contains("i'm"));

        }

        [Test]
        public void KeywordBlacklistEndPunctuationTest()
        {
            var blacklistTest = new KeywordBlacklist(new string[] { "pie" }, "CENSORED!!");

            var result = blacklistTest.CheckInput("1448470905", "bobby", "Hi i'm hello, Hey pie!");

            Assert.That(result[2].Contains("CENSORED!!"));
            Assert.That(!result[2].Contains("pie"));

        }
    }
}
