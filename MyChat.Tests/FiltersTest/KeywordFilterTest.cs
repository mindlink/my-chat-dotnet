using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using MindLink.Recruitment.MyChat.Filters;


namespace MindLink.Recruitment.MyChat.Tests.FiltersTest
{
    class KeywordFilterTest
    {
        [Test]
        public void KeywordFilterStandardCase()
        {
            var testFilter = new KeywordFilter(new string[] { "hi" });

            var resultOut = testFilter.FilterInput("1448470905", "test1", "How are you today");

            Assert.That(resultOut[0].Equals(""));
            Assert.That(resultOut[1].Equals(""));
            Assert.That(resultOut[2].Equals(""));
        }

        [Test]
        public void KeywordFilterPassCase()
        {
            var testFilter = new KeywordFilter(new string[] { "are" });

            var resultOut = testFilter.FilterInput("1448470905", "test1", "How are you today");

            Assert.That(resultOut[0].Equals("1448470905"));
            Assert.That(resultOut[1].Equals("test1"));
            Assert.That(resultOut[2].Equals("How are you today"));
        }

        [Test]
        public void KeywordFilterHalfWordCase()
        {
            var testFilter = new KeywordFilter(new string[] { "are" });

            var resultOut = testFilter.FilterInput("1448470905", "test1", "What about the area around backgate?");

            Assert.That(resultOut[0].Equals(""));
            Assert.That(resultOut[1].Equals(""));
            Assert.That(resultOut[2].Equals(""));
        }

    }
}
