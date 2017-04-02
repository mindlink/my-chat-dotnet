using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Text;
using MindLink.Recruitment.MyChat;
using Newtonsoft.Json;

namespace MyChat
{
    /// <summary>
    /// Represents a conversation exporter that can read a conversation and write it out in JSON.
    /// </summary>
    public sealed class ConversationExporter
    {
        /// <summary>
        /// The application entry point.
        /// </summary>
        /// <param name="args">
        /// The command line arguments.
        /// </param>
        static void Main(string[] args)
        {
            var conversationExporter = new ConversationExporter();

            //An instance of this class contains arguments read from args[] argument
            CommandLineArgumentParser options = new CommandLineArgumentParser();

            //ExportCreteria holds the output preferences of the user;
            ExportCreteria exportCreteria;

            /// ---------- Program options: 
            ///-i input,the input file
            ///-o output, the output file
            ///-f user , filter by user 
            ///-k keyword, filter by keyword
            ///-L word1,word2,...,wordn  -- set black list
            ///-r include report
            ///-h hide user id
           
            
            try
            {
                if (CommandLine.Parser.Default.ParseArguments(args, options))                   
                {
                    //Export creteria initialization
                    exportCreteria = new ExportCreteria(options.InputFile, options.OutputFile); 
                    exportCreteria.HideUserName = options.HideUserId;

                    if(options.User != null)
                        exportCreteria.Export_by_User = new User(options.User);

                    if(options.Keyword != null) 
                        exportCreteria.Export_by_Keyword = options.Keyword;

                    if (options.blackList != null)
                        exportCreteria.SetBlackListItems(options.blackList);

                    exportCreteria.IncludeReport = options.IncludeReport;
          
                    //ExportHandler contains a serie of functions
                    //for exporting a convertation, based on exportCreteria defined
                    //defined by the user
                    ExportHandler.ExportConversation(exportCreteria);

                    ///successful export
                    Console.WriteLine("Conversation exported successfully.");
                }

            }
            catch (FileNotFoundException fileNotFoundException)
            {
                Console.WriteLine(fileNotFoundException.Message);
            }
            catch (IOException ioException)
            {
                Console.WriteLine(ioException.Message);
            }
            catch (ArgumentNullException nullArgumentException)
            {
                Console.WriteLine(nullArgumentException.Message);
            }
            finally
            {
                Console.WriteLine("Press any key to continue");
                Console.Read();
            }
        }   

      
    }
}
