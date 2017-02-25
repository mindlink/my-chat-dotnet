using MindLink.Recruitment.MyChat.Actions;
using MindLink.Recruitment.MyChat.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat.Sates
{
    class Initializing : State
    {
        string[] args;
        ConversationsManager cm;

        public  Initializing(string[] arguments, ConversationsManager CM)
        {
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
                switch (parameterName.ToLower())
                {
                    case "/i":

                        i++;
                        if (i < args.Length)
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
                        if (i < args.Length)
                        {
                            cm.addAction(new FilterUser(args[i]));
                        }
                        else
                        {
                            curent = new DispalyHelp();
                            curent.setErrorMsg("Expected a file to be specified with the input file parameter.");
                            return curent;
                        }
                        break;
                    case "/hs":
                        cm.addAction(new HideSenderID());
                        break;
                    case "/r":
                        cm.addAction(new CrateReport());
                        break;
                    case "/ct":
                            cm.addAction(new HideCreditcartPhone());
                        break;
                    case "/w":

                        i++;
                        if (i < args.Length)
                        {
                            cm.addAction(new FilterWord(args[i]));
                        }
                        else
                        {
                            curent = new DispalyHelp();
                            curent.setErrorMsg("Expected a file to be specified with the input file parameter.");
                            return curent;
                        }
                        break;
                    case "/b":

                        i++;
                        if (i < args.Length)
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
                            curent.setErrorMsg("Expected a file to be specified with the input file parameter.");
                            return curent;
                        }
                        break;
                    case "/o":
                        i++;
                        if (i < args.Length)
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
                        //showHelp = true;
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
