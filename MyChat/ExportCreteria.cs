using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat
{
    /// <summary>
    /// This is a class derived from ConversationExporterConfiguration 
    /// which implements tha essential export futures of the 
    /// solution
    /// </summary>
    public sealed class ExportCreteria:ConversationExporterConfiguration
    {
        /// <summary>
        /// Export Conversation by user 
        /// </summary>
        User exportByUser;

        /// <summary>
        /// export Conversation by a Keyword
        /// </summary>
        string export_by_Keyword;

        /// <summary>
        /// A list that holds blacklisted words
        /// </summary>
        List<string> blackList = new List<string>();

        /// <summary>
        /// A flag for including or not a Report
        /// </summary>
        bool includeReport = false;

        /// <summary>
        /// A flag to Obfuscate user Id's
        /// </summary>
        bool hideUserName = false;
       
        ///<summary>
        ///Property for setting the report flag
        /// </summary>
        public bool IncludeReport
        {
            get { return includeReport; }
            set { includeReport = value; }
        }

        /// <summary>
        /// Property for setting the ''hide user id's'' flag
        /// </summary>
        public bool HideUserName
        {
            get { return hideUserName; }
            set { hideUserName = value;}
        }

        /// <summary>
        /// Property for setting the username filtered user name
        /// </summary>
        public User Export_by_User
        {
            get { return exportByUser; }
            set { exportByUser = value; }
        }

        /// <summary>
        /// Property that sets or returns the keyword
        /// defined for the conversation exportation 
        /// </summary>
        public string Export_by_Keyword
        {
            get { return export_by_Keyword; }
            set { export_by_Keyword = value; }
        }

        /// <summary>
        /// Function that returns an Enumerator 
        /// that loops through the blacklist
        /// </summary>
        /// <returns></returns>
        public List<string> ReturnBlackListedWords()
        {
            return blackList;

        }

        /// <summary>
        /// function for setting the blacklist
        /// </summary>
        public void SetBlackListItems(List<string> blackList)
        {
            this.blackList = blackList;
        }

        /// <summary>
        /// A constructor with the required fields
        /// </summary>
        public ExportCreteria(string inputFile, string outputFile)
            :base(inputFile, outputFile)
        {
            Export_by_User = new User("");
            Export_by_Keyword = "";
        }

        /// <summary>
        /// A constructor with the required fields plus an initialization for the filtered user
        /// </summary>
        public ExportCreteria(string inputfile, string outputFile, User user)
            :base(inputfile,outputFile)
        {
            Export_by_User = user;
            Export_by_Keyword = "";
        }

        /// <summary>
        /// A constructor with the required fields plus an initialization for the filtered keyword
        /// </summary>
        public ExportCreteria(string inputFile, string outputFile, string keyword)
            : base(inputFile, outputFile)
        {
            Export_by_User = new User("");
            Export_by_Keyword = keyword;
        }

        /// <summary>
        /// A constructor with the required fields , plus the initialization of the filtered
        /// user and keyword
        /// </summary>
        public ExportCreteria(string inputFile, string outputFile,User user, string keyword)
            :base(inputFile,outputFile)
        {
            Export_by_User = user;
            Export_by_Keyword = keyword;
            

        }
    }
}
