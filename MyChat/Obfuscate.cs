namespace MindLink.Recruitment.MyChat
{
    /// <summary>
    /// All user IDs are obfuscated in the output.
    /// </summary>
    public sealed class Obfuscate
    {
        /// <param name="word">
        /// The string value to be obfuscated.
        /// </param>
        public static string ObfuscateString(string word)
        {

            char[] wordChars = word.ToCharArray();

            if (wordChars.Length > 1)
            {
                for (int i = 0; i < wordChars.Length; i += 2)
                {
                    if (i < wordChars.Length / 2)
                    {
                        var temp = wordChars[i];
                        wordChars[i] = wordChars[wordChars.Length - 1];
                        wordChars[wordChars.Length - 1] = temp;
                    }
                }
            }
            else wordChars[0] = '*';
           
            return new string(wordChars);
        }

    }
}
