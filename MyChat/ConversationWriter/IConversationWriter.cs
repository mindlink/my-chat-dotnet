﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MindLink.Recruitment.MyChat
{
    public interface IConversationWriter
    {
        /// <summary>
        /// Writes the <paramref name="conversation"/> as JSON to <paramref name="outputFilePath"/>.
        /// </summary>
        /// <param name="conversation">
        /// The conversation.
        /// </param>
        /// <param name="outputFilePath">
        /// The output file path.
        /// </param>
        void WriteConversation(Conversation conversation, string outputFilePath);
    }
}