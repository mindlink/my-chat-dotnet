using MindLink.Recruitment.MyChat.Actions;
using MindLink.Recruitment.MyChat.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat.Sates
{
    /// <summary>
    /// Process the user input
    /// </summary>
    class Initializing : State
    {
        string[] args;
        ConversationsManager cm;

        /// <summary>
        ///  Constractor
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="CM"></param>
        public  Initializing(string[] arguments, ConversationsManager CM)
        {
            if (CM == null)
            {
                throw new ArgumentNullException("ConversationsManager", String.Format("Exception in {0}, Error message : {1}",
                        this.GetType().Name + "." +System.Reflection.MethodBase.GetCurrentMethod().Name, "ConversationsManager can not be null when creating a new initializing state"));
            }

            if (arguments == null)
            {
                throw new ArgumentNullException("arguments", String.Format("Exception in {0}, Error message : {1}",
                        this.GetType().Name + "." +System.Reflection.MethodBase.GetCurrentMethod().Name, "arguments can not be null when creating a new initializing state"));
            }
            args = arguments;
            this.cm = CM;

        }

        public override State Run()
        {

            for (int i = 0; i < args.Length; i++)
            {
                string parameterName;

                parameterName = args[i];
                DispalyHelp curent;
                // parse the user arguments
                switch (parameterName.ToLower())
                {
                    case "/i":

                        i++;
                        if (i < args.Length && !String.IsNullOrWhiteSpace(args[i]))
                        {
                           cm.InputFilePath = args[i];
                        }
                        else
                        {
                            curent = new DispalyHelp();
                            curent.setErrorMsg("Expected a file to be specified with the input file parameter.");
                            return curent;
                        }

                        break;

                    case "/u":

                        i++;
                        if (i < args.Length && !String.IsNullOrWhiteSpace(args[i]))
                        {
                            cm.addAction(new FilterUser(args[i]));
                        }
                        else
                        {
                            curent = new DispalyHelp();
                            curent.setErrorMsg("Expected a userid to be specified with the filter by user parameter.");
                            return curent;
                        }
                        break;
                    case "/hs":
                        cm.addAction(new HideSenderID());
                        break;
                    case "/r":
                        cm.addAction(new CreateReport());
                        break;
                    case "/ct":
                            cm.addAction(new HideCreditcartPhone());
                        break;
                    case "/w":

                        i++;
                        if (i < args.Length && !String.IsNullOrWhiteSpace(args[i]))
                        {
                            cm.addAction(new FilterWord(args[i]));
                        }
                        else
                        {
                            curent = new DispalyHelp();
                            curent.setErrorMsg("Expected a word to be specified with the filter word parameter.");
                            return curent;
                        }
                        break;
                    case "/b":

                        i++;
                        if (i < args.Length && !String.IsNullOrWhiteSpace(args[i]))
                        {
                            HideBlackList cation = new HideBlackList();
                            while ((i < args.Length) && (!args[i].StartsWith("/")))
                            {
                                cation.addBlackListWord(args[i]);
                                 i++;
                            }
                            if (args[i].StartsWith("/"))i--;
                            cm.addAction(cation);
                        }
                        else
                        {
                            curent = new DispalyHelp();
                            curent.setErrorMsg("Expected a sequence of words with the filter black list parameter.");
                            return curent;
                        }
                        break;
                    case "/o":
                        i++;
                        if (i < args.Length && !String.IsNullOrWhiteSpace(args[i]))
                        {
                            cm.OutputFilePath = args[i];
                        }
                        else
                        {
                            curent = new DispalyHelp();
                            curent.setErrorMsg("Expected a file to be specified with the output file parameter.");
                            return curent;
                        }
                        break;
                    case "/?":
                    case "/help":
                        return new DispalyHelp();
                    default:
                        curent = new DispalyHelp();
                        curent.setErrorMsg("Unrecognized parameter : " + parameterName);
                        return curent;
                }
                
            }
            if (!(String.IsNullOrWhiteSpace(cm.InputFilePath) || String.IsNullOrWhiteSpace(cm.OutputFilePath)))
                return new Processing(cm);
            else {
                DispalyHelp curent = new DispalyHelp();
                curent.setErrorMsg("You must define input and output files");
                return curent;
            }
        }
    }
}
