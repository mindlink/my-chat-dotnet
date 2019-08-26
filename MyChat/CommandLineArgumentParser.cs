namespace MindLink.Recruitment.MyChat
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Represents a helper to parse command line arguments.
    /// </summary>
    public sealed class CommandLineArgumentParser
    {
        #region Variables
        public string inputFilePath;
        public string outputFilePath;
        public string senderIdFilter;
        public string keywordFilter;
        public List<string> blacklist = new List<string>() { };
        public bool isNumberFilterActive;
        public bool isIdObfuscationActive;
        #endregion

        #region Constructor
        public CommandLineArgumentParser(string[] arguments)
        {
            ParseCommandLineArguments(arguments);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Parses the given <paramref name="arguments"/> and assigns class variables.
        /// </summary>
        private void ParseCommandLineArguments(string[] arguments)
        {

            if (Array.IndexOf(arguments, "help") > -1)
            {
                Console.Write("-I inputfile.txt\n"+
                              "-O outputfile.json\n"+
                              "Optional arguments:\n"+
                              "-U userId (Only messages sent by specified user will be exported)\n" +
                              "-B blacklist.txt (Any words matching those given in blacklist will be redacted)\n" +
                              "-K keyword (Only messages containing specified keyword will be exported)\n" +
                              "-N (If set, activates filter for phone / credit card numbers)\n"+
                              "-F (If set, obfuscates user identities)\n");
            }

            if (Array.IndexOf(arguments, "-I") > -1)
            {
                inputFilePath = arguments[Array.IndexOf(arguments, "-I") + 1];
            }
            else
            {
                throw new ArgumentException("No input file given. Input file can be specified via '-I inputfile.txt'. Type 'help' for full list of arguments.");
            }

            if (Array.IndexOf(arguments, "-O") > -1)
            {
                outputFilePath = arguments[Array.IndexOf(arguments, "-O") + 1];
            }
            else
            {
                throw new Exception("No output file given. Output file can be specified via '-O outputfile.json'. Type 'help' for full list of args.");
            }

            if (Array.IndexOf(arguments, "-U") > -1)
            {
                senderIdFilter = arguments[Array.IndexOf(arguments, "-U") + 1];
            }
            else
            {
                senderIdFilter = null;
            }

            if (Array.IndexOf(arguments, "-B") > -1)
            {
                BlacklistToList(arguments[Array.IndexOf(arguments, "-B") + 1]);
            }
            else
            {
                blacklist.Clear();
            }

            if (Array.IndexOf(arguments, "-K") > -1)
            {
                keywordFilter = arguments[Array.IndexOf(arguments, "-K") + 1];
            }
            else
            {
                keywordFilter = null;
            }

            if (Array.IndexOf(arguments, "-N") > -1)
            {
                isNumberFilterActive = true;
            }
            else
            {
                isNumberFilterActive = false;
            }

            if (Array.IndexOf(arguments, "-F") > -1)
            {
                isIdObfuscationActive = true;
            }
            else
            {
                isIdObfuscationActive = false;
            }

        }

        /// <summary>
        /// Clears existing list then reads blacklist.txt file and appends each line to list as string.
        /// </summary>
        private void BlacklistToList(string blacklist)
        {

            this.blacklist.Clear();
            var reader = new StreamReader(new FileStream(blacklist, FileMode.Open, FileAccess.Read),
                Encoding.ASCII);

            string line;

            while ((line = reader.ReadLine()) != null)
            {
                this.blacklist.Add(line);
            }
            
        }
        #endregion
    }
}
