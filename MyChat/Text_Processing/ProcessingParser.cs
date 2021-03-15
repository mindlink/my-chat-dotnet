using MindLink.Recruitment.MyChat.Blacklisting;
using MindLink.Recruitment.MyChat.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace MindLink.Recruitment.MyChat.Text_Processing
{
    //handles all text pre-processing before output to Json
    public sealed class ProcessingParser
    {
        public BaseFilter[] filters { get; set; }
        public IBlacklist[] blacklists { get; set; }

        //constructors
        public ProcessingParser()
        {
            filters = new BaseFilter[0];
            blacklists = new IBlacklist[0];
        }

        public ProcessingParser(BaseFilter[] filters)
        {
            this.filters = filters;
            blacklists = new IBlacklist[0];
        }

        public ProcessingParser(BaseFilter[] filters, IBlacklist[] blacklists)
        {
            this.filters = filters;
            this.blacklists = blacklists;
        }

        //runs all filters, blacklists nad future pre-processing features
        public string[] RunLineCheck(string unixSeconds, string userId, string contents)
        {
            var checkSeconds = unixSeconds;
            var checkUserId = userId;
            var checkContents = contents;

            //run filters checks
            foreach (var filter in filters)
            {
                var results = filter.FilterInput(checkSeconds, checkUserId, checkContents);

                checkSeconds = results[0];
                checkUserId = results[1];
                checkContents = results[2];
            }

            //runs blacklist checks
            foreach (var blacklist in blacklists)
            {
                var cleared = blacklist.CheckInput(checkSeconds, checkUserId, checkContents);

                checkSeconds = cleared[0];
                checkUserId = cleared[1];
                checkContents = cleared[2];
            }

            return new string[] { checkSeconds, checkUserId, checkContents};
        }

    }
}
