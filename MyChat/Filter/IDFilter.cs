 using System;
using System.Collections.Generic;

namespace MindLink.Recruitment.MyChat
{
    public class IDFilter : IFilter
    {
        public string Word { get; set; }

        public IDFilter()
        {
            Word = AskUserForKeyword();
        }
        public IDFilter(string id)
        {
            Word = id;
        }

        public Conversation Filter(Conversation conversation)
        {
            var result = new List<Message>();
            foreach (var message in conversation.Messages)
            {
                if (message.SenderId.ToLower() == Word.ToLower())
                {
                    result.Add(message);
                }
            }
            return new Conversation(conversation.Name, result);
        }

        private string AskUserForKeyword()
        {
            Console.WriteLine("Write the UserID you want to filter the conversation by");
            var userID = Console.ReadLine();

            return userID;
        }
    }
}
