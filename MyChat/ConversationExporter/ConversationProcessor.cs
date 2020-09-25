using System;


namespace MindLink.Recruitment.MyChat
{
    public sealed class ConversationProcessor
    {
        public static Conversation ReadConversation(string inputFile)
        {
            return new ConversationReader(inputFile).Conversation;
        }

        public static ConversationWriter WriteConversation(Conversation conversation, string path)
        {
            return new ConversationWriter(conversation, path); 
        }

        public static Conversation FilterByID(Conversation conversation)
        {
            return new IDFilter().Filter(conversation);
        }

        public static Conversation FilterByKeyword(Conversation conversation)
        {
            return new KeywordFilter().Filter(conversation);
        }

        public static Conversation CheckBlackListWithPath(string path, Conversation conversation)
        {
            return new BlacklistChecker(path).CheckConversation(conversation);
        }

        public static Conversation HidePhoneNumbers(Conversation conversation)
        {
            PhoneNumberCover cover = new PhoneNumberCover(conversation);
            cover.Hide();
            return cover.Conversation;
        }

        public static Conversation HideCreditCardNumbers(Conversation conversation)
        {
            CreditCardCover cover = new CreditCardCover(conversation);
            cover.Hide();
            return cover.Conversation;
        }
    }
}
