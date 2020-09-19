﻿namespace MindLink.Recruitment.MyChat.Controllers
{
    using MindLink.Recruitment.MyChat.Features.Additional;
    using MindLink.Recruitment.MyChat.Features.Essential;
    using MindLink.Recruitment.MyChat.Interfaces.ControllerInterfaces;
    using MindLink.Recruitment.MyChat.Interfaces.FeatureInterfaces;
    using MyChatModel.ModelData;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;

    /// <summary>
    /// Class to filter a conversation based on the selected filters
    /// </summary>
    public sealed class FilterController : IFilterController
    {
        private IList<IStrategyFilter> filters;

        public bool FiltersToApply { get; set; }

        /// <summary>
        /// Constructor for FilterController class, empty
        /// </summary>
        public FilterController() 
        {
            // SETUP variables
            filters = new List<IStrategyFilter>();
            // FiltersToApply to false to begin with
            FiltersToApply = false;
        }

        /// <summary>
        /// FilterConversation, method which will filter the conversation based 
        /// on the filters which have been selected
        /// </summary>
        /// <param name="conversation"> <see cref="Conversation"> conversation to be filtered</param>
        /// <returns> Returns the <see cref="Conversation"> filtered conversation </returns>
        public Conversation FilterConversation(Conversation conversation) 
        {
            Conversation filteredConversation = conversation;

            // FOREACH through the list of filters
            foreach (IStrategyFilter filter in filters) 
            {
                // APPLY the filter 
                filteredConversation = filter.ApplyFilter(filteredConversation);
            }

            // RETURN the filtered conversation for exporting
            return filteredConversation;
        }

        /// <summary>
        /// CheckForFilters method, checks the additional command line strings
        /// for filter keywords and arguments
        /// </summary>
        /// <param name="filters"></param>
        public void CheckForFilters(string[] filtersToCheck) 
        {
            for (int i = 0; i < filtersToCheck.Length; i++) 
            {
                // SWITCH case to check for the different filters
                switch (filtersToCheck[i])
                {
                    case "-filter-user":
                        FiltersToApply = true;
                        this.filters.Add(new FilterByUser(filtersToCheck[i + 1]));
                        break;
                    case "-filter-search-word":
                        FiltersToApply = true;
                        this.filters.Add(new FilterByWord(filtersToCheck[i + 1]));
                        break;
                    case "-filter-blacklist-word":
                        FiltersToApply = true;
                        this.filters.Add(new FilterByBlacklist(filtersToCheck[i + 1]));
                        break;
                    case "-filter-card-details":
                        FiltersToApply = true;
                        this.filters.Add(new FilterCardDetails());
                        break;
                    case "-filter-phone-number":
                        FiltersToApply = true;
                        this.filters.Add(new FilterPhoneNumbers());
                        break;
                }
            }
        }
    }
}