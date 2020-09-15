using System;
using NUnit.Framework;

namespace MindLink.Recruitment.MyChat.Tests
{
    [TestFixture]
    public class CommandLineArgumentParserTests
    {
        [Test]
        public void NoCommandLineArgumentsThrowError()
        {    
            string[] noItems = {" "};
            
            CommandLineArgumentParser cp = new CommandLineArgumentParser();

            Assert.That(() => cp.ParseCommandLineArguments(noItems),
                Throws.Exception
                    .TypeOf<ArgumentException>());
        }

        [Test]
        public void TooFewCommandLineArgumentsThrowsError()
        {
            string[] tooFewItems = {"item1"};
            
            CommandLineArgumentParser cp = new CommandLineArgumentParser();

            Assert.That(() => cp.ParseCommandLineArguments(tooFewItems),
                Throws.Exception
                    .TypeOf<ArgumentException>());
        }

        [Test]
        public void TooManyCommandLineArgumentsThrowsError()
        {
            string[] tooManyItems = {"item1", "item2", "item3"};
            
            CommandLineArgumentParser cp = new CommandLineArgumentParser();

            Assert.That(() => cp.ParseCommandLineArguments(tooManyItems),
                Throws.Exception
                    .TypeOf<ArgumentException>());
        }
        
        

    }
}