using System;
using System.IO;
using NUnit.Framework;

namespace MindLink.Recruitment.MyChat.Tests
{
    [TestFixture]
    public class ConversationReaderTests
    {
        [Test]
        public void ConversationCanNotBeReadFromInvalidPath()
        {
            Assert.Throws<FileNotFoundException>(() => new ConversationReader("WrongPath"));
        }
    }
}
