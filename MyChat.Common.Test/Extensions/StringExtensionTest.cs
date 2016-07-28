namespace MindLink.Recruitment.MyChat.Common.Test.Extensions
{
    using Common.Extensions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StringExtensionsTest
    {
        [TestMethod]
        public void It_should_reverse_the_input_string()
        {
            string input = "abcd";
            string reversed = StringExtensions.Reverse(input);
            Assert.AreEqual("dcba", reversed);
        }

        [TestMethod]
        public void It_should_convert_generate_the_original_value_when_reversing_twice()
        {
            string input = "abcd";

            string reversed1 = StringExtensions.Reverse(input);
            string reversed2 = StringExtensions.Reverse(reversed1);

            Assert.AreEqual(input, reversed2);
        }
    }
}
