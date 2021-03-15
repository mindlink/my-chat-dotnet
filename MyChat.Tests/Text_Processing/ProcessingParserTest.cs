using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using MindLink.Recruitment.MyChat.Text_Processing;
using MindLink.Recruitment.MyChat.Filters;
using MindLink.Recruitment.MyChat.Blacklisting;

namespace MindLink.Recruitment.MyChat.Tests.Text_Processing
{
    class ProcessingParserTest
    {
        [Test]
        public void ProcessingParserPassTest()
        {
            var parser = new ProcessingParser(new BaseFilter[] { new UserFilter(new string[] { "bob" }) },
                new IBlacklist[] { new KeywordBlacklist(new string[] { "pie"}, "redacted") });

            var result = parser.RunLineCheck("1448470905", "bob", "Hi, there how are you");

            Assert.That(result[0].Equals("1448470905"));
            Assert.That(result[1].Equals("bob"));
            Assert.That(result[2].Equals("Hi, there how are you"));
        }

        [Test]
        public void ProcessingParserUserFilterFailTest()
        {
            var parser = new ProcessingParser(new BaseFilter[] { new UserFilter(new string[] { "bob" }) },
                new IBlacklist[] { new KeywordBlacklist(new string[] { "pie" }, "redacted") });

            var result = parser.RunLineCheck("1448470905", "fred", "Hi, there how are you");

            Assert.That(result[0].Equals(""));
            Assert.That(result[1].Equals(""));
            Assert.That(result[2].Equals(""));
        }
    }
}
