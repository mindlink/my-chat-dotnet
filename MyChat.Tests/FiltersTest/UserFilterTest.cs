using MindLink.Recruitment.MyChat.Filters;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MindLink.Recruitment.MyChat.Tests.FiltersTest
{
    class UserFilterTest
    {
        [Test]
        public void UserFilterPassTest()
        {
            var testFilter = new UserFilter(new string[] { "sam" });
            var resultOut = testFilter.FilterInput("1448470905", "sam", "How are you today");

            Assert.That(resultOut[0].Equals("1448470905"));
            Assert.That(resultOut[1].Equals("sam"));
            Assert.That(resultOut[2].Equals("How are you today"));
        }

        [Test]
        public void UserFilterFailTest()
        {
            var testFilter = new UserFilter(new string[] { "sam" });
            var resultOut = testFilter.FilterInput("1448470905", "bob", "How are you today");

            Assert.That(resultOut[0].Equals(""));
            Assert.That(resultOut[1].Equals(""));
            Assert.That(resultOut[2].Equals(""));
        }

        [Test]
        public void UserFilterHalfWordTest()
        {
            var testFilter = new UserFilter(new string[] { "bob" });
            var resultOut = testFilter.FilterInput("1448470905", "bobby", "How are you today");

            Assert.That(resultOut[0].Equals(""));
            Assert.That(resultOut[1].Equals(""));
            Assert.That(resultOut[2].Equals(""));
        }
    }
}
