﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MindLink.Recruitment.MyChat
{
    /// <summary>
    /// Responsible for filtering <see cref="Conversation"/> objects.
    /// </summary>
    public interface IConversationFilter
    {
        /// <summary>
        /// Filters a <see cref="Conversation"/> object according to a <see cref="ConversationConfig"/> object.
        /// </summary>
        /// <param name="configuration">
        /// The configuration object.
        /// </param>
        /// <param name="conversation">
        /// The conversation object.
        /// </param>
        Conversation FilterConversation(ConversationConfig configuration, Conversation conversation);
    }
}