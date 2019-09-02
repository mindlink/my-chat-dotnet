﻿namespace MindLink.Recruitment.MyChat
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Linq;
    using MindLink.Recruitment.MyChat;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a blacklist reader that can read a blacklist text file and return a hashset.
    /// </summary>
    public static class BlacklistReader
    {
        public static Blacklist TextToBlacklist(string inputFilePath)
        {
            var blacklist = new HashSet<string>() { };
            if (inputFilePath == null)
            {
                return new Blacklist(blacklist);
            }
            else
            {
                try
                {
                    var reader = new StreamReader(new FileStream(@"..\..\" + inputFilePath, FileMode.Open, FileAccess.Read),
                        Encoding.ASCII);

                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        blacklist.Add(line.ToLower());
                    }

                    return new Blacklist(blacklist);
                }
                catch (FileNotFoundException exception)
                {
                    throw new ArgumentNullException("Blacklist file could not be located / does not exist.", exception);
                }
            }
        }
    }
}
