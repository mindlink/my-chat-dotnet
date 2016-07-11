namespace MyChat
{
    using System;
    using System.Collections.Generic;
    using MindLink.Recruitment.MyChat;
    using MindLink.Recruitment.MyChat.Controlers;
    using MindLink.Recruitment.MyChat.ViewModels;
    using MyChat.Core.Abstract;
    using MindLink.Recruitment.MyChat.Helpers;
    using System.IO;
    using MyChat.Core.Managers;
    using MyChat.Core.Helpers;
    using System.Diagnostics;

    /// <summary>
    /// Represents a conversation exporter that can read a conversation and write it out in JSON.
    /// </summary>
    public sealed class ConversationExporter : ViewModelController<MyChatViewModel>
    {
        /// <summary>
        /// The application entry point. Using MVVM , assuming this would eventually have a User Interface and maybe going cross platform (just like most chat based apps :))
        /// </summary>
        /// <param name="args">
        /// The command line arguments.
        /// </param>
        static void Main(string[] args)
        {
            // In case something went terribly wrong and we didnt thought about it
            AppDomain.CurrentDomain.UnhandledException += (o, e) =>
            {
                var ex = e.ExceptionObject as Exception;
                Logger.Log(ex); 

                #if DEBUG
                   Debugger.Break();  // Set a breakpoint
                #endif

            };

            Setup_IOC_Modules();

           new ConversationExporter().Export(args);
        }

        /// <summary>
        /// Initialize Inversion Of Control modules , in case this app goes cross platform.
        /// </summary>
        static void Setup_IOC_Modules()
        {
            //Definetly will be platform specific API calls for IO methods, so we need to inject this for cross platform utilization
            IOCManager.Register<IIOHelper>(() => new IOHelper());

            //Inject Newtonsoft.Json as its not available to use in a class library and/or PCL ( portable class library)
            //We probably going to use a different serializer in each of our different platform ( windows , ios , android ....)
            IOCManager.Register<ISerialize>(() => new Serializer());          

        }

      
    }
}
