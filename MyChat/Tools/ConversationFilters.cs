using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MyChat.Models;

namespace MyChat.Tools
{

    /// <summary>
    /// A class used for applying filters to the conversation.
    /// </summary>
    public sealed class ConversationFilters
    {

        /// <summary>
        /// An instance if the <see cref="Conversation"/> class.
        /// </summary>
        public Conversation conversation;

        /// <summary>
        /// The specified user based on which messages are filtered.
        /// </summary>
        public string userMessagesFilter;

        /// <summary>
        /// The specified keyword based on which messages are filtered.
        /// </summary>
        public string keywordMessagesFilter;

        /// <summary>
        /// The set of words which are hidden from messages.
        /// </summary>
        public string[] messageHiddenWords;

        /// <summary>
        /// The replacement word for the every word in the specified set of hidden words.
        /// </summary>
        public string messageHiddenWordReplacement;

        /// <summary>
        /// Initializes an instance of the class.
        /// </summary>
        /// <param name="userFilter">
        /// The username filter.
        /// </param>
        /// <param name="keywordFilter">
        /// The keyword filter.
        /// </param>
        /// <param name="hiddenWords">
        /// The hidden words filter.
        /// </param>
        public ConversationFilters(string userFilter, string keywordFilter, string[] hiddenWords)
        {
            this.userMessagesFilter = userFilter;
            this.keywordMessagesFilter = keywordFilter;
            this.messageHiddenWords = hiddenWords;
            this.messageHiddenWordReplacement = "\\*redacted\\*";
        }

        /// <summary>
        /// Applies filters to an instance of the Conversation class.
        /// </summary>
        /// <param name="conversation">
        /// A instance of the <see cref="Conversation"/> class.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when there is a problem with the arguments.
        /// </exception>
        /// <exception cref="RegexMatchTimeoutException">
        /// THrown when there is problem executing the regex.
        /// </exception>
        public Conversation ApplyFilters(Conversation conversation)
        {
            try
            {
                this.conversation = conversation;
                this.ApplyUserMessageFilter();
                this.ApplyKeywordMessageFilter();
                this.ReplaceHiddenWords();
                return this.conversation;
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException(e.Message);
            }
            catch (RegexMatchTimeoutException e)
            {
                throw new RegexMatchTimeoutException(e.Message);
            }

        }


        /// <summary>
        /// Applies the user filter to an instance of the conversation.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// Thrown when there is a problem with the arguments.
        /// </exception>
        public void ApplyUserMessageFilter ()
        {
            try
            {
                if (!this.userMessagesFilter.Equals(""))
                {
                    this.conversation.messages = this.conversation.messages.Where(message => message.sender.username.Equals(this.userMessagesFilter));
                }
            }
            catch (NullReferenceException)
            {
                throw new ArgumentException("The user filter was not specified.");
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("Not enough arguments to apply the user filter.");
            }
        }


        /// <summary>
        /// Applies the keyword filter to an instance of the conversation.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// Thrown when there is a problem with the arguments.
        /// </exception>
        /// <exception cref="RegexMatchTimeoutException">
        /// THrown when there is problem executing the regex.
        /// </exception>
        public void ApplyKeywordMessageFilter()
        {
            try
            {
                if (!this.keywordMessagesFilter.Equals(""))
                {
                    var messages = new List<Message>();
                    foreach (Message message in this.conversation.messages.Where(message => message.content.Contains(this.keywordMessagesFilter)))
                    {
                        foreach (string word in message.content.Split(' '))
                        {
                            if (string.Equals(Regex.Replace(word, @"[^\w\d]", ""), this.keywordMessagesFilter, StringComparison.OrdinalIgnoreCase))
                            {
                                messages.Add(message);
                            }
                        }
                    }
                    this.conversation.messages = messages;

                }
            }
            catch (NullReferenceException)
            {
                throw new ArgumentException("The keyword filter was not specified.");
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("Not enough arguments to apply the keyword filter.");
            }
            catch (RegexMatchTimeoutException)
            {
                throw new RegexMatchTimeoutException("Finding the keyword filter is taking too long.");
            }
        }


        /// <summary>
        /// Applies the hidden word filter to an instance of the conversation.
        /// <exception cref="ArgumentException">
        /// Thrown when there is a problem with the arguments.
        /// </exception>
        /// <exception cref="RegexMatchTimeoutException">
        /// THrown when there is problem executing the regex.
        /// </exception>
        public void ReplaceHiddenWords()
        {
            try
            {
                foreach (string hiddenWord in this.messageHiddenWords)
                {
                    if (!hiddenWord.Equals(""))
                    {
                        foreach (Message message in this.conversation.messages)
                        {
                            foreach (string word in message.content.Split(' '))
                            {
                                if (string.Equals(Regex.Replace(word, @"[^\w\d]", ""), hiddenWord, StringComparison.OrdinalIgnoreCase))
                                {
                                    message.content = Regex.Replace(message.content, word, this.messageHiddenWordReplacement, RegexOptions.IgnoreCase);
                                }
                            }
                        }
                    }
                }
            }
            catch (NullReferenceException)
            {
                throw new ArgumentException("The hidden words filter was not specified.");
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("Not enough arguments to replace the hidden words.");
            }
            catch (RegexMatchTimeoutException)
            {
                throw new RegexMatchTimeoutException("Replacing the hidden words is taking too long.");
            }
        }

    }
}
