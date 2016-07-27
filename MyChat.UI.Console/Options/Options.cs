using System.Collections.Generic;

namespace MindLink.Recruitment.MyChat.UI.Console.Options
{
    public sealed class Options
    {
        /// <summary>
        /// The name of the input file
        /// </summary>
        public string InputFile { get; set; }

        /// <summary>
        /// The name of the generated output file
        /// </summary>
        public string OutputFile { get; set; }

        /// <summary>
        /// If specified, the exported file will only consider messages of this user.
        /// </summary>
        public string UserFilter { get; set; }

        /// <summary>
        /// If specified only messages which contain the specified keyword will be included in the result.
        /// </summary>
        public string ContentKeywordFilter { get; set; }

        /// <summary>
        /// Keywords to censor in the result.
        /// </summary>
        public List<string> KeywordsToCensor { get; set; }

        /// <summary>
        /// If true, sensitive information such as credit card and telephone numbers will be censored.
        /// </summary>
        public bool CensorSensitiveInformation { get; set; }

        /// <summary>
        /// If specified, the user of each message will be obfuscated.
        /// </summary>
        public bool ObfuscateUser { get; set; }

        /// <summary>
        /// If specified, the result will include a report with the most active users in the conversation.
        /// </summary>
        public bool GenerateMostActiveUsersReport { get; set; }
    }
}
