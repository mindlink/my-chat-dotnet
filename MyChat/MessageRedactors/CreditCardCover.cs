using System;
using System.Text.RegularExpressions;

namespace MindLink.Recruitment.MyChat
{
    public class CreditCardCover : INumberCover
    { 
        public Conversation Conversation { get; set; }

        //This regex string will help find strings of numbers, from 13 to 16 digits long, with spaces or hyphens beetween them anywhere in the string
        public string[] RegexString { get; } = new string[] { @"\d{13,16}", @"\b(?:\d[ -]*?){13,16}\b" };

        public CreditCardCover(Conversation conversation)
        {
            Conversation = conversation;
        }

        public void Hide()
        {
            foreach (var pattern in RegexString)
            {
                foreach (var message in Conversation.Messages)
                {
                    message.Content = Regex.Replace(message.Content, pattern, "*redacted*");

                }
            }
        }
    }
}
