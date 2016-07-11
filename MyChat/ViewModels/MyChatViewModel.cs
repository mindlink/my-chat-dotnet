using MindLink.Recruitment.MyChat.Core;
using MindLink.Recruitment.MyChat.Core.ViewModels;
using MyChat;
using MyChat.Core.Helpers;
using MyChat.Core.Managers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat.ViewModels
{
    public class MyChatViewModel: ViewModel
    {

        public override Conversation ExportConversation(string[] args)
        {
            ConversationExporterConfiguration configuration = DataManager.Instance.ArgsParser.ParseCommandLineArguments(args);

            if (configuration == null)
            { 
                Logger.Log("CommandLine Arguments Parsing failed. Exiting MyChat");
                return null;
            }

            return ExportConversation(configuration);
        }

        /// <summary>
        /// Exports the conversation at <paramref name="inputFilePath"/> as JSON to <paramref name="outputFilePath"/>.
        /// </summary>
        /// <param name="inputFilePath">
        /// The input file path.
        /// </param>
        /// <param name="outputFilePath">
        /// The output file path.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when a path is invalid.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown when something bad happens.
        /// </exception>
        Conversation ExportConversation(ConversationExporterConfiguration config)
        {
            Conversation conversation = DataManager.Instance.ReadConversation(config.inputFilePath);
            conversation = applyFiltersToConversation(conversation, config);

            DataManager.Instance.WriteConversation(conversation, config.outputFilePath);

            Log(String.Format( "Conversation exported from '{0}' to '{1}'", config.inputFilePath, config.outputFilePath));

            return conversation;
        }


        Conversation applyFiltersToConversation(Conversation input, ConversationExporterConfiguration config)
        {
            Conversation ret = input;

            //Apply Filtering with order of excecution
            foreach (ExtraArgument extraArgs in config.Extra)
            {
                switch (extraArgs.Filter)
                {
                    case ExtraArgument.FilterType.User:
                        ret.messages = ret.FilterMessagesForUser(extraArgs.Value);
                        break;

                    case ExtraArgument.FilterType.Keyword:
                        ret.messages = ret.FilterMessagesForKeyword(extraArgs.Value);
                        break;


                    case ExtraArgument.FilterType.HideWord:
                        ret.messages = ret.HideWordsFromMessages(extraArgs.Value);
                        break;
                }
            }
            return ret;
        }
    }
}
