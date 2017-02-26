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
    public class ConversationsManager
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


        // perform all the actions
        public void PerformActions()
        {
            //sort actions based on priority we execute first the report action them the filtering actions and lastly the hide actions
            actions = actions.OrderByDescending(o => (int)o.ActionPriority).ToList();
            actions.ForEach(i => i.PerformOn(curentConversation));
        }

        /// <summary>
        /// Return actions list
        /// </summary>
        public List<ConversationAction> Actions
        {
            get
            {
                return actions;
            }

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

        // Add action to the actions list
        public void addAction(ConversationAction myAction) {
            //if word is empty we throw an exception
            if (myAction == null)
            {
                throw new ArgumentNullException("myAction", String.Format("Exception in {0}, Error message : {1}",
                        this.GetType().Name + "." +System.Reflection.MethodBase.GetCurrentMethod().Name, "myAction can nto be bull when adding a new action to the action to perform list"));
            }
            actions.Add(myAction);
        }

        // load conversation from the pre define file
        public void loadConversation() {
            curentConversation = fileImporter.ReadConversationFromTextFile(inputFilePath);
        }

        // export conversation from the pre define file
        public void exportConversation() {
            fileExporter.WriteConversationToJson(ConversationToJson(curentConversation), outputFilePath);
        }

        // retrun the curent conversation to a json odject
        public JObject converedConversationToJson() {
            return ConversationToJson(curentConversation);
        }

        // convert a conversation to a json objet
        private JObject ConversationToJson(Conversation conversation) {
            JObject JconConversation = new JObject();
            JconConversation.Add(new JProperty("name", conversation.Name));
            if (conversation.SendersInfoSorted != null && conversation.SendersInfoSorted.Count > 0 )
            {
                JArray Jsenders = new JArray();
                //create most active users repot
                if (conversation.HideSenders) JconConversation.Add(new JProperty("MostActiveSender", conversation.MostActiveSender.senderHidenID));
                else JconConversation.Add(new JProperty("MostActiveSender", conversation.MostActiveSender.senderID));
       
                foreach (SenderInfo senderInfo in conversation.SendersInfoSorted)
                    {
                        JObject temp = new JObject();
                        // hide sender id if the hide id flags was selected
                        if (conversation.HideSenders) temp.Add(new JProperty("sender", senderInfo.sender.senderHidenID));
                        else temp.Add(new JProperty("sender", senderInfo.sender.senderID));
                        temp.Add(new JProperty("messages", senderInfo.count));
                        Jsenders.Add(temp);
                    }
                JconConversation.Add("report", Jsenders);
            }
            // convert converasation to json
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
