namespace MindLink.Recruitment.MyChat.Common.Extensions
{
    using System;

    public static class StringExtensions
    {
        /// <summary>
        /// Reverses a string.
        /// </summary>
        /// <param name="input">The string to reverse</param>
        /// <returns>The reversed string</returns>
        public static string Reverse(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            char[] inputArr = input.ToCharArray();
            Array.Reverse(inputArr);
            return new string(inputArr);
        }
    }
}
