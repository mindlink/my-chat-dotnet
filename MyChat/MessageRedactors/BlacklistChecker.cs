using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MindLink.Recruitment.MyChat
{
    public class BlacklistChecker
    {
        public List<string> Blacklist { get; set; }

        //Constructor for BlacklistChecker that uses a path to a txt file containing a list of words
        public BlacklistChecker(string path)
        {
            Blacklist = new List<string>();
            FillBlacklist(path);
            
        }


        //Constructor for BlacklistChecker that uses a list of words as a parameter
        public BlacklistChecker(List<string> list)
        {
            Blacklist = list;
        }


        public Conversation CheckConversation(Conversation conversation)
        {
            if (Blacklist.Count > 0)
            {
                var result =  new List<Message>();
                foreach (var word in Blacklist)
                {
                    foreach (var message in conversation.Messages)
                    {
                        message.Content = message.Content.Replace(word, "*redacted*");
                        result.Add(message);
                    }
                }

                return new Conversation(conversation.Name,result);
            }
            else
            {
                return conversation;
            }
        }

        private void FillBlacklist(string path)
        {
            try
            {
                var reader = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read), Encoding.ASCII);
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Blacklist.Add(line);
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("An exception was caught when attempting to fill the BlackList from a path : " + ex.Message + ex.StackTrace);

            }
            catch (IOException ex)
            {
                Console.WriteLine("An exception was caught when attempting to fill the BlackList from a path : " + ex.Message + ex.StackTrace); 
            }
        }
    }
}
