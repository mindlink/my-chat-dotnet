using MindLink.Recruitment.MyChat;
using MindLink.Recruitment.MyChat.Core;
using MyChat.Core.Abstract;
using MyChat.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyChat.Core.Managers
{
    public class DataManager
    {
        private static DataManager instance;
        private IIOHelper IOManager;
        private ISerialize JsonSerializer;

        public CommandLineArgumentParser ArgsParser;

        public static DataManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new DataManager();

                return instance;
            }
        }

        private DataManager()
        {
            IOManager = IOCManager.Resolve<IIOHelper>();
            JsonSerializer = IOCManager.Resolve<ISerialize>();

            ArgsParser = new CommandLineArgumentParser();

        }


        public Conversation ReadConversation(string inputFilePath)
        {
            var reader = IOManager.ReadFile(inputFilePath);

            string conversationName = reader.ReadLine();
            var messages = new List<Message>();

            string line;

            while ((line = reader.ReadLine()) != null)
            {
                var split = line.Split(' ');

                var msgs = split.Skip(2).Aggregate((current, next) => current + " " + next);

                messages.Add(new Message(IOManager.FromUnixTimeSeconds(split[0]), split[1], msgs));
            }

            reader.Close();

            return new Conversation(conversationName, messages);
        }


        public void WriteConversation(Conversation conversation, string outputFilePath)
        {
            var serialized = JsonSerializer.SerializeObect(conversation);
            IOManager.WriteFile(serialized, outputFilePath);
        }


        public void AppendTextToFile(String text )
        {
            IOManager.AppendTextToFile(text);
        }

    }
}
