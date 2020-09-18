using MyChatModel.ModelData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MindLink.Recruitment.MyChat.Interfaces.ControllerInterfaces
{
    public interface IFilterController
    {
        /// <summary>
        /// bool so that an easy check can be performed to see if any filters
        /// need to be applied to a conversation or not
        /// </summary>
        bool FiltersToApply { get; set; }

        /// <summary>
        /// FilterConversation, method which will filter the conversation based 
        /// on the filters which have been selected
        /// </summary>
        /// <param name="conversation"> <see cref="Conversation"> conversation to be filtered</param>
        /// <returns> Returns the <see cref="Conversation"> filtered conversation </returns>
        Conversation FilterConversation(Conversation conversation);

        /// <summary>
        /// CheckForFilters method, checks the additional command line strings
        /// for filter keywords and arguments
        /// </summary>
        /// <param name="filters"></param>
        void CheckForFilters(string[] filters);
    }
}
