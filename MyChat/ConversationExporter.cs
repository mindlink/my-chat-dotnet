using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MindLink.Recruitment.MyChat
{
    public class ConversationExporter
    {
        /*
            I decided to go with this sort of plugin/DI approach.
            But there are a couple of other different ways I had floating around in my head at the time.
            E.g
            You could define a class that do a conversion for any combo of input/output and an enum choose that on input
        */
        private IConversationReader conversationReader;
        private IConversationWriter conversationWriter;
        private IConversationFilterer conversationFilterer;

        public bool ExportConversation(ProgramOptions options)
        {
            if (options == null)
            {
                return false;
            }

            conversationReader = new TextConversationReader();
            conversationWriter = new JsonConversationWriter();
            conversationFilterer = new ConversationFilterer();

            Conversation conversation;
            try
            {
                conversation = conversationReader.ReadConversation(options.InputFile);
            }
            catch (FileNotFoundException ex) 
            {
                Console.WriteLine("Can't find the specified input file.");
                Debug.WriteLine(ex);
                return false;
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine("Can't find the directory specified in the input file.");
                Debug.WriteLine(ex);
                return false;
            }
            catch (IOException ex) 
            {
                Console.WriteLine("Something wrong with IO.");
                Debug.WriteLine(ex);
                return false;
            }

            conversation = conversationFilterer.FilterConversation(conversation, options);

            if (options.GenerateReport)
            {
                Report report = GenerateReport(conversation);
                conversation.ReportMostActiveUser = report.MostActiveUser;
                conversation.UserMessageCount = report.UserMessageCount;
            }

            try
            {
                conversationWriter.WriteConversation(conversation, options.OutputFile);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Potentially there was an issue initialising the json writer.");
                Debug.WriteLine(ex);
                return false;
            }
            catch (System.Security.SecurityException ex)
            {
                Console.WriteLine("It looks you may not have permissions to create, or write to a file.");
                Debug.WriteLine(ex);
                return false;
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Can't find the specified input file.");
                Debug.WriteLine(ex);
                return false;
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine("Can't find the directory specified in the input file.");
                Debug.WriteLine(ex);
                return false;
            }
            catch (IOException ex)
            {
                Console.WriteLine("Something wrong with IO.");
                Debug.WriteLine(ex);
                return false;
            }

            return true;
        }

        private Report GenerateReport(Conversation conversation)
        {
            Dictionary<string, int> userMessageCount = new Dictionary<string, int>();
            string mostActiveUser;

            foreach (var message in conversation.Messages)
            {
                int count;
                if (userMessageCount.TryGetValue(message.Sender, out count))
                {
                    userMessageCount[message.Sender]++;
                }
                else
                {
                    userMessageCount.Add(message.Sender, 1);
                }
            }

            userMessageCount.Values.OrderByDescending(i => i);

            mostActiveUser = userMessageCount.FirstOrDefault().Key;

            return new Report(mostActiveUser, userMessageCount);
        }
    }
}
