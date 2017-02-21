using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat
{

    /// <summary>
    /// Represents a the filters appied to the conversation.
    /// </summary>
    public sealed class ConversationFilters
    {

        /// <summary>
        /// The conversation.
        /// </summary>
        public Conversation conversation;

        /// <summary>
        /// Applies filters to the conversation.
        /// </summary>
        /// <param name="conversation">
        /// The conversation.
        /// </param>
        /// <param name="configuration">
        /// The conversation exporter configuration.
        /// </param>
        /// <exception cref="NullReferenceException">
        /// Thrown when any of the given arguments is empty.
        /// </exception>
        public Conversation ApplyFilters(Conversation conversation, ConversationExporterConfiguration configuration)
        {
            try
            {
                this.conversation = conversation;
                this.ApplyUserMessageFilter(configuration.userMessagesFilter);
                this.ApplyKeywordMessageFilter(configuration.keywordMessagesFilter);
                this.ReplaceHiddenWords(configuration.messageHiddenWords, configuration.messageHiddenWordReplacement);
                return conversation;
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException("Something went wrong while applying the conversation filters.");
            }

        }


        /// <summary>
        /// Applies the <parammref name="userMessagesFilter"/> to the conversation.
        /// </summary>
        /// <param name="userMessagesFilter">
        /// The specified user based on which messages are filtered.
        /// </param>
        /// <exception cref="NullReferenceException">
        /// Thrown when any of the given arguments is empty.
        /// </exception>
        public void ApplyUserMessageFilter (string userMessagesFilter)
        {
            try
            {
                if (!userMessagesFilter.Equals(""))
                {
                    this.conversation.messages = this.conversation.messages.Where(message => message.sender.username.Equals(userMessagesFilter));
                }
            }
            catch(NullReferenceException)
            {
                throw new NullReferenceException("Something went wrong while applying the user conversation filters.");
            }
        }


        /// <summary>
        /// Applies the <parammref name="keywordMessagesFilter"/> to the conversation.
        /// </summary>
        /// <param name="keywordMessagesFilter">
        /// The specified keyword based on which messages are filtered.
        /// </param>
        /// <exception cref="NullReferenceException">
        /// Thrown when any of the given arguments is empty.
        /// </exception>
        /// <exception cref="RegexMatchTimeoutException">
        /// Thrown when regular expression is not completed.
        /// </exception>
        public void ApplyKeywordMessageFilter(string keywordMessagesFilter)
        {
            try
            {
                if (!keywordMessagesFilter.Equals(""))
                {
                    var messages = new List<Message>();
                    foreach (Message message in this.conversation.messages.Where(message => message.content.Contains(keywordMessagesFilter)))
                    {
                        foreach (string word in message.content.Split(' '))
                        {
                            if (string.Equals(Regex.Replace(word, @"[^\w\d]", ""), keywordMessagesFilter, StringComparison.OrdinalIgnoreCase))
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
                throw new NullReferenceException("Something went wrong while applying the keyword conversation filter.");
            }
            catch (RegexMatchTimeoutException)
            {
                throw new RegexMatchTimeoutException("Something went wrong while replacing the hidden words.");
            }
        }


        /// <summary>
        /// Replaces all the words in <parammref name="messageHiddenWords"/> with the word <parammref name="messageHiddenWordReplacement"/> to the conversation.
        /// </summary>
        /// <param name="messageHiddenWords">
        /// The set of words which are hidden from messages.
        /// </param>
        /// <param name="messageHiddenWordReplacement">
        /// The replacement word for the every word in the specified set of hidden words.
        /// </param>
        /// <exception cref="NullReferenceException">
        /// Thrown when regular expression is not completed.
        /// </exception>
        /// <exception cref="RegexMatchTimeoutException">
        /// Thrown when any of the given arguments is empty.
        /// </exception>
        public void ReplaceHiddenWords(string[] messageHiddenWords, string messageHiddenWordReplacement)
        {
            try
            {
                foreach (string hiddenWord in messageHiddenWords)
                {
                    if (!hiddenWord.Equals(""))
                    {
                        foreach (Message message in this.conversation.messages)
                        {
                            foreach (string word in message.content.Split(' '))
                            {
                                if (string.Equals(Regex.Replace(word, @"[^\w\d]", ""), hiddenWord, StringComparison.OrdinalIgnoreCase))
                                {
                                    message.content = Regex.Replace(message.content, word, messageHiddenWordReplacement, RegexOptions.IgnoreCase);
                                }
                            }
                        }
                    }
                }
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException("Something went wrong while replacing the hidden words.");
            }
            catch (RegexMatchTimeoutException)
            {
                throw new RegexMatchTimeoutException("Something went wrong while replacing the hidden words.");
            }
        }

    }
}
