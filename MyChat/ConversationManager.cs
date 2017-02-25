using MindLink.Recruitment.MyChat.Actions;
using MindLink.Recruitment.MyChat.Elements;
using MindLink.Recruitment.MyChat.FilesProcessing;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MindLink.Recruitment.MyChat.Elements.Conversation;

namespace MindLink.Recruitment.MyChat
{
    class ConversationsManager
    {

        /// <summary>
        /// The input file path.
        /// </summary>
        private string inputFilePath;

        /// <summary>
        /// The output file path.
        /// </summary>
        private string outputFilePath;

        private FileExporter fileExporter = new FileExporter();
        private FileImporter fileImporter = new FileImporter();

        private Conversation curentConversation;
        private List<ConversationAction> actions = new List<ConversationAction>();

        /// <summary>
        /// we ensure that we only have on instance of ConversationManager
        /// </summary>
        private ConversationsManager() {
        }

        /// <summary>
        /// Return the instace of the ConversationManager object
        /// </summary>
        public static ConversationsManager GetInstance
        {
            get
            {
                return ConversationsManagerImpl.instance;
            }
        }

        /// <summary>
        /// initialize the ConversationManager object
        /// </summary>
        private class ConversationsManagerImpl
        {
            static ConversationsManagerImpl()
            {

            }

            internal static readonly ConversationsManager instance = new ConversationsManager();
        }

        public void PerformActions() {
            actions.ForEach(i => i.PerformOn(curentConversation));
        }

        /// <summary>
        /// Return the input file or set the input file
        /// </summary>
        public String InputFilePath
        {
            get
            {
                return inputFilePath;
            }
            set
            {
                inputFilePath = value;
            }
        }

        /// <summary>
        /// Return the output file or set the output file
        /// </summary>
        public String OutputFilePath
        {
            get
            {
                return outputFilePath;
            }
            set
            {
                outputFilePath = value;
            }
        }

        public void addAction(ConversationAction myAction) {
            actions.Add(myAction);
        }

        public void loadConversation() {
            curentConversation = fileImporter.ReadConversationFromTextFile(inputFilePath);
        }

        public void exportConversation() {
            fileExporter.WriteConversationToJson(ConversationToJson(curentConversation), outputFilePath);
        }

        public JObject converedConversationToJson() {
            return ConversationToJson(curentConversation);
        }

        private JObject ConversationToJson(Conversation conversation) {
            JObject JconConversation = new JObject();
            JconConversation.Add(new JProperty("name", conversation.Name));
            if (conversation.SendersInfoSorted != null && conversation.SendersInfoSorted.Count > 0 )
            {
                JArray Jsenders = new JArray();
                if (conversation.HideSenders)
                {
                    JconConversation.Add(new JProperty("most active user", conversation.MostActiveSender.senderHidenID));
                    foreach (SenderInfo senderInfo in conversation.SendersInfoSorted)
                    {
                        JObject temp = new JObject();
                        temp.Add(new JProperty("sender", senderInfo.sender.senderHidenID));
                        temp.Add(new JProperty("number of messages", senderInfo.count));
                        Jsenders.Add(temp);
                    }
                }
                else {
                    foreach (SenderInfo senderInfo in conversation.SendersInfoSorted)
                    {
                        JObject temp = new JObject();
                        JconConversation.Add(new JProperty("most active user", conversation.MostActiveSender.senderID));
                        temp.Add(new JProperty("sender", senderInfo.sender.senderID));
                        temp.Add(new JProperty("number of messages", senderInfo.count));
                        Jsenders.Add(temp);
                    }
                }

                JconConversation.Add("Report", Jsenders);
            }

            JArray Jmsg = new JArray();
            foreach (Message msg in conversation.Messages)
            {
                JObject temp = new JObject();
                temp.Add(new JProperty("content", msg.Content));
                temp.Add(new JProperty("timestamp", msg.Timestamp));
                if(conversation.HideSenders) temp.Add(new JProperty("senderId", msg.msgSender.senderHidenID));
                else temp.Add(new JProperty("senderId", msg.msgSender.senderID));
                Jmsg.Add(temp);
            }
            JconConversation.Add("messages",Jmsg);

            return JconConversation;
        }


    }
}
