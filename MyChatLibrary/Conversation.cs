namespace MyChatLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Represents the model of a conversation.
    /// </summary>
    public sealed class Conversation
    {
        /// <summary>
        /// The name of the conversation.
        /// </summary>
        public string name;

        /// <summary>
        /// The messages in the conversation.
        /// </summary>
        public IEnumerable<Message> messages
        {
            get
            {
                return applyFilter();
            }

            set
            {
                messages = filteredMessages;
            }
        }


        /// <summary>
        /// The most active User in words
        /// </summary>
        public string mostActiveUser
        {
            get
            {
                // groupby, summarize and order descending
                var res = filteredMessages.GroupBy(fm => fm.senderId).Select(fm => new { Key = fm.Key, total = fm.Count() }).OrderBy(fm => fm.Key).OrderByDescending(fm => fm.total);

                try
                {
                    return string.Format("User: {0} made {1} Chats", res.ElementAt(0).Key, res.ElementAt(0).total);
                }
                catch(ArgumentOutOfRangeException)
                {
                    return "no Chat datas avaiable";
                }
            }
            set { }
        }


        /// <summary>
        /// Array with User Activity, decending
        /// </summary>
        public List<Statistics> userActivity
        {
            get
            {
                int i = 0;      // record count
                List<Statistics> _statlist = new List<Statistics>();    // return list

                // groupby, summarize and order descending
                var res = filteredMessages.GroupBy(fm => fm.senderId).Select(fm => new { Key = fm.Key, total = fm.Count() }).OrderBy(fm => fm.Key).OrderByDescending(fm => fm.total);

                // build output list
                foreach (var rec in res)
                {
                    var _stat = new Statistics();

                    _stat.UserId = res.ElementAt(i).Key;
                    _stat.Chats = res.ElementAt(i).total;

                    _statlist.Add(_stat);
                    i++;        
                }

                return _statlist;
            }
            set { }
        }

        /// <summary>
        /// The messages in the conversation applied with filter.
        /// </summary>
        private IEnumerable<Message> filteredMessages;

        /// <summary>
        /// The User Filter to be applied.
        /// </summary>
        private string userFilter;

        /// <summary>
        /// The Keyword Included Filter to be applied.
        /// </summary>
        private string keywordInclude;

        /// <summary>
        /// The Keyword Excluded Filter to be applied.
        /// </summary>
        private string keywordExclude;

        /// <summary>
        /// The Blacklist to be applied.
        /// </summary>
        private string blackList;

        /// <summary>
        /// Hide Credit Cards applied.
        /// </summary>
        private bool hideCreditCard;

        /// <summary>
        /// Hide Phone Numbers applied.
        /// </summary>
        private bool hidePhone;

        /// <summary>
        /// Obfuscate User Names
        /// </summary>
        private bool obfuscateUser;


        /// <summary>
        /// Initializes a new instance of the <see cref="Conversation"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the conversation.
        /// </param>
        /// <param name="messages">
        /// The messages in the conversation.
        /// </param>
        public Conversation(string name, IEnumerable<Message> messages, ConversationExporterConfiguration configuration)
        {
            try
            {
                this.userFilter = configuration.userFilter;
                this.keywordInclude = configuration.keywordInclude;
                this.keywordExclude = configuration.keywordExclude;
                this.blackList = configuration.blacklistFilter;
                this.hideCreditCard = configuration.hideCreditCard;
                this.hidePhone = configuration.hidePhone;
                this.obfuscateUser = configuration.obfuscateUser;
            }
            catch (NullReferenceException)       // normally in case of Unit Tests
            {
                this.userFilter = null;
                this.keywordInclude = null;
                this.keywordExclude = null;
                this.blackList = null;
                this.hideCreditCard = false;
                this.hidePhone = false;
                this.obfuscateUser = false;
            }

            this.name = name;
            this.filteredMessages = messages;
        }

        /// <summary>
        /// Redacting the content against the blacklist.
        /// </summary>
        /// <param name="content">
        /// Content/Text to be redacted.
        /// </param>
        /// <returns IEnumerable<Message>></Message>>Redacted List</returns>
        private IEnumerable<Message> redactWordlist(IEnumerable<Message> content)
        {
            foreach (var record in content)
            {
                var words = this.blackList.Split(',');

                foreach (var wrd in words)
                {
                    record.content = record.content.Replace(wrd, @"/*redacted/*");
                }
            }

            return content;
        }


        /// <summary>
        /// Redacting the content against the CreditCard Numbers.
        /// </summary>
        /// <param name="content">
        /// Content/Text to be redacted.
        /// </param>
        /// <returns IEnumerable<Message>></Message>>Redacted List</returns>
        private IEnumerable<Message> HasCreditCardNumber(IEnumerable<Message> content)
        {
            foreach (var record in content)
            {
                var temp = record.content.Replace("-", "");
                record.content = Regex.Replace(temp, @"\b4[0-9]{12}(?:[0-9]{3})?\b|\b5[1-5][0-9]{14}\b|\b3[47][0-9]{13}\b|\b3(?:0[0-5]|[68][0-9])[0-9]{11}\b|\b6(?:011|5[0-9]{2})[0-9]{12}\b|\b(?:2131|1800|35\d{3})\d{11}\b", @"/*redacted/*");
            }

            return content;
        }

        /// <summary>
        /// Redacting the content against the Phone Numbers.
        /// </summary>
        /// <param name="content">
        /// Content/Text to be redacted.
        /// </param>
        /// <returns IEnumerable<Message>></Message>>Redacted List</returns>
        private IEnumerable<Message> HasPhoneNumber(IEnumerable<Message> content)
        {
            foreach (var record in content)
            {
                record.content = Regex.Replace(record.content, @"\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d", @"/*redacted/*");
            }

            return content;
        }


        /// <summary>
        /// Shows User ID's instead of Usernames
        /// </summary>
        /// <param name="content">
        /// Content/Text to be redacted.
        /// </param>
        /// <returns IEnumerable<Message>></Message>>UserIds</returns>
        private IEnumerable<Message> isObfuscateUser(IEnumerable<Message> content)
        {
            Users _users = new Users();

            foreach (var record in content)
            {
                record.senderId = _users.addUser(record.senderId).ToString();
            }

            return content;
        }


        /// <summary>
        /// Apply the Filters.
        /// </summary>
        /// <returns IEnumerable<Message>></Message>>filtered List</returns>
        private IEnumerable<Message> applyFilter()
        {
            // Handle User Filter
            if (userFilter != null)
            {
                filteredMessages = filteredMessages.Where(p => p.senderId == userFilter);
            }

            // Handle Include Keyword Filter
            if (keywordInclude != null)
            {
                filteredMessages = filteredMessages.Where(p => p.content.Contains(keywordInclude));
            }

            // Handle Exclude Keyword Filter
            if (keywordExclude != null)
            {
                filteredMessages = filteredMessages.Where(p => !p.content.Contains(keywordExclude));
            }

            // Handle Blacklist
            if (blackList != null)
            {
                filteredMessages = redactWordlist(filteredMessages);
            }

            // Handle Hide Credit Cards
            if (hideCreditCard)
            {
                filteredMessages = HasCreditCardNumber(filteredMessages);
            }

            // Handle Hide Phone Numbers
            if (hidePhone)
            {
                filteredMessages = HasPhoneNumber(filteredMessages);
            }

            // Handle Obfuscate Usernames
            if (obfuscateUser)
            {
                filteredMessages = isObfuscateUser(filteredMessages);
            }

            return filteredMessages;
        }
    }
}