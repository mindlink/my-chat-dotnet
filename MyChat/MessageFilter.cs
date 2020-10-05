using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MindLink.Recruitment.MyChat
{
    class MessageFilter:Filter,IFilterMessages
    {
        INameFilter NFilter = new FilterName();
        IKeywordFilter KFilter = new FilterKeyword();
        IRedactedWordFilter RFilter = new FilterRedacted();
        public Conversation FilterMessages(Conversation conversation)
        {
            try
            {
                // after reading text file is complete optional message filtering is after
                List<Message> messages = conversation.messages.ToList();

                Console.WriteLine("Select filter type(Name|Keyword|Redacted) type anything else to ignore");
                filterType = Console.ReadLine();

                switch (filterType.ToLower())
                {// if user wants to filter by name
                    case "name":
                        Console.WriteLine("Filter Type: By Name");
                        Console.WriteLine("Which Name?");
                        user = Console.ReadLine();
                        var NameFilteredMessages = NFilter.NameFilter(messages, user);
                        return new Conversation(conversation.name, NameFilteredMessages);

                    // if user wants to filter by keyword      
                    case "keyword":
                        Console.WriteLine("Filter Type: By Key Word");
                        Console.WriteLine("Which word?");
                        user = Console.ReadLine();
                        var wordFilteredMessages = KFilter.KeywordFilter(messages, user);
                        
                        return new Conversation(conversation.name, wordFilteredMessages);

                    // if user wants to redact a certain word in messages
                    case "redacted":
                        Console.WriteLine("Filter Type: Redact specific word");
                        Console.WriteLine("Which word?");
                        user = Console.ReadLine();
                        var redactFilteredMessages = RFilter.RedactedWordFilter(messages, user);
                        return new Conversation(conversation.name, redactFilteredMessages);

                    // shown if no filter is applied       
                    default:
                        Console.WriteLine("No Filter selected");
                        break;
                }

                return new Conversation(conversation.name, messages);//messages //filteredmessages
            }
            catch (FormatException)
            {
                throw new Exception("Input string is in incorrect format");
            }
        }
    }
}
