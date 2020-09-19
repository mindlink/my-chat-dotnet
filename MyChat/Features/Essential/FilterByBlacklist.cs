namespace MindLink.Recruitment.MyChat.Features.Essential
{
    using MindLink.Recruitment.MyChat.Interfaces.FeatureInterfaces;
    using MyChatModel.ModelData;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    public sealed class FilterByBlacklist : IStrategyFilter
    {
        /// <summary>
        /// string to contain a word which will be removed from the conversations
        /// and replaced with \*redacted*\
        /// </summary>
        private string word;

        public FilterByBlacklist(string word) 
        {
            this.word = word;
        }

        public Conversation ApplyFilter(Conversation conversation) 
        {
            // INITIALISE a bool to say whether the word was found in the conversation
            bool validInput = false;
            // FOREACH through the message in conversation messages list
            foreach (Message msg in conversation.Messages) 
            {
                validInput = true;
                // SET the content of the message to the returned
                // string from RemoveWord, passing in the current message
                msg.Content = RemoveWord(msg);
            }

            // IF there is no valid input
            if (!validInput)
            {
                // IF the word passed in is empty
                if (word == "" || word == " ")
                {
                    // THROW an ArgumentNullException, to notify the user they have specifed the filter but
                    // not supplied any arguments
                    throw new ArgumentNullException("No word was supplied for the blacklist to use");
                }
                else
                {
                    // ELSE the word was not found in the conversation, and we would like to tell the user this
                    string conversationMessage = "The word " + word + " was not found in the conversation";
                    // CALL to the conversations AddFilterMessage and pass in the message
                    conversation.AddFilterMessage(conversationMessage);
                }
            }

            return conversation;        
        }

        /// <summary>
        /// RemoveWord, removes the word and keeps characters after the word
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private string RemoveWord(Message msg)
        {
            // IF the message contains the blacklisted word
            if (msg.Content.Contains(word))
            {
                // SPLIT the message into a series of substrings, split by a space
                IList<string> subStrings = msg.Content.Split(' ').ToList<string>();

                // INITIALISE a local string called filtered message
                string filteredMessage = "";

                // FOR loop through the subStrings, in other words each word in the message
                for (int i = 0; i < subStrings.Count; i++)
                {
                    // IF the subString contains the word
                    if (subStrings[i].Contains(word))
                    {
                        // IF the length of the substring is greater than the length of the word
                        if (subStrings[i].Length > word.Length)
                        {
                            // THEN there is an additional character on the end
                            // we would like to keep

                            // INITIALISE an array of char's called letters
                            // initialise as the return of the substring ToCharArray method
                            char[] letters = subStrings[i].ToCharArray();

                            // SET the substring at i to "\*redacted\*" with the last character 
                            // in the character array appended on the end
                            subStrings[i] = "\\*redacted\\*" + letters.Last();
                        }
                        else
                        {
                            // THEN just set the string to "\*redacted\*"
                            subStrings[i] = "\\*redacted\\*";
                        }
                    }

                    // ADD the substring to the filtered message, with a space appended on the end of the string 
                    filteredMessage += subStrings[i] + ' ';
                }

                return filteredMessage;
            }
            else 
            {
                return msg.Content;
            }
        }
    }
}
