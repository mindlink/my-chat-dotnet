using System;
using System.Collections.Generic;

namespace MindLink.Recruitment.MyChat
{
    public class KeywordFilter : IFilter
    {
        public string Word { get; set; }

        public KeywordFilter()
        {
            Word = AskUserForKeyword();
        }
        public KeywordFilter(string word)
        {
            Word = word;
        }

        public Conversation Filter(Conversation conversation)
        {
            var result = new List<Message>();
            foreach (var message in conversation.Messages)
            {
                if (message.Content.ToLower().Contains(Word.ToLower()))
                {
                    result.Add(message);
                }
            }
            return new Conversation(conversation.Name, result);
        }

        private string AskUserForKeyword()
        {
            Console.WriteLine("Write the keyword you want to filter the conversation by");
            var keyword = Console.ReadLine();

            return keyword;
        }
    }
}
