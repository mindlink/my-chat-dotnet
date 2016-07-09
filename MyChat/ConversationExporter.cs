namespace MyChat
{
    using System;
    using System.Collections.Generic;
    using MindLink.Recruitment.MyChat;
    using MindLink.Recruitment.MyChat.Controlers;
    using MindLink.Recruitment.MyChat.ViewModels;
    using MyChat.Core.Managers;
    using MyChat.Core.Abstract;
    using MyChat.Core.Helpers;

    /// <summary>
    /// Represents a conversation exporter that can read a conversation and write it out in JSON.
    /// </summary>
    public sealed class ConversationExporter : ViewModelController<MyChatViewModel>
    {
        /// <summary>
        /// The application entry point.
        /// </summary>
        /// <param name="args">
        /// The command line arguments.
        /// </param>
        static void Main(string[] args)
        {
            //Inversion Of Control
            IOCManager.Register<IIOHelperBase>(() => new IOHelperBase()); 


            var conversationExporter = new ConversationExporter();
            conversationExporter.Start(new String[]{"chat.txt", "chat.json"});
        }

      
    }
}
