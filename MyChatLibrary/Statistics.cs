using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MyChatLibrary
{
    /// <summary>
    /// Holds Statistic Datas per User
    /// </summary>
    public sealed class Statistics
    {
        /// <summary>
        /// User ID 
        /// </summary>
        public string UserId;
        /// <summary>
        /// How many Chats for the User
        /// </summary>
        public int Chats;



        /// <summary>
        /// Returns the Google Charts Conform Chat Statistic for a Pie Chart
        /// </summary>
        /// <param name="file">
        /// File with Json content
        /// <returns>
        /// Google Chart Conform Data String
        /// </returns>
        public string getPieChartData(string file)
        {
            var stat = new List<Statistics>();
            string serializedConversation;

            try
            {
                using (var fs = new FileStream(file, FileMode.Open))
                {
                    serializedConversation = new StreamReader(fs).ReadToEnd();
                }
            }
            catch (ArgumentNullException)
            {
                return "";
            }

            Conversation savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);
            var messages = savedConversation.messages.ToList();

            var res = messages.GroupBy(fm => fm.senderId).Select(fm => new { Key = fm.Key, total = fm.Count() }).OrderBy(fm => fm.Key).OrderByDescending(fm => fm.total);

            string str = "";
            foreach (var item in res)
            {
                str += "'" + item.Key.ToString() + "'," + item.total.ToString() + "],[";
            }

            return str.Substring(0, str.Length - 2);
        }
    }
}
