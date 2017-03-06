using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MyChatLibrary;

namespace MindLink.Recruitment.MyChat.Tests
{
    [TestClass()]
    public class ConversationExporterConfigurationTests
    {
        [TestMethod()]
        public void ConversationExporterConfigurationArgumentlist()
        {
            CommandLineArgumentParser clap = new CommandLineArgumentParser();
            string[] strings = new[] { "chat.txt", "chat.json", "user=robelli" };
            var res = new ConversationExporterConfiguration(strings);
            Console.WriteLine(res.userFilter);

            strings = new[] { "chat.txt", "chat.json", "keyword_include=pie" };
            res = new ConversationExporterConfiguration(strings);
            Console.WriteLine(res.keywordInclude);

            strings = new[] { "chat.txt", "chat.json", "keyword_exclude=pie" };
            res = new ConversationExporterConfiguration(strings);
            Console.WriteLine(res.keywordExclude);

            strings = new[] { "chat.txt", "chat.json", "blacklist=pie,angus" };
            res = new ConversationExporterConfiguration(strings);
            Console.WriteLine(res.blacklistFilter);

            strings = new[] { "chat.txt", "chat.json", "hideCreditCard" };
            res = new ConversationExporterConfiguration(strings);
            Console.WriteLine(res.hideCreditCard);

            strings = new[] { "chat.txt", "chat.json", "hidePhone" };
            res = new ConversationExporterConfiguration(strings);
            Console.WriteLine(res.hidePhone);

            strings = new[] { "chat.txt", "chat.json", "obfuscateUser" };
            res = new ConversationExporterConfiguration(strings);
            Console.WriteLine(res.obfuscateUser);
        }
    }
}