using System.Collections.Generic;
using System.Linq;
using System;
using System.Text.RegularExpressions;
using MindLink.Recruitment.MyChat.Filters;

namespace MindLink.Recruitment.MyChat
{
    /// </summary>
    /// Provide functionality to parse individual filter depending on certain conditions set out in the configuration.
    /// </summary>
    public sealed class ConversationFilter
    {
        public Conversation newConversation { get; }
        private NameFilter FilterName;
        private KeywordFilter FilterKeyword;
        private BlacklistFilter FilterBlacklist;
        private InformationFilter FilterInfo;

        public ConversationFilter(ConversationExporterConfiguration configuration, Conversation conversation)
        {
            newConversation = conversation;

            if (configuration.FilterID == true && configuration.Blacklist == false)
            {
                newConversation = this.NameFilter(configuration.Filter, conversation);
            }
            if (configuration.Filter != null && configuration.FilterID == false)
            {
                newConversation = this.KeywordFilters(configuration.Filter, conversation);
            }
            if (configuration.Blacklist == true)
            {
                newConversation = this.BlacklistFilter(configuration, conversation);
            }
            if (configuration.PersonalNumbers == true)
            {
                newConversation = this.InformationFilter(conversation);
            }
        }

        /// <summary>
        /// Method filter by Name.
        /// </summary>
        public Conversation NameFilter(string filter, Conversation conversation)
        {
            FilterName = new NameFilter(filter);
            return FilterName.filter(filter, conversation);
        }

        /// <summary>
        /// Method to filter by Keyword.
        /// </summary>
        public Conversation KeywordFilters(string filter, Conversation conversation)
        {
            FilterKeyword = new KeywordFilter(filter);
            return FilterKeyword.filter(filter, conversation);
        }

        
        /// <summary>
        /// Method to redact blacklisted words.
        /// </summary>
        public Conversation BlacklistFilter(ConversationExporterConfiguration configuration, Conversation conversation)
        {
            FilterBlacklist = new BlacklistFilter();
            return FilterBlacklist.filter(configuration, conversation);
        }

        /// <summary>
        /// Method to redact personal information.
        /// </summary>
        public Conversation InformationFilter(Conversation conversation)
        {
            FilterInfo = new InformationFilter();
            return FilterInfo.filter(conversation);
        }
    }
}
