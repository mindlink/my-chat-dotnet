using System;
using System.Linq;
using Xunit;

namespace MindLink.Recruitment.MyChat.Tests
{
    /// <summary>
    /// Run tests on the CLAParser class using.
    /// </summary>
    public class CLAParserTests
    {
        /// <summary>
        /// Check if correct exception is thrown if not enough arguments are given.
        /// </summary>
        [Fact]
        public void NotEnoughArgumentsToParse()
        {
            // Arrange
            var parser = new CLAParser();
            // Act & Assert
            var ex = Assert.Throws<IndexOutOfRangeException>(() => new CLAParser().ParseCommandLineArguments(
                new string[] { "a" }));
            Assert.Equal("2 or more parameters required.", ex.Message);
        }

        /// <summary>
        /// Check if the correct exception is thrown when too many arguments are given.
        /// </summary>
        [Fact]
        public void TooManyArgumentsToParse()
        {
            // Arrange
            var parser = new CLAParser();
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new CLAParser().ParseCommandLineArguments(
                new string[] { "a", "a", "a", "a", "a", "a" }));
            Assert.Equal("Too many parameters provided", ex.Message);
        }

        /// <summary>
        /// Checks if Input and Output paths are correct.
        /// </summary>
        [Fact]
        public void TwoArgumentsParsed()
        {
            // Arrange
            var parser = new CLAParser();
            // Act & Assert
            Assert.Equal("chat.txt", parser.ParseCommandLineArguments(
                new string[] { "chat.txt", "chat.json", }).InputFilePath);
            Assert.Equal("chat.json", parser.ParseCommandLineArguments(
                new string[] { "chat.txt", "chat.json", }).OutputFilePath);
        }

        /// <summary>
        /// Check if four arguments are parsed correctly
        /// </summary>
        [Fact]
        public void FourArgumentsParsed()
        {
            // Arrange
            var parser = new CLAParser();
            // Act & Assert
            Assert.Equal("chat.txt", parser.ParseCommandLineArguments(
                new string[] { "chat.txt", "chat.json", "name", "bob" }).InputFilePath);

            Assert.Equal("chat.json", parser.ParseCommandLineArguments(
                new string[] { "chat.txt", "chat.json", "name", "bob" }).OutputFilePath);

            Assert.Equal("bob", parser.ParseCommandLineArguments(
                new string[] { "chat.txt", "chat.json", "name", "bob" }).Filter);

            Assert.True(parser.ParseCommandLineArguments(
                new string[] { "chat.txt", "chat.json", "name", "bob" }).FilterID);
        }

        /// <summary>
        /// Check if the name filter returns a name and that Blacklist has not changed.
        /// </summary>
        [Fact]
        public void NameFilterParsed()
        {
            // Arrange
            var parser = new CLAParser();
            // Act & Assert
            var args = new string[] { "chat.txt", "chat.json", "name", "bob" };
            Assert.Equal("bob", parser.ParseCommandLineArguments(args).Filter);

            Assert.True(parser.ParseCommandLineArguments(args).FilterID);

            Assert.False(parser.ParseCommandLineArguments(args).Blacklist);    
        }

        /// <summary>
        /// Check if filter returns a keyword with FilterID returning True and blacklist
        /// remaining unchanged.
        /// </summary>
        [Fact]
        public void WordFilterParsed()
        {
            // Arrange
            var parser = new CLAParser();

            // Act & Assert
            Assert.Equal("here", parser.ParseCommandLineArguments(
                new string[] { "chat.txt", "chat.json", "name", "here" }).Filter);

            Assert.False(parser.ParseCommandLineArguments(
                new string[] { "chat.txt", "chat.json", "word", "here" }).FilterID);

            Assert.False(parser.ParseCommandLineArguments(
                new string[] { "chat.txt", "chat.json", "name", "here" }).Blacklist);    
        }

        /// <summary>
        /// Checks if blacklist returns true and the BlacklistWords list is populated.
        /// </summary>
        [Fact]
        public void BlacklistParsed()
        {
            // Arrange
            var parser = new CLAParser();
            // Act & Assert
            var args = new string[] { "chat.txt", "chat.json", "blacklist", "blacklist.txt" };
            Assert.Null(parser.ParseCommandLineArguments(args).Filter);

            Assert.False(parser.ParseCommandLineArguments(args).FilterID);

            Assert.True(parser.ParseCommandLineArguments(args).Blacklist);

            Assert.Equal(8, parser.ParseCommandLineArguments(args).BlacklistWords.ToList().Count); 
        }
    }
}
