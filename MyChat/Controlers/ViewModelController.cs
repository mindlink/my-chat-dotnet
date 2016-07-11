using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindLink.Recruitment.MyChat.Core.ViewModels;
using MindLink.Recruitment.MyChat.Core;

namespace MindLink.Recruitment.MyChat.Controlers
{
    public class ViewModelController<TViewModel> where TViewModel : ViewModel, new()
    {
        protected TViewModel ViewModel { get; private set; }

        public ViewModelController()
        {
            ViewModel = new TViewModel();
        }

        public Conversation Export(String[] args)
        {
            return ViewModel.ExportConversation(args);            
        }
    }
}
