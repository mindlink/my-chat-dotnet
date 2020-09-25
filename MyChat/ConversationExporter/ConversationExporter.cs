using System;
using MindLink.Recruitment.MyChat;

namespace MyChat
{
    public sealed class ConversationExporter
    {
        public Conversation Conversation { get; set; }

        public void ExportConversation(ConversationExporterConfiguration configuration)
        {
            Conversation = ConversationProcessor.ReadConversation(configuration.InputFilePath);

            if (configuration.FilterType == "IdFiltering")
            {
               
                Conversation = ConversationProcessor.FilterByID(Conversation);
            }
            else if (configuration.FilterType == "KeywordFiltering")
            {

                Conversation = ConversationProcessor.FilterByKeyword(Conversation);
            }

            Conversation = ConversationProcessor.CheckBlackListWithPath("blacklist.txt", Conversation);


            Conversation = ConversationProcessor.HidePhoneNumbers(Conversation);
            Conversation = ConversationProcessor.HideCreditCardNumbers(Conversation);


            ConversationWriter writer = ConversationProcessor.WriteConversation(Conversation, configuration.OutputFilePath);

            if (writer.Successful)
            {
                Console.WriteLine($"Conversation exported from {configuration.InputFilePath} to {configuration.OutputFilePath}");
            }
          
        }
        
    }
}
