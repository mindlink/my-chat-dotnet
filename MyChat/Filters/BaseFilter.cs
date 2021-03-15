using System;
using System.Collections.Generic;
using System.Text;

namespace MindLink.Recruitment.MyChat.Filters
{
    //base filter class that other specialized filters derive from,
    //allowing expansion of the filters
    public abstract class BaseFilter
    {
        public string[] filterValues;

        public BaseFilter(string[] filterValues)
        {
            this.filterValues = filterValues;
        }

        //base overridable method that has the filter check the 3 inputs
        public abstract string[] FilterInput(string unixSeconds, string userId, string contents);
    }
}
