using System;
using System.Text.RegularExpressions;

namespace MindLink.Recruitment.MyChat
{
    public class PhoneNumberCover : INumberCover
    {
        public Conversation Conversation { get; set; }

        //This regex string is for finding a number beetween 7 and 11 digits long, with spaces or hyphens beetween them anywhere in the string
        public string[] RegexString { get; } = new string[] { @"\b(?:\d[ -]*?){7,11}\b" };


        public PhoneNumberCover(Conversation conversation)
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
