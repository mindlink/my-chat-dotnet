using System;
using System.Collections.Generic;
using System.Text;

namespace MindLink.Recruitment.MyChat
{
    /// <summary>
    /// Reads conversation data from drive according to a <see cref="ConversationConfig"/>
    /// </summary>
    public interface IConversationReader
    {
        /// <summary>
        /// Reads a conversation from 'configuration.InputFilePath' into a <see cref="Conversation"/> object.
        /// </summary>
        /// <param name="configuration"></param>
        /// The conversation configuration object
        /// <returns></returns>
        Conversation ReadConversation(ConversationConfig configuration);
    }
}
