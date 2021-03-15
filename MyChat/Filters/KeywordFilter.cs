using System;
using System.Collections.Generic;
using System.Text;

namespace MindLink.Recruitment.MyChat.Filters
{
    public class KeywordFilter : BaseFilter
    {
        //initialises parent abstract class
        public KeywordFilter(string[] filterValues) : base(filterValues)
        {
        }

        //filters and selects only messages with keywords
        public override string[] FilterInput(string unixSeconds, string userId, string contents)
        {
            foreach (var item in filterValues)
            {
                string testingWord = "";
                for (int i = 0; i < contents.Length; i++)
                {
                    if (contents[i].Equals(' ')||(i==contents.Length-1))
                    {
                        if ((i == contents.Length - 1)&&(!Char.IsPunctuation(contents[i])))
                            testingWord = testingWord + contents[i];

                        if (testingWord.Equals(item))
                        {
                            return new string[] { unixSeconds, userId, contents };
                        }
                        testingWord = "";
                    }
                    else if ((!Char.IsPunctuation(contents[i]))||(contents[i].Equals('\'')))
                    {
                        testingWord = testingWord + contents[i];
                    }
                }
            }

            //clears message information if not a filtered value
            unixSeconds = "";
            userId = "";
            contents = "";

            return new string[] { unixSeconds, userId, contents };
        }
    }
}
