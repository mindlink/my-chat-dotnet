using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat.Sates
{
    class DispalyHelp : State
    {
        private string errorMsg =null;

        public override State Run()
        {
            if (!String.IsNullOrWhiteSpace(errorMsg)) {
                Console.WriteLine(" An Error has occured : "+ errorMsg);
                Console.WriteLine();
            }

            // help screen
            Console.WriteLine(" How to use conversation prosessing comand-line untility :");
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("/i       Defines the input files from where a conversation will be loaded e.g: /i inputfile.txt");
            Console.WriteLine("/o       Defines the output files where the curent conversation will be exported in json format e.g: /o outputfile.json");
            Console.WriteLine("/u       Filter conversation messages that belong to the user specified  e.g: /u userid");
            Console.WriteLine("/w       Filter conversation messages that contained the word specified e.g: /w word");
            Console.WriteLine("/b       Hide specific words in the messages contentend (replace with *redacted*) e.g: /b word1 word2 word3");
            Console.WriteLine("/ct      Hide credit cart and pohone like numbers in messages contentend e.g: /ct");
            Console.WriteLine("/hs      Obfuscate user ID with a unique random generated id e.g: /hs");
            Console.WriteLine("/r       Adds a report to the conversation that details the most active users e.g: /r");
            Console.WriteLine("/?       Dispaly help screen e.g: /?");
            Console.WriteLine("----------------------------------------------------------");
            return null;
        }

        public void setErrorMsg(string msg) {
            errorMsg = msg;

        }
    }

}
