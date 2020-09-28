using System;
using System.Linq;
using System.Text;

namespace MindLink.Recruitment.MyChat
{
    public class KeywordFilterer : IFilterer
    {
        public IFilterer NextFilterer { get; set; }
        
        public string Keyword { get; set; }

        public KeywordFilterer(string k)
        {
            Keyword = k;
        }
        
        public bool Filter(IMessage m)
        {
            //Does the message contain the keyword?
            var found = KeywordInMessage(m);
            
            // Do extra validation work if we passed this round of validation
            // and if there's actually more to do.
            if(found && NextFilterer != null)
            {
                return NextFilterer.Filter(m);
                
            }
            return found;
        }
        
        public bool KeywordInMessage(IMessage m)
        {
            //KeywordInMessage will take a Message object and search if the Keyword instance variable is included.
            //Upper and lowercase orthographic differences DO NOT matter. Hello and hello are the same.
            //If the equality test for two lowercase words fails, it will also attempt to make sure we're not being foxed
            //by punctuation. Therefore, if a user wants "foxed" and the message contains "foxed!", we'll find it.
            return m.content.Split(" ").Any(word => word.ToLower() == Keyword.ToLower() ||
                                                    StripPunctuation(word).ToLower() == Keyword.ToLower());
        }
        
        public string StripPunctuation(string word)
        {
            //StripPunctuation and remove all punctuation from it.
            var builder = new StringBuilder();
            foreach (var c in word)
            {
                if (!char.IsPunctuation(c))
                {
                    builder.Append(c);
                }
            }
        
            return builder.ToString();
        }
    }
    
}