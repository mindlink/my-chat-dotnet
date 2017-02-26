using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat.States
{
    /// <summary>
    ///  Executing the user input
    /// </summary>
    class Processing : State
    {
        ConversationsManager cm;
        /// <summary>
        /// constractor
        /// </summary>
        /// <param name="CM"></param>
        public Processing(ConversationsManager CM)
        {
            if (CM == null)
            {
                throw new ArgumentNullException("ConversationsManager", String.Format("Exception in {0}, Error message : {1}",
                        this.GetType().Name + "." +System.Reflection.MethodBase.GetCurrentMethod().Name, "ConversationsManager can not be null when creating a new Processing state"));
            }
            this.cm = CM;

        }
        public override State Run()
        {
            cm.loadConversation();
            cm.PerformActions();
            cm.exportConversation();
            DisaplyResults dresult = new DisaplyResults();
            dresult.setResultMsg("The input file : " + cm.InputFilePath + " was processed successfully and the output was stored in json format on the file : " + cm.OutputFilePath);
            return dresult;
        }
    }
}
