namespace MindLink.Recruitment.MyChat
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public abstract class ReplaceOptions : ConversationOptions
    {
        public abstract void Replace(List<Message> messages, string[] wordsToReplace);

        /// <summary>
        /// Generate a list of regular expressions from a list of words to replace
        /// </summary>
        /// <param name="wordsToReplace">
        /// List of keywords
        /// </param>
        /// <returns>
        /// List of regular expressions
        /// </returns>
        public List<Regex> GenerateListOfRegexForKeyWords(string[] wordsToReplace)
        {
            var wordsToReplaceRegexList = new List<Regex>();

            ///Generate list of regular expressions for each word to replace
            foreach (var wordToReplace in wordsToReplace)
            {
                wordsToReplaceRegexList.Add(new Regex(@"\b" + wordToReplace + @"[!?.,']?\b", RegexOptions.IgnoreCase));
            }

            return wordsToReplaceRegexList;
        }

    }

    public class ReplaceWithRedact : ReplaceOptions
    {
        /// <summary>
        /// Replace words in messages that match any of the keywords with *redacted*
        /// </summary>
        /// <param name="messages"></param>
        /// <param name="wordsToReplace"></param>
        public override void Replace(List<Message> messages, string[] wordsToReplace)
        {
            try
            {
                var wordsToReplaceRegexList = GenerateListOfRegexForKeyWords(wordsToReplace);

                /// Loop through each message from list of messages
                foreach (var message in messages)
                {
                    var words = message.content.Split(' ');

                    /// Compare each word in a message to the list of words to replace. Replace any word that matches
                    for (var i = 0; i < words.Length; i++)
                    {
                        foreach(var wordToReplaceRegex in wordsToReplaceRegexList)
                        {
                            if (wordToReplaceRegex.IsMatch(words[i]))
                            {
                                var capturedSubstringLength = wordToReplaceRegex.Match(words[i]).Length;

                                ///Any symbols after the matched word
                                var postfixString = words[i][capturedSubstringLength..];

                                words[i] = "*redacted*" + postfixString;

                                break;
                            }
                        }
                    }

                    message.content = string.Join(' ', words);
                }
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("Something is wrong with the regular expression");
            }
        }
    }
}