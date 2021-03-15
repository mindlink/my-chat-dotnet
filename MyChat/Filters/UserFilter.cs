using System;
using System.Collections.Generic;
using System.Text;

namespace MindLink.Recruitment.MyChat.Filters
{
    public class UserFilter : BaseFilter
    {
        //initialises parent filter as well
        public UserFilter(string[] filterValues) : base(filterValues)
        {
        }

        //filter only selects messages from a select group of users
        public override string[] FilterInput(string unixSeconds, string userId, string contents)
        {
            foreach (var allowed in filterValues)
            {
                if (userId.Equals(allowed))
                {
                    //returns the info back as it is fits the filter requirement
                    return new string[] { unixSeconds, userId, contents };
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
